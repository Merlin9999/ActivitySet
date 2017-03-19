using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Fluent
{
    public class DateProjectionCreateHelper
    {
        public IDateProjection Daily(int periodCount)
        {
            return new DailyProjection() { DayCount = periodCount };
        }

        public IDateProjection Weekly(int periodCount, EDaysOfWeekFlags daysOfWeek)
        {
            return new WeeklyProjection() { DaysOfWeek = daysOfWeek, WeekCount = periodCount };
        }

        public IDateProjection Monthly(int periodCount, int dayOfMonth)
        {
            return new MonthlyProjection() { MonthCount = periodCount, DayOfMonth = dayOfMonth };
        }

        public IDateProjection Monthly(int periodCount, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            return new MonthlyRelativeProjection() { MonthCount = periodCount, WeeksInMonth = weeksInMonth, DaysOfWeekExt = daysOfWeek };
        }

        public IDateProjection Yearly(EMonth month, int dayOfMonth)
        {
            return new YearlyProjection() { Month = month, DayOfMonth = dayOfMonth };
        }

        public IDateProjection Yearly(EMonth month, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            return new YearlyRelativeProjection() { Month = month, WeeksInMonth = weeksInMonth, DaysOfWeekExt = daysOfWeek };
        }

    }
}