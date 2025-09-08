using LedgerlyApi.Domain.Interfaces;
using LedgerlyApi.Application.DTO;
using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Application.Interfaces;
using LedgerlyApi.Domain.Enums;

namespace LedgerlyApi.Application.Services;

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

    public async Task<bool> AddBudgetCategory(BudgetCategoryDto budgetCategoryDto, string userId)
    {
        var budgetCategory = CreateBudgetCategory(budgetCategoryDto, userId);
        return await _budgetCategoryRepository.AddBudgetCategoryAsync(budgetCategory);
    }

    public async Task<BudgetCategory> UpdateBudgetCategory(BudgetCategoryDto budgetCategoryDto)
    {
        var budgetCategory = new BudgetCategory
        {
            Title = budgetCategoryDto.Title,
            Amount = budgetCategoryDto.Amount,
            StartDate = budgetCategoryDto.StartDate,
            EndDate = budgetCategoryDto.EndDate,
            Description = budgetCategoryDto.Description,
            CategoryType = budgetCategoryDto.CategoryType,
            UserId = budgetCategoryDto.UserId,
        };

        return await _budgetCategoryRepository.UpdateBudgetCategoryAsync(budgetCategory);
    }

    public async Task<bool> RemoveBudgetCategory(int budgetCategoryId)
    {
        return await _budgetCategoryRepository.RemoveBudgetCategoryAsync(budgetCategoryId);
    }

    private BudgetCategory CreateBudgetCategory(BudgetCategoryDto budgetCategoryDto, string userId)
    {
        var budgetCategory = new BudgetCategory()
        {
            Title = budgetCategoryDto.Title,
            Amount = budgetCategoryDto.Amount,
            StartDate = budgetCategoryDto.StartDate,
            EndDate = budgetCategoryDto.EndDate,
            Description = budgetCategoryDto.Description,
            CategoryType = budgetCategoryDto.CategoryType,
            UserId = userId
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