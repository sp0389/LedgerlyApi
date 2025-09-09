using LedgerlyApi.Application.Interfaces;
using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Application.DTO;
using LedgerlyApi.Domain.Enums;
using LedgerlyApi.Domain.Interfaces;
using LedgerlyApi.Domain.ValueObjects;

namespace LedgerlyApi.Application.Services;

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

    public async Task<bool> AddTransaction(TransactionDto transactionDto, string userId)
    {
        var transaction = await CreateSingleTransaction(transactionDto, userId);
        return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> UpdateTransaction(TransactionDto transactionDto)
    {
        var transaction = new Transaction
        {
            Id = transactionDto.Id,
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            EndDate = transactionDto.EndDate,
            IsRecurring = transactionDto.IsRecurring,
            Description = transactionDto.Description,
            UserId = transactionDto.UserId,
            TransactionType = transactionDto.TransactionType
        };

        return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> RemoveTransactionById(int transactionId)
    {
        return await _transactionRepository.RemoveTransactionByIdAsync(transactionId);
    }

    public async Task<bool> AddRepeatingWeeklyTransaction(TransactionDto transactionDto, string userId)
    {
        WeeklySchedule weeklySchedule =
            new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        var schedule = weeklySchedule.GenerateDates(transactionDto.SelectedDays!, 7);
        
        return await ProcessTransactions(transactionDto, schedule, userId);
    }
    
    public async Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDto transactionDto, string userId)
    {
        BiWeeklySchedule biWeeklySchedule =
            new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        var schedule = biWeeklySchedule.GenerateDates(transactionDto.SelectedDays!, 14);

        return await ProcessTransactions(transactionDto, schedule, userId);
    }

    public async Task<bool> AddRepeatingMonthlyTransaction(TransactionDto transactionDto, string userId)
    {
        MonthlySchedule monthlySchedule =
            new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        var schedule = monthlySchedule.GenerateDates(transactionDto.SelectedDays!, 1);

        return await ProcessTransactions(transactionDto, schedule, userId);
    }
    
    private async Task<bool> ProcessTransactions(TransactionDto transactionDto, RecurringSchedule schedule, string userId)
    {
        var transactions = new List<Transaction>();

        foreach (var date in schedule)
        {
            var transaction = await CreateRepeatingTransaction(transactionDto, date, userId);
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

    private async Task<Transaction> CreateSingleTransaction(TransactionDto transactionDto, string userId)
    {
        BudgetCategory? budgetCategory = null;

        if (transactionDto.BudgetCategoryId != null)
            budgetCategory = await _budgetCategoryRepository
                .GetBudgetCategoryByIdAsync(transactionDto.BudgetCategoryId!.Value);

        var transaction = new Transaction
        {
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            EndDate = transactionDto.EndDate,
            IsRecurring = false,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            UserId = userId,
            BudgetCategory = budgetCategory
        };

        transaction.Validate();

        if (transaction.BudgetCategory == null)
            return transaction;

        var totalBudgetCategoryTransactionAmount = await GetTotalBudgetTransactionAmount(budgetCategory!);
        transaction.ValidateTransactionBudget(totalBudgetCategoryTransactionAmount);

        return transaction;
    }

    private async Task<Transaction> CreateRepeatingTransaction(TransactionDto transactionDto, DateTime date, string userId)
    {
        BudgetCategory? budgetCategory = null;

        if (transactionDto.BudgetCategoryId != null)
            budgetCategory = await _budgetCategoryRepository
                .GetBudgetCategoryByIdAsync(transactionDto.BudgetCategoryId!.Value);

        var transaction = new Transaction
        {
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            Date = date,
            EndDate = transactionDto.EndDate,
            IsRecurring = true,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            UserId = userId,
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

    public async Task<decimal> GetTransactionBalance(TransactionType transactionType)
    {
        return await _transactionRepository.GetTransactionBalanceAsync(transactionType);
    }
    
    public async Task<IEnumerable<decimal>> GetMonthlyTransactionAmountsForYear(int year,
        TransactionType transactionType)
    {
        return await _transactionRepository.GetMonthlyTransactionAmountsForYearAsync(year, transactionType);
    }

    public async Task<IEnumerable<Transaction>> GetPagedTransactions(int page, int pageSize)
    {
        return  await _transactionRepository.GetPagedTransactionsAsync(page, pageSize);
    }

    public async Task<int> GetTotalTransactionCount()
    {
        return await _transactionRepository.GetTotalTransactionCountAsync();
    }

    public async Task<decimal> GetTotalTransactionBalance()
    {
        return await _transactionRepository.GetTotalTransactionBalanceAsync();
    }
}