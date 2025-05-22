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

    public async Task<bool> AddBudgetCategoryAsync(BudgetCategoryDTO budgetCategory)
    {
        BudgetCategory bc = new();
        _context.Entry(bc).CurrentValues.SetValues(budgetCategory);

        await _context.BudgetCategories.AddAsync(bc);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<BudgetCategory>UpdateBudgetCategoryAsync(BudgetCategoryDTO budgetCategory)
    {
        BudgetCategory existingBudgetCategory = await _context.BudgetCategories
            .FindAsync(budgetCategory.Id) ?? throw new ApplicationException("No budget category was found with that ID.");

        _context.Entry(existingBudgetCategory).CurrentValues.SetValues(budgetCategory);
        await _context.SaveChangesAsync();

        return existingBudgetCategory;
    }

    public async Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId)
    {
        BudgetCategory budgetCategory = await _context.BudgetCategories.FindAsync(budgetCategoryId)
            ?? throw new ApplicationException("No budget category was found with the specified ID.");
        _context.BudgetCategories.Remove(budgetCategory);
        return await _context.SaveChangesAsync() > 0;
    }
}