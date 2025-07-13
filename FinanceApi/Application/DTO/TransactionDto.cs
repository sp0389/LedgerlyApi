using FinanceApi.Domain.Enums;

namespace FinanceApi.Application.DTO;

public class TransactionDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime? EndDate { get; set; }
    public int Occurrences { get; set; } = 0;
    public string Description { get; set; } = default!;
    public IList<DayOfWeek>? SelectedDays { get; set; } = new List<DayOfWeek>();
    public TransactionType TransactionType { get; set; }
    public CategoryType? CategoryType { get; set; }
    public int? BudgetCategoryId { get; set; }
}