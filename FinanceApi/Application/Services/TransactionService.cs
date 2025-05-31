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

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int categoryId)
    {
        return await _transactionRepository.GetTransactionsByCategoryIdAsync(categoryId);
    }

    public async Task<bool> AddTransaction(TransactionDto transactionDto)
    {
        Transaction transaction = await CreateSingleTransaction(transactionDto);
        return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> UpdateTransaction(TransactionDto transactionDto)
    {
        Transaction transaction = await CreateSingleTransaction(transactionDto);
        return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> RemoveTransactionById(int transactionId)
    {
        return await _transactionRepository.RemoveTransactionByIdAsync(transactionId);
    }

    public async Task<bool> AddRepeatingMonthlyTransaction(TransactionDto transactionDto)
    {
        MonthlySchedule monthlySchedule = new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        RecurringSchedule schedule = monthlySchedule.GenerateDates(transactionDto.SelectedDays);

        foreach (DateTime date in schedule)
        {
            Transaction transaction = await CreateRepeatingTransaction(transactionDto, date);
            bool result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result)
            {
                return false;
            }
        }
        return true;
    }

    public async Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDto transactionDto)
    {
        BiWeeklySchedule biWeeklySchedule = new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        RecurringSchedule schedule = biWeeklySchedule.GenerateDates(transactionDto.SelectedDays);

        foreach (DateTime date in schedule)
        {
            Transaction transaction = await CreateRepeatingTransaction(transactionDto, date);

            bool result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result)
            {
                return false;
            }
        }
        return true;
    }

    private async Task<Transaction> CreateSingleTransaction(TransactionDto transactionDto)
    {
        BudgetCategory existingBudgetCategory =
            await _budgetCategoryRepository.GetBudgetCategoryAsync(transactionDto.BudgetCategoryString);
        
        Transaction transaction = new()
        {
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            EndDate = transactionDto.EndDate,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = existingBudgetCategory
        };
        
        return transaction;
    }

    private async Task<Transaction> CreateRepeatingTransaction(TransactionDto transactionDto, DateTime date)
    {
        BudgetCategory existingBudgetCategory = await _budgetCategoryRepository
            .GetBudgetCategoryAsync(transactionDto.BudgetCategoryString);
        
        Transaction transaction = new()
        {
            Amount = transactionDto.Amount,
            Date = date,
            EndDate = transactionDto.EndDate,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = existingBudgetCategory
        };
        
        return transaction;
    }
}