using System;
using FinanceApi.Models;
namespace FinanceApi.DAL.Interface;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(int categoryId);
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    Task<bool> RemoveTransactionById(int transactionId);
    Task<Transaction> UpdateTransactionAsync(int transactionId, Transaction transaction);
}