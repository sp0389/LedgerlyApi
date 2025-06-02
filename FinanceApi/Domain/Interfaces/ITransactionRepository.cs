using FinanceApi.Domain.Entities;

namespace FinanceApi.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(int categoryId);
    Task<bool> AddTransactionAsync(Transaction transaction);
    Task<bool> RemoveTransactionByIdAsync(int transactionId);
    Task<Transaction> UpdateTransactionAsync(Transaction transaction);
}