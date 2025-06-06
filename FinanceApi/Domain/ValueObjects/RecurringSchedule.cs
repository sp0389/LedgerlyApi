using System.Collections;

namespace FinanceApi.Domain.ValueObjects;

public sealed class RecurringSchedule : IEnumerable<DateTime>
{
    private readonly IEnumerable<DateTime> _dates;

    public RecurringSchedule(List<DateTime> dates)
    {
        _dates = dates;
    }

    public IEnumerator<DateTime> GetEnumerator()
    {
        return _dates.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}