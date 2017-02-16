using System;

namespace PCLActivitySet.Recurrence
{
    public static class RelativeDate
    {
        public static DateTime GetDate(int year, int month, EWeeksInMonth weekInMonth, EDaysOfWeek dayOfWeek)
        {
            return GetDate(year, Month.GetMonth(month), weekInMonth, dayOfWeek);
        }

        public static DateTime GetDate(int year, EMonth month, EWeeksInMonth weekInMonth, EDaysOfWeek dayOfWeek)
        {
            return GetDate(year, month, weekInMonth, DaysOfWeekExt.ConvertFrom(dayOfWeek));
        }

        public static DateTime GetDate(int year, int month, EWeeksInMonth weekInMonth, EDaysOfWeekExt dayOfWeekExt)
        {
            return GetDate(year, Month.GetMonth(month), weekInMonth, dayOfWeekExt);
        }

        public static DateTime GetDate(int year, EMonth month, EWeeksInMonth weekInMonth, EDaysOfWeekExt dayOfWeekExt)
        {
            int monthNumber = Month.GetMonthNumber(month);
            bool lookForward = true;

            DateTime date;

            switch (weekInMonth)
            {
                case EWeeksInMonth.First:
                    date = new DateTime(year, monthNumber, 1);
                    break;
                case EWeeksInMonth.Second:
                    date = new DateTime(year, monthNumber, 8);
                    break;
                case EWeeksInMonth.Third:
                    date = new DateTime(year, monthNumber, 15);
                    break;
                case EWeeksInMonth.Fourth:
                    date = new DateTime(year, monthNumber, 22);
                    break;
                case EWeeksInMonth.Last:
                    date = new DateTime(year, monthNumber, DateTime.DaysInMonth(year, monthNumber));
                    lookForward = false;
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized Week in Month ({weekInMonth})!");
            }

            if (lookForward)
            {
                while (!DaysOfWeekExt.DateMatches(date, dayOfWeekExt))
                    date = date.AddDays(1);
            }
            else
            {
                while (!DaysOfWeekExt.DateMatches(date, dayOfWeekExt))
                    date = date.AddDays(-1);
            }

            return date;
        }

    }

}
