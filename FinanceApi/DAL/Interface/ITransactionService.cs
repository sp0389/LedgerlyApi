using System;
using FinanceApi.Models;
using FinanceApi.Models.DTO;
namespace FinanceApi.DAL.Interface;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransactionsById(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int categoryId);
    Task<bool> AddTransaction(TransactionDTO transaction);
    Task<bool> RemoveTransactionById(int transactionId);
    Task<Transaction> UpdateTransaction(TransactionDTO transaction);
}