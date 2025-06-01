namespace FinanceApi.Domain.ValueObjects;

public abstract class Schedule
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Occurrences { get; set; }
}

public abstract class RepeatingSchedule : Schedule
{
    public RepeatingSchedule(DateTime startDate, DateTime endDate, int occurrences)
    {
        StartDate = startDate;
        EndDate = endDate;
        Occurrences = occurrences;
    }

    public abstract RecurringSchedule GenerateDates(IList<DayOfWeek> chosenDays);
}

public class BiWeeklySchedule : RepeatingSchedule
{
    public BiWeeklySchedule(DateTime startDate, DateTime endDate, int occurrences)
        : base(startDate, endDate, occurrences)
    {
    }

    public override RecurringSchedule GenerateDates(IList<DayOfWeek> chosenDays)
    {
        var currentDate = StartDate;
        List<DateTime> scheduledDates = new();

        if (Occurrences != 0)
        {
            for (var i = 0; i < Occurrences; i++)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek)) scheduledDates.Add(currentDate);
                currentDate = currentDate.AddDays(14);
            }

            return new RecurringSchedule(scheduledDates);
        }

        else if (currentDate <= EndDate)
        {
            while (currentDate <= EndDate)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek)) scheduledDates.Add(currentDate);
                currentDate = currentDate.AddDays(14);
            }
        }

        return new RecurringSchedule(scheduledDates);
    }
}

public class MonthlySchedule : RepeatingSchedule
{
    public MonthlySchedule(DateTime startDate, DateTime endDate, int occurrences)
        : base(startDate, endDate, occurrences)
    {
    }

    public override RecurringSchedule GenerateDates(IList<DayOfWeek> chosenDays)
    {
        var currentDate = StartDate;
        List<DateTime> scheduledDates = new();

        if (Occurrences != 0)
        {
            for (var i = 0; i < Occurrences; i++)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek)) scheduledDates.Add(currentDate);

                currentDate = currentDate.AddMonths(1);
            }

            return new RecurringSchedule(scheduledDates);
        }

        else if (StartDate <= EndDate)
        {
            currentDate = StartDate;

            while (currentDate <= EndDate)
            {
                if (chosenDays.Contains(currentDate.DayOfWeek)) scheduledDates.Add(currentDate);
                currentDate = currentDate.AddMonths(1);
            }
        }

        return new RecurringSchedule(scheduledDates);
    }
}