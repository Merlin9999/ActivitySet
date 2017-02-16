using System;

namespace PCLActivitySet.Recurrence
{
    public static class Month
    {
        public static int GetMonthNumber(EMonth month)
        {
            if (!Enum.IsDefined(typeof(EMonth), month))
                throw new InvalidOperationException($"The month value ({month}) is not defined!");

            return (int)month;
        }

        public static EMonth GetMonth(int month)
        {
            if (!Enum.IsDefined(typeof(EMonth), month))
                throw new InvalidOperationException($"The month value ({month}) is not defined!");

            return (EMonth)month;
        }

    }

}
