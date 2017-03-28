using System;
using System.Collections.Generic;
using System.Linq;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Domain.Recurrence
{
    public static class DaysOfWeekExt
    {
        public static bool DateMatches(this DateTime date, EDaysOfWeekExt dowe)
        {
            EDaysOfWeekExt dateDowe = ConvertFrom(date.DayOfWeek);

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

        public static bool IsWeekDay(this EDaysOfWeekExt dayOfWeek)
        {
            return WeekDays.Contains(dayOfWeek);
        }

        public static bool IsWeekendDay(this EDaysOfWeekExt dayOfWeek)
        {
            return WeekendDays.Contains(dayOfWeek);
        }

        public static bool IsDayGroupClassifier(this EDaysOfWeekExt dayOfWeek)
        {
            return DayGroupClassifiers.Contains(dayOfWeek);
        }

        public static bool HasWeekDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasWeekDays(daysOfWeek, false);
        }

        public static bool HasWeekDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeekExt> daysOfWeekList = daysOfWeek as IList<EDaysOfWeekExt> ?? daysOfWeek.ToList();
            if (allowEmpty && !daysOfWeekList.Any())
                return true;
            return daysOfWeekList.Any(IsWeekDay);
        }

        public static bool HasWeekendDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasWeekendDays(daysOfWeek, false);
        }

        public static bool HasWeekendDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeekExt> daysOfWeekList = daysOfWeek as IList<EDaysOfWeekExt> ?? daysOfWeek.ToList();
            if (allowEmpty && !daysOfWeekList.Any())
                return true;
            return daysOfWeekList.Any(IsWeekendDay);
        }

        public static bool HasDayGroupClassifier(this IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasDayGroupClassifier(daysOfWeek, false);
        }

        public static bool HasDayGroupClassifier(this IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeekExt> daysOfWeekList = daysOfWeek as IList<EDaysOfWeekExt> ?? daysOfWeek.ToList();
            if (allowEmpty && !daysOfWeekList.Any())
                return true;
            return daysOfWeekList.Any(IsDayGroupClassifier);
        }

        public static bool HasOnlyWeekDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasOnlyWeekDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeekExt> daysOfWeekList = daysOfWeek as IList<EDaysOfWeekExt> ?? daysOfWeek.ToList();
            if (!allowEmpty && !daysOfWeekList.Any())
                return false;
            return daysOfWeekList.All(IsWeekDay);
        }

        public static bool HasOnlyWeekendDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasOnlyWeekendDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekendDays(this IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeekExt> daysOfWeekList = daysOfWeek as IList<EDaysOfWeekExt> ?? daysOfWeek.ToList();
            if (!allowEmpty && !daysOfWeekList.Any())
                return false;
            return daysOfWeekList.All(IsWeekendDay);
        }

        public static bool HasOnlyDayGroupClassifiers(this IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            return HasOnlyDayGroupClassifiers(daysOfWeek, false);
        }

        public static bool HasOnlyDayGroupClassifiers(this IEnumerable<EDaysOfWeekExt> daysOfWeek, bool allowEmpty)
        {
            IList<EDaysOfWeekExt> daysOfWeekList = daysOfWeek as IList<EDaysOfWeekExt> ?? daysOfWeek.ToList();
            if (!allowEmpty && !daysOfWeekList.Any())
                return false;
            return daysOfWeekList.All(IsDayGroupClassifier);
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

            foreach (EDaysOfWeekFlags d in DaysOfWeekFlags.AsSeperateValues(daysOfWeek))
                retVal.Add((EDaysOfWeekExt)Enum.Parse(typeof(EDaysOfWeekExt), Enum.GetName(typeof(EDaysOfWeekFlags), d)));

            return retVal;
        }

        public static EDaysOfWeekExt ConvertFrom(DayOfWeek dow)
        {
            switch (dow)
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
                    throw new ArgumentException($"Invalid Day Of Week ({dow})");
            }
        }

        public static readonly EDaysOfWeekExt[] WeekDays = { EDaysOfWeekExt.Monday, EDaysOfWeekExt.Tuesday, EDaysOfWeekExt.Wednesday, EDaysOfWeekExt.Thursday, EDaysOfWeekExt.Friday };
        public static readonly EDaysOfWeekExt[] WeekendDays = { EDaysOfWeekExt.Saturday, EDaysOfWeekExt.Sunday };
        public static readonly EDaysOfWeekExt[] DayGroupClassifiers = { EDaysOfWeekExt.WeekDay, EDaysOfWeekExt.WeekendDay, EDaysOfWeekExt.EveryDay };
    }
}