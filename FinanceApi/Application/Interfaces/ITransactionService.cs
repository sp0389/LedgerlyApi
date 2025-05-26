using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;

namespace FinanceApi.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransactionsById(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int categoryId);
    Task<bool> AddTransaction(TransactionDTO transaction);
    Task<bool> RemoveTransactionById(int transactionId);
    Task<Transaction> UpdateTransaction(TransactionDTO transaction);
    Task<bool> AddRepeatingMonthlyTransaction(TransactionDTO transaction);
    Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDTO transaction);
}