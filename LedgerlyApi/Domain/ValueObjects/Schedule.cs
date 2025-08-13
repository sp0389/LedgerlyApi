namespace LedgerlyApi.Domain.ValueObjects;

public abstract class Schedule
{
    protected DateTime StartDate { get; set; }
    protected DateTime EndDate { get; set; }
    protected int Occurrences { get; set; }
}

public abstract class RepeatingSchedule : Schedule
{
    public RepeatingSchedule(DateTime startDate, DateTime endDate, int occurrences)
    {
        StartDate = startDate;
        EndDate = endDate;
        Occurrences = occurrences;
    }

    public RecurringSchedule GenerateDates(IList<DayOfWeek> chosenDays, int frequency)
    {
        var currentDate = StartDate;
        var count = 0;
        List<DateTime> scheduledDates = new();

        if (Occurrences != 0)
        {
            while (count < Occurrences)
            {
                for (var i = 0; i < 7; i++)
                {
                    var currentDateCheck = currentDate.AddDays(i);

                    if (chosenDays.Contains(currentDateCheck.DayOfWeek))
                    {
                        scheduledDates.Add(currentDateCheck);
                        count++;

                        if (count == Occurrences)
                            break;
                    }
                }

                if (frequency == 14)
                {
                    currentDate = currentDate.AddDays(frequency);
                }

                else
                {
                    currentDate = currentDate.AddMonths(frequency);
                }
            }

            return new RecurringSchedule(scheduledDates);
        }

        while (currentDate <= EndDate)
        {
            for (var i = 0; i < 7; i++)
            {
                var currentDateCheck = currentDate.AddDays(i);

                if (chosenDays.Contains(currentDateCheck.DayOfWeek)) scheduledDates.Add(currentDateCheck);
            }

            if (frequency == 14)
            {
                currentDate = currentDate.AddDays(frequency);
            }

            else
            {
                currentDate = currentDate.AddMonths(frequency);
            }
        }

        return new RecurringSchedule(scheduledDates);
    }
}

public class BiWeeklySchedule : RepeatingSchedule
{
    public BiWeeklySchedule(DateTime startDate, DateTime endDate, int occurrences)
        : base(startDate, endDate, occurrences)
    {
    }
}

public class MonthlySchedule : RepeatingSchedule
{
    public MonthlySchedule(DateTime startDate, DateTime endDate, int occurrences)
        : base(startDate, endDate, occurrences)
    {
    }
}