using System;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Data.Recurrence
{
    [Flags]
    public enum EDaysOfWeekFlags
    {
        None = 0,
        Sunday = 0x1,
        Monday = 0x2,
        Tuesday = 0x4,
        Wednesday = 0x8,
        Thursday = 0x10,
        Friday = 0x20,
        Saturday = 0x40,
    }
}
