using System;
using FinanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.DAL.Core;

public class ApplicationDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BudgetCategory> BudgetCategories { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = new EntityMapper(modelBuilder);
    }
}
