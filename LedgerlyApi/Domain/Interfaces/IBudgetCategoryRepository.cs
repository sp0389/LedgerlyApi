using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Domain.Enums;

namespace LedgerlyApi.Domain.Interfaces;

public interface IBudgetCategoryRepository
{
    Task<bool> AddBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<BudgetCategory> UpdateBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId);
    Task<IEnumerable<BudgetCategory>> GetAllBudgetCategoriesAsync();
    Task<BudgetCategory> GetBudgetCategoryByIdAsync(int budgetCategoryId);
    Task<BudgetCategory> GetBudgetCategoryByCategoryTypeAsync(CategoryType categoryType);
    Task<decimal> GetAvailableBudgetCategoryBalance(BudgetCategory budgetCategory);
}