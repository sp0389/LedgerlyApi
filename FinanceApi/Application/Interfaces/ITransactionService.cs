using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;
using FinanceApi.Domain.Enums;

namespace FinanceApi.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransactionsById(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategory(CategoryType categoryType);
    Task<bool> AddTransaction(TransactionDto transaction);
    Task<bool> RemoveTransactionById(int transactionId);
    Task<Transaction> UpdateTransaction(TransactionDto transaction);
    Task<bool> AddRepeatingMonthlyTransaction(TransactionDto transaction);
    Task<bool> AddRepeatingBiWeeklyTransaction(TransactionDto transaction);
}