using FinanceApi.Domain.Enums;
using FinanceApi.Domain.Entities;

namespace FinanceApi.Application.DTO;

public class TransactionDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime? EndDate { get; set; }
    public int Occurrences { get; set; }
    public string Description { get; set; } = default!;
    public IList<DayOfWeek> SelectedDays { get; set; } = new List<DayOfWeek>();
    public TransactionType TransactionType { get; set; }
    public BudgetCategory BudgetCategory { get; set; } = default!;
}