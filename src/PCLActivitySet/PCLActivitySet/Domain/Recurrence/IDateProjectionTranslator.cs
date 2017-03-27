namespace PCLActivitySet.Domain.Recurrence
{
    public interface IDateProjectionTranslator
    {
        EDateProjectionType ProjectionType { get; }

        // DailyRecurrence           => DayCount
        // WeeklyRecurrence          => WeekCount
        // MonthlyRecurrence         => MonthCount
        // MonthlyRelativeRecurrence => MonthCount
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => 
        int PeriodCount { get; set; }

        // DailyRecurrence           => 
        // WeeklyRecurrence          => DaysOfWeek
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => 
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => 
        EDaysOfWeekFlags DaysOfWeekFlags { get; set; }

        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => DayOfMonth
        // MonthlyRelativeRecurrence => 
        // YearlyRecurrence          => DayOfMonth
        // YearlyRelativeRecurrence  => 
        int DayOfMonth { get; set; }

        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => DaysOfWeekExt
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => DaysOfWeekExt
        EDaysOfWeekExt DaysOfWeekExt { get; set; }

        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => WeeksInMonth
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => WeeksInMonth
        EWeeksInMonth WeeksInMonth { get; set; }

        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => 
        // YearlyRecurrence          => Month
        // YearlyRelativeRecurrence  => Month
        EMonth Month { get; set; }

    }

}
