using FinanceApi.Domain.Interfaces;
using FinanceApi.Application.DTO;
using FinanceApi.Domain.Entities;
using FinanceApi.Application.Interfaces;

namespace FinanceApi.Application.Services;

public class BudgetCategoryService : IBudgetCategoryService
{
    private readonly IBudgetCategoryRepository _budgetCategoryRepository;

    public BudgetCategoryService(IBudgetCategoryRepository budgetCategoryRepository)
    {
        _budgetCategoryRepository = budgetCategoryRepository;
    }

    public async Task<bool> AddBudgetCategory(BudgetCategoryDTO budgetCategoryDto)
    {
        BudgetCategory budgetCategory = CreateBudgetCategory(budgetCategoryDto);
        return await _budgetCategoryRepository.AddBudgetCategoryAsync(budgetCategory);
    }

    public async Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDTO budgetCategoryDto)
    {
        BudgetCategory budgetCategory = CreateBudgetCategory(budgetCategoryDto);
        return await _budgetCategoryRepository.UpdateBudgetCategoryAsync(budgetCategory);
    }

    public async Task<bool> RemoveBudgetCategory(int budgetCategoryId)
    {
        return await _budgetCategoryRepository.RemoveBudgetCategoryAsync(budgetCategoryId);
    }

    private static BudgetCategory CreateBudgetCategory(BudgetCategoryDTO budgetCategoryDto)
    {
        BudgetCategory budgetCategory = new()
        {
            Amount = budgetCategoryDto.Amount,
            StartDate = budgetCategoryDto.StartDate,
            EndDate = budgetCategoryDto.EndDate,
            Description = budgetCategoryDto.Description,
            CategoryType = budgetCategoryDto.CategoryType,
        };

        return budgetCategory;
    }
}