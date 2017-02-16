using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet.Recurrence
{
    public enum EDaysOfWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }

    public static class DaysOfWeek
    {
        public static bool DateMatches(DateTime date, EDaysOfWeek dow)
        {
            return ConvertFrom(date) == dow;
        }

        public static bool IsWeekDay(EDaysOfWeek dayOfWeek)
        {
            return WeekDays.Contains(dayOfWeek);
        }

        public static bool IsWeekendDay(EDaysOfWeek dayOfWeek)
        {
            return WeekendDays.Contains(dayOfWeek);
        }

        public static bool HasWeekDays(IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasWeekDays(daysOfWeek, false);
        }

        public static bool HasWeekDays(IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            if (allowEmpty && !daysOfWeek.Any(d => true))
                return true;
            return daysOfWeek.Any(d => IsWeekDay(d));
        }

        public static bool HasWeekendDays(IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasWeekendDays(daysOfWeek, false);
        }

        public static bool HasWeekendDays(IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            if (allowEmpty && !daysOfWeek.Any(d => true))
                return true;
            return daysOfWeek.Any(d => IsWeekendDay(d));
        }

        public static bool HasOnlyWeekDays(IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasOnlyWeekDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekDays(IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            if (!allowEmpty && !daysOfWeek.Any(d => true))
                return false;
            return daysOfWeek.All(d => IsWeekDay(d));
        }

        public static bool HasOnlyWeekendDays(IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasOnlyWeekendDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekendDays(IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            if (!allowEmpty && !daysOfWeek.Any(d => true))
                return false;
            return daysOfWeek.All(d => IsWeekendDay(d));
        }

        public static EDaysOfWeek ConvertFrom(EDaysOfWeekExt dayOfWeek)
        {
            if (DaysOfWeekExt.IsDayGroupClassifier(dayOfWeek))
                throw new ArgumentException($"Day Group Classifiers of the type {typeof(EDaysOfWeekExt)} cannnot be converted to the type {typeof(EDaysOfWeek)}.");

            if (Enum.IsDefined(typeof(EDaysOfWeekExt), dayOfWeek))
                return (EDaysOfWeek)Enum.Parse(typeof(EDaysOfWeek), Enum.GetName(typeof(EDaysOfWeekExt), dayOfWeek));
            else
                throw new ArgumentException($"Undefined {typeof(EDaysOfWeekExt)} value ({(int) dayOfWeek}).");
        }

        public static HashSet<EDaysOfWeek> ConvertFrom(EDaysOfWeekFlags daysOfWeek)
        {
            //if (daysOfWeek == EDaysOfWeekFlags.None)
            //    throw new ArgumentException(string.Format("There is no defined conversion from {0}.None to type {1}.", typeof(EDaysOfWeekFlags), typeof(EDaysOfWeek)));

            HashSet<EDaysOfWeek> retVal = new HashSet<EDaysOfWeek>();

            foreach (EDaysOfWeekFlags d in DaysOfWeekFlags.AsSeperatedValues(daysOfWeek))
                retVal.Add((EDaysOfWeek)Enum.Parse(typeof(EDaysOfWeek), Enum.GetName(typeof(EDaysOfWeekFlags), d)));

            return retVal;
        }

        public static EDaysOfWeek ConvertFrom(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return EDaysOfWeek.Sunday;
                case DayOfWeek.Monday:
                    return EDaysOfWeek.Monday;
                case DayOfWeek.Tuesday:
                    return EDaysOfWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return EDaysOfWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return EDaysOfWeek.Thursday;
                case DayOfWeek.Friday:
                    return EDaysOfWeek.Friday;
                case DayOfWeek.Saturday:
                    return EDaysOfWeek.Saturday;
                default:
                    throw new InvalidOperationException("Invalid Day Of Week");
            }
        }

        public static readonly EDaysOfWeek[] WeekDays = { EDaysOfWeek.Monday, EDaysOfWeek.Tuesday, EDaysOfWeek.Wednesday, EDaysOfWeek.Thursday, EDaysOfWeek.Friday };
        public static readonly EDaysOfWeek[] WeekendDays = { EDaysOfWeek.Saturday, EDaysOfWeek.Sunday };
    }

}
