using System;
using FinanceApi.Domain.Enums;

namespace FinanceApi.Domain.Entities;

public class BudgetCategory
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
    public CategoryType CategoryType { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}