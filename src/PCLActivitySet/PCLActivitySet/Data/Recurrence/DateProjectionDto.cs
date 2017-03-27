﻿namespace PCLActivitySet.Data.Recurrence
{
    public class DateProjectionDto
    {
        public EDateProjectionType ProjectionType { get; set; }
        public int PeriodCount { get; set; }
        public EDaysOfWeekFlags DaysOfWeekFlags { get; set; }
        public int DayOfMonth { get; set; }
        public EDaysOfWeekExt DaysOfWeekExt { get; set; }
        public EWeeksInMonth WeeksInMonth { get; set; }
        public EMonth Month { get; set; }
    }
}