using System;
using FinanceApi.Models;
using FinanceApi.Models.Enum;
using FinanceApi.DAL.Core;
using FinanceApi.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using FinanceApi.Models.DTO;

namespace FinanceApi.DAL.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        return await _context.Transactions.ToListAsync();
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id)
    {
        return await _context.Transactions
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync() ?? throw new ApplicationException("Transaction with that ID was not found");
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(int categoryId)
    {
        return await _context.Transactions
            .Where(t => t.BudgetCategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<bool> AddTransactionAsync(TransactionDTO transaction)
    {
        if (transaction.BudgetCategoryString != null && Enum.TryParse(transaction.BudgetCategoryString, out CategoryType categoryType))
        {
            BudgetCategory bc = await _context.BudgetCategories
                .FirstAsync(bc => bc.CategoryType == categoryType);
            transaction.BudgetCategory = bc;
        }

        Transaction t = new();
        _context.Entry(t).CurrentValues.SetValues(transaction);

        await _context.Transactions.AddAsync(t);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTransactionByIdAsync(int transactionId)
    {
        Transaction transaction = await _context.Transactions.FindAsync(transactionId)
            ?? throw new ApplicationException("Could not find transaction with provided transaction ID");
        _context.Transactions.Remove(transaction);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Transaction> UpdateTransactionAsync(TransactionDTO transaction)
    {
        Transaction existingTransaction = await _context.Transactions.FindAsync(transaction.Id)
            ?? throw new ApplicationException("Could not find transaction with provided transaction ID.");

        _context.Entry(existingTransaction).CurrentValues.SetValues(transaction);
        await _context.SaveChangesAsync();

        return existingTransaction;
    }
}