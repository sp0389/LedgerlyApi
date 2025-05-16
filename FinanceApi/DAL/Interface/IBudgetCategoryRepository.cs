using System;
using FinanceApi.Models;

namespace FinanceApi.DAL.Interface;

public interface IBudgetCategoryRepository
{
    Task<bool> AddBudgetCategoryAsync(BudgetCategoryDTO budgetCategory);
    Task<BudgetCategory>UpdateBudgetCategoryAsync(BudgetCategoryDTO budgetCategory);
    Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId);
}