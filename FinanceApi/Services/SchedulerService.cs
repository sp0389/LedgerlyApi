using System;

namespace FinanceApi.Services;

public abstract class SchedulerService
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Occurrences { get; set; }
    protected HashSet<DayOfWeek> SelectedDays = new();
    protected void UpdateSelectedDays(IList<DayOfWeek> chosenDays)
    {
        SelectedDays = chosenDays.ToHashSet();
    }
}
public abstract class Repeating : SchedulerService
{
    public Repeating(DateTime startDate, DateTime endDate, int occurrences)
    {
        StartDate = startDate;
        EndDate = endDate;
        Occurrences = occurrences;
    }
}
public class BiWeekly : Repeating
{
    public BiWeekly(DateTime startDate, DateTime endDate, int occurrences, IList<DayOfWeek> chosenDays)
        : base(startDate, endDate, occurrences)
    {
        UpdateSelectedDays(chosenDays);
    }

    public IList<DateTime> GenerateBiWeeklyDates()
    {
        DateTime currentDate = StartDate;
        List<DateTime> selectedDates = new();

        if (Occurrences != 0)
        {
            for (int i = 0; i < Occurrences; i++)
            {
                if (SelectedDays.Contains(currentDate.DayOfWeek))
                {
                    selectedDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(14);
            }
            return selectedDates;
        }

        else if (currentDate <= EndDate)
        {
            while (currentDate <= EndDate)
            {
                if (SelectedDays.Contains(currentDate.DayOfWeek))
                {
                    selectedDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(14);
            }
        }
        return selectedDates;
    }
}

public class Monthly : Repeating
{
    public Monthly(DateTime startDate, DateTime endDate, int occurrences, IList<DayOfWeek> chosenDays)
        : base(startDate, endDate, occurrences)
    {
        UpdateSelectedDays(chosenDays);
    }

    public IList<DateTime> GenerateMonthlyDates()
    {
        DateTime currentDate = StartDate;
        List<DateTime> selectedDates = new();

        if (Occurrences != 0)
        {
            for (int i = 0; i < Occurrences; i++)
            {
                if (SelectedDays.Contains(currentDate.DayOfWeek))
                {
                    selectedDates.Add(currentDate);
                }

                currentDate = currentDate.AddMonths(1);
            }
            return selectedDates;
        }

        else if (StartDate <= EndDate)
        {
            currentDate = StartDate;

            while (currentDate <= EndDate)
            {
                if (SelectedDays.Contains(currentDate.DayOfWeek))
                {
                    selectedDates.Add(currentDate);
                }
                currentDate = currentDate.AddMonths(1);
            }
        }
        return selectedDates;
    }
}