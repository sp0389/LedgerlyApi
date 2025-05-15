using System;
using FinanceApi.DAL.Interface;
using FinanceApi.Models;

namespace FinanceApi.Services;

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

    public async Task<bool> AddTransaction(Transaction transaction)
    {
        Transaction t = new()
        {
            Amount = transaction.Amount,
            Date = transaction.Date,
            IsRecurring = transaction.IsRecurring,
            EndDate = transaction.EndDate,
            Occurrences = transaction.Occurrences,
            Description = transaction.Description,
            TransactionType = transaction.TransactionType,
            BudgetCategoryId = transaction.BudgetCategory.Id,
        };

        return await _transactionRepository.AddTransactionAsync(t);
    }

    public async Task<Transaction> UpdateTransaction(Transaction transaction)
    {
        return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> RemoveTransactionById(int transactionId)
    {
        return await _transactionRepository.RemoveTransactionByIdAsync(transactionId);
    }
}
