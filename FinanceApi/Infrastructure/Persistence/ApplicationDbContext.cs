using FinanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinanceApi.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>
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
        _ = new Seed(modelBuilder);
    }
}