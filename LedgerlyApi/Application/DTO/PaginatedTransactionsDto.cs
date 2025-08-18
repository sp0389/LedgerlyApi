using LedgerlyApi.Domain.Entities;
namespace LedgerlyApi.Application.DTO;

public class PagedTransactionsDto
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } =  new List<Transaction>();
}