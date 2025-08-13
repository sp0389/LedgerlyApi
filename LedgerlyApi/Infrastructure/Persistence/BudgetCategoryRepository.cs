using LedgerlyApi.Domain.Interfaces;
using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LedgerlyApi.Infrastructure.Persistence;

public class BudgetCategoryRepository : IBudgetCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public BudgetCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddBudgetCategoryAsync(BudgetCategory budgetCategory)
    {
        await _context.BudgetCategories.AddAsync(budgetCategory);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<BudgetCategory> UpdateBudgetCategoryAsync(BudgetCategory budgetCategory)
    {
        var existingBudgetCategory = await _context.BudgetCategories
                                         .FindAsync(budgetCategory.Id) ??
                                     throw new ApplicationException("No budget category was found with that ID.");

        _context.Entry(existingBudgetCategory).CurrentValues.SetValues(budgetCategory);
        await _context.SaveChangesAsync();

        return existingBudgetCategory;
    }

    public async Task<bool> RemoveBudgetCategoryAsync(int budgetCategoryId)
    {
        var budgetCategory = await _context.BudgetCategories.FindAsync(budgetCategoryId)
                             ?? throw new ApplicationException("No budget category was found with the specified ID.");
        _context.BudgetCategories.Remove(budgetCategory);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<BudgetCategory>> GetAllBudgetCategoriesAsync()
    {
        return await _context.BudgetCategories.ToListAsync();
    }

    public async Task<BudgetCategory> GetBudgetCategoryByIdAsync(int budgetCategoryId)
    {
        var budgetCategory = await _context.BudgetCategories.FindAsync(budgetCategoryId)
                             ?? throw new ApplicationException("No budget category was found with the specified ID.");
        return budgetCategory;
    }

    public async Task<BudgetCategory> GetBudgetCategoryByCategoryTypeAsync(CategoryType categoryType)
    {
        return await _context.BudgetCategories
                   .FirstOrDefaultAsync(bc => bc.CategoryType == categoryType) ??
               throw new ApplicationException("No budget category was found with the specified category type.");
    }

    public async Task<decimal> GetAvailableBudgetCategoryBalance(BudgetCategory budgetCategory)
    {
        var budgetCategoryTransactions = await _context.BudgetCategories
            .Include(bc => bc.Transactions)
            .SelectMany(bc => bc.Transactions)
            .ToListAsync();
        
        var transactionTotal = budgetCategoryTransactions.Sum(t => t.Amount);
        return budgetCategory.Amount - transactionTotal;
    }
}