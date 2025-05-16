using FinanceApi.Models;

namespace FinanceApi.DAL.Interface;

public interface IBudgetCategoryService
{
    Task<bool> AddBudgetCategory(BudgetCategoryDTO budgetCategory);
    Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDTO budgetCategory);
    Task<bool> RemoveBudgetCategory(int budgetCategoryId);
}