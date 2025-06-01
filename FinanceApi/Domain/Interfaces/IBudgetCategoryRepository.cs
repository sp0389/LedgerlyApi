using FinanceApi.Domain.Entities;

namespace FinanceApi.Domain.Interfaces;

public interface IBudgetCategoryRepository
{
    Task<bool> AddBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<BudgetCategory> UpdateBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId);
    Task<IEnumerable<BudgetCategory>> GetAllBudgetCategoriesAsync();
    Task<BudgetCategory> GetBudgetCategoryByIdAsync(int budgetCategoryId);
}