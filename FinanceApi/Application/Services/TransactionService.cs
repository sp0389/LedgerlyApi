using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;
using FinanceApi.Domain.Interfaces;
using FinanceApi.Domain.ValueObjects;

namespace FinanceApi.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
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

    public async Task<bool> AddTransaction(TransactionDTO transactionDto)
    {
        Transaction transaction = CreateSingleTransaction(transactionDto);
        return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> UpdateTransaction(TransactionDTO transactionDto)
    {
        Transaction transaction = CreateSingleTransaction(transactionDto);
        return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> RemoveTransactionById(int transactionId)
    {
        return await _transactionRepository.RemoveTransactionByIdAsync(transactionId);
    }

    public async Task<bool> AddRepeatingMonthlyTransaction(TransactionDTO transactionDto)
    {
        Monthly monthlyTransaction = new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        IEnumerable<DateTime> dates = monthlyTransaction.GenerateDates(transactionDto.SelectedDays);

        foreach (DateTime date in dates)
        {
            Transaction transaction = CreateRepeatingTransaction(transactionDto, date);
            bool result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result)
            {
                return false;
            }
        }
        return true;
    }

    public async Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDTO transactionDto)
    {
        //TODO: End Date check
        BiWeekly biWeeklyTransaction = new(transactionDto.Date, transactionDto.EndDate!.Value, transactionDto.Occurrences);
        IEnumerable<DateTime> dates = biWeeklyTransaction.GenerateDates(transactionDto.SelectedDays);

        foreach (DateTime date in dates)
        {
            Transaction transaction = CreateRepeatingTransaction(transactionDto, date);

            bool result = await _transactionRepository.AddTransactionAsync(transaction);

            if (!result)
            {
                return false;
            }
        }
        return true;
    }

    private static Transaction CreateSingleTransaction(TransactionDTO transactionDto)
    {
        Transaction transaction = new()
        {
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            EndDate = transactionDto.EndDate,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = transactionDto.BudgetCategory,
        };
        return transaction;
    }

    private static Transaction CreateRepeatingTransaction(TransactionDTO transactionDto, DateTime date)
    {
        Transaction transaction = new()
        {
            Amount = transactionDto.Amount,
            Date = date,
            EndDate = transactionDto.EndDate,
            Description = transactionDto.Description,
            TransactionType = transactionDto.TransactionType,
            BudgetCategory = transactionDto.BudgetCategory,
        };

        return transaction;
    }
}