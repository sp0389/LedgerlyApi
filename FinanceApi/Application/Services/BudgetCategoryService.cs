using FinanceApi.Domain.Interfaces;
using FinanceApi.Application.DTO;
using FinanceApi.Domain.Entities;
using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Enums;

namespace FinanceApi.Application.Services;

public class BudgetCategoryService : IBudgetCategoryService
{
    private readonly IBudgetCategoryRepository _budgetCategoryRepository;

    public BudgetCategoryService(IBudgetCategoryRepository budgetCategoryRepository)
    {
        _budgetCategoryRepository = budgetCategoryRepository;
    }

    public async Task<IEnumerable<BudgetCategory>> GetAllBudgetCategories()
    {
        return await _budgetCategoryRepository.GetAllBudgetCategoriesAsync();
    }

    public async Task<BudgetCategory> GetBudgetCategoryById(int budgetCategoryId)
    {
        return await _budgetCategoryRepository.GetBudgetCategoryByIdAsync(budgetCategoryId);
    }

    public async Task<bool> AddBudgetCategory(BudgetCategoryDto budgetCategoryDto)
    {
        var budgetCategory = CreateBudgetCategory(budgetCategoryDto);
        return await _budgetCategoryRepository.AddBudgetCategoryAsync(budgetCategory);
    }

    public async Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDto budgetCategoryDto)
    {
        var budgetCategory = CreateBudgetCategory(budgetCategoryDto);
        return await _budgetCategoryRepository.UpdateBudgetCategoryAsync(budgetCategory);
    }

    public async Task<bool> RemoveBudgetCategory(int budgetCategoryId)
    {
        return await _budgetCategoryRepository.RemoveBudgetCategoryAsync(budgetCategoryId);
    }

    private static BudgetCategory CreateBudgetCategory(BudgetCategoryDto budgetCategoryDto)
    {
        BudgetCategory budgetCategory = new()
        {
            Title = budgetCategoryDto.Title,
            Amount = budgetCategoryDto.Amount,
            StartDate = budgetCategoryDto.StartDate,
            EndDate = budgetCategoryDto.EndDate,
            Description = budgetCategoryDto.Description,
            CategoryType = budgetCategoryDto.CategoryType
        };
        
        budgetCategory.Validate();
        return budgetCategory;
    }

    public async Task<decimal> GetAvailableBudgetCategoryBalance(BudgetCategory budgetCategory)
    {
        return await _budgetCategoryRepository.GetAvailableBudgetCategoryBalance(budgetCategory);
    }

    public IEnumerable<string> GetAvailableCategoryTypes()
    {
        return Enum.GetNames<CategoryType>().ToList();
    }
}