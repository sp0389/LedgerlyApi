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
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(transaction =>
        {
            transaction.Property(t => t.Title).HasMaxLength(50);
            transaction.Property(t => t.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.UserName).HasMaxLength(50);
            user.Property(u => u.Email).HasMaxLength(50);
            
            user.HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            user.HasMany(u => u.BudgetCategories)
                .WithOne(bc => bc.User)
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}