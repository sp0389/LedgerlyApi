using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;
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

    public async Task<Transaction> GetTransactionsById(int transactionId)
    {
        return await _transactionRepository.GetTransactionByIdAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByBudgetCategory(BudgetCategory budgetCategory)
    {
        return await _transactionRepository.GetTransactionsByBudgetCategoryAsync(budgetCategory);
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
        var schedule = monthlySchedule.GenerateDates(transactionDto.SelectedDays);

        foreach (var date in schedule)
        {
            var transaction = CreateRepeatingTransaction(transactionDto, date);
            var result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result) return false;
        }

        return true;
    }

    public async Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDto transactionDto)
    {
        BiWeeklySchedule biWeeklySchedule =
            new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        var schedule = biWeeklySchedule.GenerateDates(transactionDto.SelectedDays);

        foreach (var date in schedule)
        {
            var transaction = CreateRepeatingTransaction(transactionDto, date);

            var result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result) return false;
        }

        return true;
    }

    private async Task<Transaction> CreateSingleTransaction(TransactionDto transactionDto)
    {
        var budgetCategory = await _budgetCategoryRepository
            .GetBudgetCategoryByIdAsync(transactionDto.BudgetCategoryId);

        Transaction transaction = new()
        {
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            EndDate = transactionDto.EndDate,
            IsRecurring = false,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = budgetCategory,
        };
        
        var totalBudgetCategoryTransactionAmount = await GetTotalBudgetTransactionAmount(budgetCategory);
        transaction.ValidateTransactionBudget(totalBudgetCategoryTransactionAmount);
        
        return transaction;
    }

    private static Transaction CreateRepeatingTransaction(TransactionDto transactionDto, DateTime date)
    {
        Transaction transaction = new()
        {
            
            Amount = transactionDto.Amount,
            Date = date,
            EndDate = transactionDto.EndDate,
            IsRecurring = true,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
        };
        
        return transaction;
    }

    private async Task<decimal> GetTotalBudgetTransactionAmount(BudgetCategory budgetCategory)
    {
        var transactions = await _transactionRepository
            .GetTransactionsByBudgetCategoryAsync(budgetCategory);
        return transactions.Sum(t => t.Amount);
    }
}