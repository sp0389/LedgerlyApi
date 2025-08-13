using LedgerlyApi.Domain.Enums;

namespace LedgerlyApi.Application.DTO;

public class BudgetCategoryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
    public CategoryType CategoryType { get; set; }
}