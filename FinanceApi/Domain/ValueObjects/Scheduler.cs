namespace FinanceApi.Domain.ValueObjects;

public abstract class Scheduler
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Occurrences { get; set; }
}
public abstract class Repeating : Scheduler
{
    public Repeating(DateTime startDate, DateTime endDate, int occurrences)
    {
        StartDate = startDate;
        EndDate = endDate;
        Occurrences = occurrences;
    }
    public abstract IList<DateTime> GenerateDates(IList<DayOfWeek> chosenDays);
}
public class BiWeekly : Repeating
{
    public BiWeekly(DateTime startDate, DateTime endDate, int occurrences)
        : base(startDate, endDate, occurrences) { }
    public override IList<DateTime> GenerateDates(IList<DayOfWeek> chosenDays)
    {
        DateTime currentDate = StartDate;
        List<DateTime> scheduledDates = new();

        if (Occurrences != 0)
        {
            for (int i = 0; i < Occurrences; i++)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek))
                {
                    scheduledDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(14);
            }
            return scheduledDates;
        }

        else if (currentDate <= EndDate)
        {
            while (currentDate <= EndDate)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek))
                {
                    scheduledDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(14);
            }
        }
        return scheduledDates;
    }
}

public class Monthly : Repeating
{
    public Monthly(DateTime startDate, DateTime endDate, int occurrences)
        : base(startDate, endDate, occurrences) { }

    public override IList<DateTime> GenerateDates(IList<DayOfWeek> chosenDays)
    {
        DateTime currentDate = StartDate;
        List<DateTime> scheduledDates = new();

        if (Occurrences != 0)
        {
            for (int i = 0; i < Occurrences; i++)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek))
                {
                    scheduledDates.Add(currentDate);
                }

                currentDate = currentDate.AddMonths(1);
            }
            return scheduledDates;
        }

        else if (StartDate <= EndDate)
        {
            currentDate = StartDate;

            while (currentDate <= EndDate)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek))
                {
                    scheduledDates.Add(currentDate);
                }
                currentDate = currentDate.AddMonths(1);
            }
        }
        return scheduledDates;
    }
}