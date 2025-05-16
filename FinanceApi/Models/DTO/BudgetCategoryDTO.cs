using System;
using FinanceApi.Models.Enum;

namespace FinanceApi.Models;

public class BudgetCategoryDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
    public CategoryType CategoryType { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}