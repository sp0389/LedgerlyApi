using LedgerlyApi.Domain.Entities;
using LedgerlyApi.Domain.Enums;
using LedgerlyApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LedgerlyApi.Infrastructure.Persistence;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        var transactions = await _context.Transactions
            .Include(t => t.BudgetCategory)
            .ToListAsync();
        return transactions;
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id)
    {
        return await _context.Transactions
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync() ?? throw new ApplicationException("Transaction with that ID was not found");
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByBudgetCategoryAsync(BudgetCategory budgetCategory)
    {
        return await _context.Transactions
            .Where(t => t.BudgetCategory == budgetCategory)
            .ToListAsync();
    }

    public async Task<bool> AddTransactionAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTransactionByIdAsync(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId)
                          ?? throw new ApplicationException("Could not find transaction with provided transaction ID");
        _context.Transactions.Remove(transaction);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
    {
        var existingTransaction = await _context.Transactions.FindAsync(transaction.Id)
                                  ?? throw new ApplicationException(
                                      "Could not find transaction with provided transaction ID.");

        _context.Entry(existingTransaction).CurrentValues.SetValues(transaction);
        await _context.SaveChangesAsync();

        return existingTransaction;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(CategoryType categoryType)
    {
        var transactions = await _context.Transactions
            .Where(t => t.BudgetCategory != null && t.BudgetCategory.CategoryType == categoryType)
            .ToListAsync();
        return transactions;
    }
    
    public async Task<decimal> GetTransactionBalanceAsync(TransactionType transactionType)
    {
        var expenseTransactions = await _context.Transactions
            .Where(t => t.TransactionType == transactionType)
            .ToListAsync();
        var balance = expenseTransactions.Sum(t => t.Amount);
        return balance;
    }

    public async Task<IEnumerable<Transaction>> GetLastFiveTransactionsAsync()
    {
        return await _context.Transactions.OrderByDescending(t => t.Date)
            .Take(5)
            .ToListAsync();
    }

    public async Task<IEnumerable<decimal>> GetMonthlyTransactionAmountsForYearAsync(int year, 
        TransactionType transactionType)
    {
        var monthlyTransactionAmounts = await _context.Transactions
            .Where(t => t.TransactionType == transactionType && t.Date.Year == year)
            .GroupBy(t => t.Date.Month)
            .Select(g => new
            {
                Month = g.Key,
                Amount = g.Sum(t => t.Amount)
            }).ToListAsync();
        
        var monthlyTransactionAmountsDict = monthlyTransactionAmounts
            .ToDictionary(m => m.Month, m => m.Amount);

        return Enumerable.Range(1, 12)
            .Select(m => monthlyTransactionAmountsDict.GetValueOrDefault(m, 0))
            .ToList();
    }
}