using PCLActivitySet.Db;

namespace PCLActivitySet.Dto.Recurrence
{
    public class DateProjectionDto : AbstractLiteDbValue
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