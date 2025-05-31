using FinanceApi.Domain.Enums;

namespace FinanceApi.Application.DTO;

public class TransactionDto
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
    public string BudgetCategoryString {get; set;} = default!;
}