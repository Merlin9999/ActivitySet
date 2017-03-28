using System;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Domain.Recurrence
{
    /// <summary>
    /// Yearly recurrence on a specific date.
    /// </summary>
    public class YearlyProjection : IDateProjection
    {
        public EDateProjectionType ProjectionType => EDateProjectionType.Yearly;

        public EMonth Month { get; set; }
        public int DayOfMonth { get; set; }

        public DateTime GetNext(DateTime fromDate)
        {
            if (this.DayOfMonth <= 0 || this.DayOfMonth > 31)
                throw new InvalidOperationException($"Invalid Day of Month property value ({this.DayOfMonth})");

            DateTime date = fromDate;

            while (!this.MonthMatches(date))
                date = date.AddMonths(-1);

            date = date.AddYears(1);
            int lastDayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            if (this.DayOfMonth > lastDayInMonth)
                return new DateTime(date.Year, date.Month, lastDayInMonth);
            return new DateTime(date.Year, date.Month, this.DayOfMonth);
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            if (this.DayOfMonth <= 0 || this.DayOfMonth > 31)
                throw new InvalidOperationException($"Invalid Day of Month property value ({this.DayOfMonth})");

            DateTime date = fromDate;

            while (!this.MonthMatches(date))
                date = date.AddMonths(1);

            date = date.AddYears(-1);
            int lastDayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            if (this.DayOfMonth > lastDayInMonth)
                return new DateTime(date.Year, date.Month, lastDayInMonth);
            return new DateTime(date.Year, date.Month, this.DayOfMonth);
        }

        protected bool MonthMatches(DateTime dt)
        {
            return dt.Month == Recurrence.Month.GetMonthNumber(this.Month);
        }

        public IDateProjectionTranslator GetTranslator()
        {
            return this._translator ?? (this._translator = new ProjectionTranslator(this));
        }

        private ProjectionTranslator _translator = null;


        #region RecurrenceTranslator Class

        public class ProjectionTranslator : IDateProjectionTranslator
        {
            public ProjectionTranslator(YearlyProjection projection)
            {
                this._projectionObj = projection;
            }

            private readonly YearlyProjection _projectionObj;

            public EDateProjectionType ProjectionType => this._projectionObj.ProjectionType;

            public int PeriodCount { get; set; }

            public EDaysOfWeekFlags DaysOfWeekFlags { get; set; }

            public int DayOfMonth
            {
                get { return this._projectionObj.DayOfMonth; }
                set { this._projectionObj.DayOfMonth = value; }
            }

            public EDaysOfWeekExt DaysOfWeekExt { get; set; }

            public EWeeksInMonth WeeksInMonth { get; set; }

            public EMonth Month
            {
                get { return this._projectionObj.Month; }
                set { this._projectionObj.Month = value; }
            }

        }

        #endregion

    }

}
