using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;
using FinanceApi.Domain.Enums;
using FinanceApi.Domain.Interfaces;
using FinanceApi.Domain.ValueObjects;

namespace FinanceApi.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetCategoryRepository _budgetCategoryRepository;

    public TransactionService(ITransactionRepository transactionRepository,
        IBudgetCategoryRepository budgetCategoryRepository)
    {
        _transactionRepository = transactionRepository;
        _budgetCategoryRepository = budgetCategoryRepository;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        return await _transactionRepository.GetAllTransactionsAsync();
    }

    public async Task<IEnumerable<Transaction>> GetLastFiveTransactions()
    {
        return await _transactionRepository.GetLastFiveTransactionsAsync();
    }

    public async Task<Transaction> GetTransactionsById(int transactionId)
    {
        return await _transactionRepository.GetTransactionByIdAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategory(CategoryType categoryType)
    {
        return await _transactionRepository.GetTransactionsByCategoryAsync(categoryType);
    }

    public async Task<bool> AddTransaction(TransactionDto transactionDto)
    {
        var transaction = await CreateSingleTransaction(transactionDto);
        return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> UpdateTransaction(TransactionDto transactionDto)
    {
        var transaction = await CreateSingleTransaction(transactionDto);
        return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> RemoveTransactionById(int transactionId)
    {
        return await _transactionRepository.RemoveTransactionByIdAsync(transactionId);
    }

    public async Task<bool> AddRepeatingMonthlyTransaction(TransactionDto transactionDto)
    {
        MonthlySchedule monthlySchedule =
            new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        var schedule = monthlySchedule.GenerateDates(transactionDto.SelectedDays!);

        return await ProcessTransactions(transactionDto, schedule);
    }

    public async Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDto transactionDto)
    {
        BiWeeklySchedule biWeeklySchedule =
            new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        var schedule = biWeeklySchedule.GenerateDates(transactionDto.SelectedDays!);

        return await ProcessTransactions(transactionDto, schedule);
    }

    private async Task<bool> ProcessTransactions(TransactionDto transactionDto, RecurringSchedule schedule)
    {
        var transactions = new List<Transaction>();

        foreach (var date in schedule)
        {
            var transaction = await CreateRepeatingTransaction(transactionDto, date);
            transactions.Add(transaction);
        }

        var budgetCategory = transactions.First().BudgetCategory;

        if (budgetCategory != null)
        {
            var totalBudgetCategoryTransactionAmount = await GetTotalBudgetTransactionAmount(budgetCategory);
            var totalTransactionAmount = GetTotalTransactionAmount(transactions);

            totalTransactionAmount += totalBudgetCategoryTransactionAmount;
            transactions.First().ValidateRepeatingTransactionBudget(totalTransactionAmount);
        }

        foreach (var transaction in transactions)
        {
            var result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result) return false;
        }

        return true;
    }

    private async Task<Transaction> CreateSingleTransaction(TransactionDto transactionDto)
    {
        BudgetCategory? budgetCategory = null;

        if (transactionDto.BudgetCategoryId != null)
            budgetCategory = await _budgetCategoryRepository
                .GetBudgetCategoryByIdAsync(transactionDto.BudgetCategoryId!.Value);

        Transaction transaction = new()
        {
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            EndDate = transactionDto.EndDate,
            IsRecurring = false,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = budgetCategory
        };

        transaction.Validate();

        if (transaction.BudgetCategory == null)
            return transaction;

        var totalBudgetCategoryTransactionAmount = await GetTotalBudgetTransactionAmount(budgetCategory!);
        transaction.ValidateTransactionBudget(totalBudgetCategoryTransactionAmount);

        return transaction;
    }

    private async Task<Transaction> CreateRepeatingTransaction(TransactionDto transactionDto, DateTime date)
    {
        BudgetCategory? budgetCategory = null;

        if (transactionDto.BudgetCategoryId != null)
            budgetCategory = await _budgetCategoryRepository
                .GetBudgetCategoryByIdAsync(transactionDto.BudgetCategoryId!.Value);

        Transaction transaction = new()
        {
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            Date = date,
            EndDate = transactionDto.EndDate,
            IsRecurring = true,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = budgetCategory
        };

        return transaction;
    }

    private async Task<decimal> GetTotalBudgetTransactionAmount(BudgetCategory budgetCategory)
    {
        var transactions = await _transactionRepository
            .GetTransactionsByBudgetCategoryAsync(budgetCategory);
        return transactions.Sum(t => t.Amount);
    }

    private static decimal GetTotalTransactionAmount(IEnumerable<Transaction> transactions)
    {
        return transactions.Sum(t => t.Amount);
    }

    public async Task<decimal> GetIncomeTransactionBalance()
    {
        return await _transactionRepository.GetIncomeTransactionBalanceAsync();
    }

    public async Task<decimal> GetExpenseTransactionBalance()
    {
        return await _transactionRepository.GetExpenseTransactionBalanceAsync();
    }
}