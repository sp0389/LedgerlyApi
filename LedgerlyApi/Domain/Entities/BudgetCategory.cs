using System.Text.Json.Serialization;
using LedgerlyApi.Domain.Enums;
using LedgerlyApi.Domain.Exceptions;

namespace LedgerlyApi.Domain.Entities;

public class BudgetCategory
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
    public byte[] Version { get; set; } = default!;
    public CategoryType CategoryType { get; set; }
    [JsonIgnore]
    public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
    public User User { get; set; } = default!;
    public string UserId { get; set; } = default!;
    private void ValidateDate()
    {
        if (StartDate > EndDate) throw new DomainRuleException("Start date cannot be after the end date.");
    }

    private void ValidateAmount()
    {
        if (Amount <= 0) throw new DomainRuleException("The amount must be not be less than or equal to zero.");
    }
    
    public void Validate()
    {
        ValidateDate();
        ValidateAmount();
    }
}