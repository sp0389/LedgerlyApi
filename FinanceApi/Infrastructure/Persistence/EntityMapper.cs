using FinanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infrastructure.Persistence;

public class EntityMapper
{
    public EntityMapper(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BudgetCategory>(budgetCategory =>
        {
            budgetCategory.Property(bc => bc.Title).HasMaxLength(50);
            budgetCategory.Property(bc => bc.Description).HasMaxLength(500);
            budgetCategory.HasMany(bc => bc.Transactions)
                .WithOne(t => t.BudgetCategory)
                .HasForeignKey(t => t.BudgetCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(transaction =>
        {
            transaction.Property(t => t.Title).HasMaxLength(50);
            transaction.Property(t => t.Description).HasMaxLength(500);
            transaction.HasOne(t => t.BudgetCategory)
                .WithMany(bc => bc.Transactions)
                .HasForeignKey(t => t.BudgetCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}