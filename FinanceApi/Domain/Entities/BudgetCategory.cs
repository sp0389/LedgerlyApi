using FinanceApi.Domain.Enums;
using FinanceApi.Domain.Exceptions;

namespace FinanceApi.Domain.Entities;

public class BudgetCategory
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
    public CategoryType CategoryType { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();

    public BudgetCategory()
    {
        ValidateDate();
        ValidateAmount();
    }

    private void ValidateDate()
    {
        if (StartDate > EndDate) throw new DomainRuleException("Start date cannot be after the end date.");
    }

    private void ValidateAmount()
    {
        if (Amount <= 0) throw new DomainRuleException("The amount must be not be less than or equal to zero.");
    }
}