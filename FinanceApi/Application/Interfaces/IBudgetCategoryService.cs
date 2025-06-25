using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;

namespace FinanceApi.Application.Interfaces;

public interface IBudgetCategoryService
{
    Task<IEnumerable<BudgetCategory>> GetAllBudgetCategories();
    Task<BudgetCategory> GetBudgetCategoryById(int budgetCategoryId);
    Task<bool> AddBudgetCategory(BudgetCategoryDto budgetCategory);
    Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDto budgetCategory);
    Task<bool> RemoveBudgetCategory(int budgetCategoryId);
    Task<decimal> GetAvailableBudgetCategoryBalance(BudgetCategory budgetCategory);
}