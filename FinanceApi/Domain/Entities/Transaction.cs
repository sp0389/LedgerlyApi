using FinanceApi.Domain.Enums;
using FinanceApi.Domain.Exceptions;

namespace FinanceApi.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime? EndDate { get; set; }
    public int Occurrences { get; set; }
    public string Description { get; set; } = default!;
    public TransactionType TransactionType { get; set; }
    public int BudgetCategoryId { get; set; }
    public BudgetCategory? BudgetCategory { get; set; }

    public Transaction()
    {
        ValidateTransactionDate();
        ValidateTransactionAmount();
        if (IsRecurring) ValidateRepeatingTransaction();
    }

    private void ValidateTransactionDate()
    {
        if (Date <= DateTime.Now)
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

    private void ValidateRepeatingTransaction()
    {
        if(Occurrences <= 0 && EndDate == null)
            throw new DomainRuleException("You must specify either occurrences or an end date for a repeating transaction.");
    }
}