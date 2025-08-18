using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Domain.Enums;

namespace LedgerlyApi.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<bool> AddTransactionAsync(Transaction transaction);
    Task<bool> RemoveTransactionByIdAsync(int transactionId);
    Task<Transaction> UpdateTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(CategoryType categoryType);
    Task<decimal> GetTransactionBalanceAsync(TransactionType transactionType);
    Task<IEnumerable<Transaction>> GetLastFiveTransactionsAsync();
    Task<IEnumerable<decimal>> GetMonthlyTransactionAmountsForYearAsync(int year, TransactionType transactionType);
    Task<IEnumerable<Transaction>> GetPagedTransactionsAsync(int page, int pageSize);
    Task<int> GetTotalTransactionCountAsync();
}