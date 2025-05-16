using System;
using FinanceApi.Models;
using FinanceApi.Models.DTO;
namespace FinanceApi.DAL.Interface;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(int categoryId);
    Task<bool> AddTransactionAsync(TransactionDTO transaction);
    Task<bool> RemoveTransactionByIdAsync(int transactionId);
    Task<Transaction> UpdateTransactionAsync(TransactionDTO transaction);
}