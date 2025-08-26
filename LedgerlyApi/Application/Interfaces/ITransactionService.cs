using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Application.DTO;
using LedgerlyApi.Domain.Enums;

namespace LedgerlyApi.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<IEnumerable<Transaction>> GetLastFiveTransactions();
    Task<Transaction> GetTransactionsById(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategory(CategoryType categoryType);
    Task<bool> AddTransaction(TransactionDto transaction);
    Task<bool> RemoveTransactionById(int transactionId);
    Task<Transaction> UpdateTransaction(TransactionDto transaction);
    Task<bool> AddRepeatingMonthlyTransaction(TransactionDto transaction);
    Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDto transaction);
    Task<decimal> GetTransactionBalance(TransactionType transactionType);
    Task<IEnumerable<decimal>> GetMonthlyTransactionAmountsForYear(int year, TransactionType transactionType);
    Task<IEnumerable<Transaction>> GetPagedTransactions(int page, int pageSize);
    Task<int> GetTotalTransactionCount();
    Task<decimal> GetTotalTransactionBalance();
}