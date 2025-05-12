using System;
using FinanceApi.Models;

namespace FinanceApi.DAL.Interface;

public interface IBudgetCategoryRepository
{
    Task<BudgetCategory> AddBudgetCategoryAsync(BudgetCategory budgetCategory);
    Task<BudgetCategory>UpdateBudgetCategoryAsync(int budgetCategoryId, BudgetCategory budgetCategory);
    Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId);
}