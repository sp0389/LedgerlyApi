using System;
using FinanceApi.Models;

namespace FinanceApi.DAL.Interface;

public interface IBudgetCategoryRepository
{
    Task<bool> AddBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<BudgetCategory>UpdateBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId);
}