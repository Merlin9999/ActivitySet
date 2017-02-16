using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet.Recurrence
{
    public enum EDaysOfWeekExt
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        WeekDay,
        WeekendDay,
        EveryDay,
    }

    public static class DaysOfWeekExt
    {
        public static bool DateMatches(DateTime date, EDaysOfWeekExt dowe)
        {
            EDaysOfWeekExt dateDowe = ConvertFrom(date);

            switch (dowe)
            {
                case EDaysOfWeekExt.EveryDay:
                    return true; // Matches every day in the week.
                case EDaysOfWeekExt.WeekDay:
                    return IsWeekDay(dateDowe);
                case EDaysOfWeekExt.WeekendDay:
                    return IsWeekendDay(dateDowe);
                default:
                    return dateDowe == dowe;
            }
        }

        public static bool IsWeekDay(EDaysOfWeekExt dayOfWeek)
        {
            return WeekDays.Contains(dayOfWeek);
        }

        public static bool IsWeekendDay(EDaysOfWeekExt dayOfWeek)
        {
            return WeekendDays.Contains(dayOfWeek);
        }

        public static bool IsDayGroupClassifier(EDaysOfWeekExt dayOfWeek)
        {
            return DayGroupClassifiers.Contains(dayOfWeek);
        }

        public static bool HasWeekDays(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasWeekDays(daysOfWeek, false);
        }

        public static bool HasWeekDays(IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            if (allowEmpty && !daysOfWeek.Any(d => true))
                return true;
            return daysOfWeek.Any(d => IsWeekDay(d));
        }

        public static bool HasWeekendDays(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasWeekendDays(daysOfWeek, false);
        }

        public static bool HasWeekendDays(IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            if (allowEmpty && !daysOfWeek.Any(d => true))
                return true;
            return daysOfWeek.Any(d => IsWeekendDay(d));
        }

        public static bool HasDayGroupClassifier(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasDayGroupClassifier(daysOfWeek, false);
        }

        public static bool HasDayGroupClassifier(IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            if (allowEmpty && !daysOfWeek.Any(d => true))
                return true;
            return daysOfWeek.Any(d => IsDayGroupClassifier(d));
        }

        public static bool HasOnlyWeekDays(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasOnlyWeekDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekDays(IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            if (!allowEmpty && !daysOfWeek.Any(d => true))
                return false;
            return daysOfWeek.All(d => IsWeekDay(d));
        }

        public static bool HasOnlyWeekendDays(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasOnlyWeekendDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekendDays(IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            if (!allowEmpty && !daysOfWeek.Any(d => true))
                return false;
            return daysOfWeek.All(d => IsWeekendDay(d));
        }

        public static bool HasOnlyDayGroupClassifiers(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasOnlyDayGroupClassifiers(daysOfWeek, false);
        }

        public static bool HasOnlyDayGroupClassifiers(IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            if (!allowEmpty && !daysOfWeek.Any(d => true))
                return false;
            return daysOfWeek.All(d => IsDayGroupClassifier(d));
        }

        public static EDaysOfWeekExt ConvertFrom(EDaysOfWeek dayOfWeek)
        {
            if (Enum.IsDefined(typeof(EDaysOfWeek), dayOfWeek))
                return (EDaysOfWeekExt)Enum.Parse(typeof(EDaysOfWeekExt), Enum.GetName(typeof(EDaysOfWeek), dayOfWeek));
            else
                throw new ArgumentException($"Undefined {typeof(EDaysOfWeek)} value ({(int) dayOfWeek}).");
        }

        public static HashSet<EDaysOfWeekExt> ConvertFrom(EDaysOfWeekFlags daysOfWeek)
        {
            HashSet<EDaysOfWeekExt> retVal = new HashSet<EDaysOfWeekExt>();

            foreach (EDaysOfWeekFlags d in DaysOfWeekFlags.AsSeperatedValues(daysOfWeek))
                retVal.Add((EDaysOfWeekExt)Enum.Parse(typeof(EDaysOfWeekExt), Enum.GetName(typeof(EDaysOfWeekFlags), d)));

            return retVal;
        }

        public static EDaysOfWeekExt ConvertFrom(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return EDaysOfWeekExt.Sunday;
                case DayOfWeek.Monday:
                    return EDaysOfWeekExt.Monday;
                case DayOfWeek.Tuesday:
                    return EDaysOfWeekExt.Tuesday;
                case DayOfWeek.Wednesday:
                    return EDaysOfWeekExt.Wednesday;
                case DayOfWeek.Thursday:
                    return EDaysOfWeekExt.Thursday;
                case DayOfWeek.Friday:
                    return EDaysOfWeekExt.Friday;
                case DayOfWeek.Saturday:
                    return EDaysOfWeekExt.Saturday;
                default:
                    throw new InvalidOperationException("Invalid Day Of Week");
            }
        }
        
        public static readonly EDaysOfWeekExt[] WeekDays = { EDaysOfWeekExt.Monday, EDaysOfWeekExt.Tuesday, EDaysOfWeekExt.Wednesday, EDaysOfWeekExt.Thursday, EDaysOfWeekExt.Friday, EDaysOfWeekExt.WeekDay };
        public static readonly EDaysOfWeekExt[] WeekendDays = { EDaysOfWeekExt.Saturday, EDaysOfWeekExt.Sunday, EDaysOfWeekExt.WeekendDay };
        public static readonly EDaysOfWeekExt[] DayGroupClassifiers = { EDaysOfWeekExt.WeekDay, EDaysOfWeekExt.WeekendDay, EDaysOfWeekExt.EveryDay };
    }

}
