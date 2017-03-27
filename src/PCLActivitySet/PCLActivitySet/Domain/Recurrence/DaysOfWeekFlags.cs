using System;
using System.Collections.Generic;
using PCLActivitySet.Data.Recurrence;

namespace PCLActivitySet.Domain.Recurrence
{
    public static class DaysOfWeekFlags
    {
        public static bool DateMatches(this DateTime date, EDaysOfWeekFlags dowFlags)
        {
            return (ConvertFrom(date.DayOfWeek) & dowFlags) != 0;
        }

        public static HashSet<EDaysOfWeekFlags> AsSeperateValues(this EDaysOfWeekFlags daysOfWeek)
        {
            HashSet<EDaysOfWeekFlags> retVal = new HashSet<EDaysOfWeekFlags>();

            foreach (EDaysOfWeekFlags d in Enum.GetValues(typeof(EDaysOfWeekFlags)))
            {
                if ((daysOfWeek & d) != 0)
                    retVal.Add(d);
            }

            return retVal;
        }

        public static bool HasWeekDays(this EDaysOfWeekFlags daysOfWeek)
        {
            return HasWeekDays(daysOfWeek, false);
        }

        public static bool HasWeekDays(this EDaysOfWeekFlags daysOfWeek, bool allowNone)
        {
            if (allowNone && daysOfWeek == EDaysOfWeekFlags.None)
                return true;
            return (WeekDays & daysOfWeek) != 0;
        }

        public static bool HasWeekendDays(this EDaysOfWeekFlags daysOfWeek)
        {
            return HasWeekendDays(daysOfWeek, false);
        }

        public static bool HasWeekendDays(this EDaysOfWeekFlags daysOfWeek, bool allowNone)
        {
            if (allowNone && daysOfWeek == EDaysOfWeekFlags.None)
                return true;
            return (WeekendDays & daysOfWeek) != 0;
        }

        public static bool HasOnlyWeekDays(this EDaysOfWeekFlags daysOfWeek)
        {
            return HasOnlyWeekDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekDays(this EDaysOfWeekFlags daysOfWeek, bool allowNone)
        {
            if (allowNone && daysOfWeek == EDaysOfWeekFlags.None)
                return true;
            return (WeekDays & daysOfWeek) != 0 && (WeekendDays & daysOfWeek) == 0;
        }

        public static bool HasOnlyWeekendDays(this EDaysOfWeekFlags daysOfWeek)
        {
            return HasOnlyWeekendDays(daysOfWeek, false);
        }

        public static bool HasOnlyWeekendDays(this EDaysOfWeekFlags daysOfWeek, bool allowNone)
        {
            if (allowNone && daysOfWeek == EDaysOfWeekFlags.None)
                return true;
            return (WeekendDays & daysOfWeek) != 0 && (WeekDays & daysOfWeek) == 0;
        }

        public static EDaysOfWeekFlags ConvertFrom(EDaysOfWeek dayOfWeek)
        {
            if (Enum.IsDefined(typeof(EDaysOfWeek), dayOfWeek))
                return (EDaysOfWeekFlags)Enum.Parse(typeof(EDaysOfWeekFlags), Enum.GetName(typeof(EDaysOfWeek), dayOfWeek));
            else
                throw new ArgumentException($"Undefined {typeof(EDaysOfWeek)} value ({(int) dayOfWeek}).");
        }

        public static EDaysOfWeekFlags ConvertFrom(IEnumerable<EDaysOfWeek> daysOfWeek)
        {
            EDaysOfWeekFlags retVal = EDaysOfWeekFlags.None;
            foreach (EDaysOfWeek day in daysOfWeek)
                retVal |= ConvertFrom(day);

            return retVal;
        }

        public static EDaysOfWeekFlags ConvertFrom(EDaysOfWeekExt dayOfWeek)
        {
            if (dayOfWeek == EDaysOfWeekExt.WeekDay)
                return WeekDays;
            if (dayOfWeek == EDaysOfWeekExt.WeekendDay)
                return WeekendDays;
            if (dayOfWeek == EDaysOfWeekExt.EveryDay)
                return EveryDay;

            if (Enum.IsDefined(typeof(EDaysOfWeekExt), dayOfWeek))
                return (EDaysOfWeekFlags)Enum.Parse(typeof(EDaysOfWeekFlags), Enum.GetName(typeof(EDaysOfWeekExt), dayOfWeek));
            else
                throw new ArgumentException($"Undefined {typeof(EDaysOfWeekExt)} value ({(int) dayOfWeek}).");}

        public static EDaysOfWeekFlags ConvertFrom(IEnumerable<EDaysOfWeekExt> daysOfWeek)
        {
            EDaysOfWeekFlags retVal = EDaysOfWeekFlags.None;
            foreach (EDaysOfWeekExt day in daysOfWeek)
                retVal |= ConvertFrom(day);

            return retVal;
        }

        public static EDaysOfWeekFlags ConvertFrom(DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday:
                    return EDaysOfWeekFlags.Sunday;
                case DayOfWeek.Monday:
                    return EDaysOfWeekFlags.Monday;
                case DayOfWeek.Tuesday:
                    return EDaysOfWeekFlags.Tuesday;
                case DayOfWeek.Wednesday:
                    return EDaysOfWeekFlags.Wednesday;
                case DayOfWeek.Thursday:
                    return EDaysOfWeekFlags.Thursday;
                case DayOfWeek.Friday:
                    return EDaysOfWeekFlags.Friday;
                case DayOfWeek.Saturday:
                    return EDaysOfWeekFlags.Saturday;
                default:
                    throw new ArgumentException($"Invalid Day Of Week ({dow})");
            }
        }

        public const EDaysOfWeekFlags WeekDays = EDaysOfWeekFlags.Monday | EDaysOfWeekFlags.Tuesday | EDaysOfWeekFlags.Wednesday | EDaysOfWeekFlags.Thursday | EDaysOfWeekFlags.Friday;
        public const EDaysOfWeekFlags WeekendDays = EDaysOfWeekFlags.Saturday | EDaysOfWeekFlags.Sunday;
        public const EDaysOfWeekFlags EveryDay = DaysOfWeekFlags.WeekDays | DaysOfWeekFlags.WeekendDays;
    }
}