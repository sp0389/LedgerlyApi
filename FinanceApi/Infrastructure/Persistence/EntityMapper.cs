using FinanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infrastructure.Persistence;

public class EntityMapper
{
    public EntityMapper(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BudgetCategory>(budgetCategory =>
        {
            budgetCategory.HasMany(bc => bc.Transactions)
                .WithOne(t => t.BudgetCategory)
                .HasForeignKey(t => t.BudgetCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(transaction =>
        {
            transaction.Property(t => t.Description).HasMaxLength(500);
            transaction.HasOne(t => t.BudgetCategory)
                .WithMany(bc => bc.Transactions)
                .HasForeignKey(t => t.BudgetCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}