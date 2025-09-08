using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Application.DTO;

namespace LedgerlyApi.Application.Interfaces;

public interface IBudgetCategoryService
{
    Task<IEnumerable<BudgetCategory>> GetAllBudgetCategories();
    Task<BudgetCategory> GetBudgetCategoryById(int budgetCategoryId);
    Task<bool> AddBudgetCategory(BudgetCategoryDto budgetCategory, string userId);
    Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDto budgetCategory);
    Task<bool> RemoveBudgetCategory(int budgetCategoryId);
    Task<decimal> GetAvailableBudgetCategoryBalance(BudgetCategory budgetCategory);
    IEnumerable<string> GetAvailableCategoryTypes();
}