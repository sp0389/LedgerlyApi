using FinanceApi.DAL.Interface;
using FinanceApi.Models;

namespace FinanceApi.Services;

public class BudgetCategoryService : IBudgetCategoryService
{
    private readonly IBudgetCategoryRepository _budgetCategoryRepository;

    public BudgetCategoryService(IBudgetCategoryRepository budgetCategoryRepository)
    {
        _budgetCategoryRepository = budgetCategoryRepository;
    }

    public async Task<bool> AddBudgetCategory(BudgetCategoryDTO budgetCategory)
    {
        return await _budgetCategoryRepository.AddBudgetCategoryAsync(budgetCategory);
    }

    public async Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDTO budgetCategory)
    {
        return await _budgetCategoryRepository.UpdateBudgetCategoryAsync(budgetCategory);
    }

    public async Task<bool> RemoveBudgetCategory(int budgetCategoryId)
    {
        return await _budgetCategoryRepository.RemoveBudgetCategoryAsync(budgetCategoryId);
    }
}