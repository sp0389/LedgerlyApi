namespace LedgerlyApi.Application.DTO;

public class BudgetCategorySummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal AvailableAmount { get; set; }
    public decimal SpentAmount { get; set; }
}