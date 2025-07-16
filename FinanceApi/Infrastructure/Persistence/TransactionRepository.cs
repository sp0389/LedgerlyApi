using FinanceApi.Domain.Entities;
using FinanceApi.Domain.Enums;
using FinanceApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infrastructure.Persistence;

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

    public async Task<decimal> GetIncomeTransactionBalanceAsync()
    {
        var incomeTransactions = await _context.Transactions
            .Where(t => t.TransactionType == TransactionType.Income)
            .ToListAsync();
        var balance = incomeTransactions.Sum(t => t.Amount);
        return balance;
    }

    public async Task<decimal> GetExpenseTransactionBalanceAsync()
    {
        var expenseTransactions = await _context.Transactions
            .Where(t => t.TransactionType == TransactionType.Expense)
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
}