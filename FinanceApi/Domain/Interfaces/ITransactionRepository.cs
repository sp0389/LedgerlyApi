using FinanceApi.Domain.Entities;
using FinanceApi.Domain.Enums;

namespace FinanceApi.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task<IEnumerable<Transaction>> GetTransactionsByBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<bool> AddTransactionAsync(Transaction transaction);
    Task<bool> RemoveTransactionByIdAsync(int transactionId);
    Task<Transaction> UpdateTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(CategoryType categoryType);
    Task<decimal> GetIncomeTransactionBalanceAsync();
    Task<decimal> GetExpenseTransactionBalanceAsync();
    Task<IEnumerable<Transaction>> GetLastFiveTransactionsAsync();
}