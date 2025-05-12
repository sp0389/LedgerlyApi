using System;
using FinanceApi.DAL.Core;
using FinanceApi.DAL.Interface;
using FinanceApi.Models;

namespace FinanceApi.DAL.Repository;

public class BudgetCategoryRepository : IBudgetCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public BudgetCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BudgetCategory> AddBudgetCategoryAsync(BudgetCategory budgetCategory)
    {
        await _context.BudgetCategories.AddAsync(budgetCategory);
        await _context.SaveChangesAsync();

        return budgetCategory;
    }

    public async Task<BudgetCategory>UpdateBudgetCategoryAsync(int budgetCategoryId, BudgetCategory budgetCategory)
    {
        BudgetCategory existingBudgetCategory = await _context.BudgetCategories
            .FindAsync(budgetCategoryId) ?? throw new ApplicationException("No budget category was found with that ID.");

        _context.Entry(existingBudgetCategory).CurrentValues.SetValues(budgetCategory);
        await _context.SaveChangesAsync();

        return existingBudgetCategory;
    }

    public async Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId)
    {
        _context.Remove(budgetCategoryId);
        return await _context.SaveChangesAsync() > 0;
    }
}
