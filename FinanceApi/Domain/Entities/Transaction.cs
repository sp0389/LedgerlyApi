using FinanceApi.Domain.Enums;
using FinanceApi.Domain.Exceptions;

namespace FinanceApi.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime? EndDate { get; set; }
    public int Occurrences { get; set; }
    public string Description { get; set; } = default!;
    public TransactionType TransactionType { get; set; }
    public int? BudgetCategoryId { get; set; }
    public BudgetCategory? BudgetCategory { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }

    private void ValidateTransactionDate()
    {
        if (Date <= DateTime.UtcNow.Date)
            throw new DomainRuleException("The transaction date must not be in the past.");
        if (Date > EndDate) throw new DomainRuleException("The transaction date cannot be after the end date.");
    }

    private void ValidateTransactionAmount()
    {
        if (Amount <= 0) throw new DomainRuleException("The amount must not be less than or equal to zero.");
    }

    public void ValidateTransactionBudget(decimal totalTransactionBudgetAmount)
    {
        if (BudgetCategory != null && Amount + totalTransactionBudgetAmount > BudgetCategory.Amount)
            throw new DomainRuleException("The transaction exceeds the total budget amount for the budget category.");
    }

    public void ValidateRepeatingTransactionBudget(decimal totalTransactionAmount)
    {
        if (BudgetCategory != null && totalTransactionAmount > BudgetCategory.Amount)
            throw new DomainRuleException("The transactions exceed the total budget amount for the budget category.");
    }

    private void ValidateRepeatingTransactionDate()
    {
        if (Occurrences <= 0 && EndDate == null && IsRecurring)
            throw new DomainRuleException(
                "You must specify either occurrences or an end date for a repeating transaction.");
    }

    public void Validate()
    {
        ValidateTransactionDate();
        ValidateTransactionAmount();
        ValidateRepeatingTransactionDate();
    }
}