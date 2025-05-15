using System;
using FinanceApi.Models;
namespace FinanceApi.DAL.Interface;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransactionsById(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int categoryId);
    Task<bool> AddTransaction(Transaction transaction);
    Task<bool> RemoveTransactionById(int transactionId);
    Task<Transaction> UpdateTransaction(Transaction transaction);
}