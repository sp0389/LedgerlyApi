using FinanceApi.Domain.Entities;
using FinanceApi.Application.DTO;

namespace FinanceApi.Application.Interfaces;

public interface IBudgetCategoryService
{
    Task<bool> AddBudgetCategory(BudgetCategoryDTO budgetCategory);
    Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDTO budgetCategory);
    Task<bool> RemoveBudgetCategory(int budgetCategoryId);
}