using Microsoft.AspNetCore.Identity;

namespace LedgerlyApi.Domain.Entities;

public class User : IdentityUser
{
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<BudgetCategory> BudgetCategories { get; set; } = new List<BudgetCategory>();
}