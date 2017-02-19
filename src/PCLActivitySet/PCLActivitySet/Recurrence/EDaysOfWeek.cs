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
        public static bool DateMatches(this DateTime date, EDaysOfWeek dow)
        {
            return ConvertFrom(date.DayOfWeek) == dow;
        }

        public static bool IsWeekDay(this EDaysOfWeek dayOfWeek)
        {
            return WeekDays.Contains(dayOfWeek);
        }

        public static bool IsWeekendDay(this EDaysOfWeek dayOfWeek)
        {
            return WeekendDays.Contains(dayOfWeek);
        }

        public static bool HasWeekDays(this IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasWeekDays(daysOfWeek, false);
        }

        public static bool HasWeekDays(this IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeek> daysOfWeekList = daysOfWeek as IList<EDaysOfWeek> ?? daysOfWeek.ToList();
            if (allowEmpty && !daysOfWeekList.Any())
                return true;
            return daysOfWeekList.Any(IsWeekDay);
        }

        public static bool HasWeekendDays(this IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasWeekendDays(daysOfWeek, false);
        }

        public static bool HasWeekendDays(this IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeek> daysOfWeekList = daysOfWeek as IList<EDaysOfWeek> ?? daysOfWeek.ToList();
            if (allowEmpty && !daysOfWeekList.Any())
                return true;
            return daysOfWeekList.Any(IsWeekendDay);
        }

        public static bool HasOnlyWeekDays(this IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasOnlyWeekDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekDays(this IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeek> daysOfWeekList = daysOfWeek as IList<EDaysOfWeek> ?? daysOfWeek.ToList();
            if (!allowEmpty && !daysOfWeekList.Any())
                return false;
            return daysOfWeekList.All(IsWeekDay);
        }

        public static bool HasOnlyWeekendDays(this IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            return HasOnlyWeekendDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekendDays(this IEnumerable<EDaysOfWeek> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeek> daysOfWeekList = daysOfWeek as IList<EDaysOfWeek> ?? daysOfWeek.ToList();
            if (!allowEmpty && !daysOfWeekList.Any())
                return false;
            return daysOfWeekList.All(IsWeekendDay);
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
            HashSet<EDaysOfWeek> retVal = new HashSet<EDaysOfWeek>();

            foreach (EDaysOfWeekFlags d in DaysOfWeekFlags.AsSeperateValues(daysOfWeek))
                retVal.Add((EDaysOfWeek)Enum.Parse(typeof(EDaysOfWeek), Enum.GetName(typeof(EDaysOfWeekFlags), d)));

            return retVal;
        }

        public static EDaysOfWeek ConvertFrom(DayOfWeek dow)
        {
            switch (dow)
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
                    throw new ArgumentException($"Invalid Day Of Week ({dow})");
            }
        }

        public static readonly EDaysOfWeek[] WeekDays = { EDaysOfWeek.Monday, EDaysOfWeek.Tuesday, EDaysOfWeek.Wednesday, EDaysOfWeek.Thursday, EDaysOfWeek.Friday };
        public static readonly EDaysOfWeek[] WeekendDays = { EDaysOfWeek.Saturday, EDaysOfWeek.Sunday };
    }

}
