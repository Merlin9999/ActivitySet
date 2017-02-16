using System;

namespace PCLActivitySet.Recurrence
{
    /// <summary>
    /// Monthly recurrence on a specific date.
    /// </summary>
    public class MonthlyProjection : IDateProjection
    {
        public EDateProjectionType ProjectionType => EDateProjectionType.Monthly;

        public int MonthCount { get; set; }
        public int DayOfMonth { get; set; }

        public DateTime GetNext(DateTime fromDate)
        {
            if (this.MonthCount <= 0)
                throw new InvalidOperationException($"Invalid Month Count property value ({this.MonthCount})");
            if (this.DayOfMonth <= 0 || this.DayOfMonth > 31)
                throw new InvalidOperationException($"Invalid Day of Month property value ({this.DayOfMonth})");

            DateTime date = fromDate;

            while (!this.DateMatches(date))
                date = date.AddDays(-1);

            date = date.AddMonths(this.MonthCount);
            int lastDayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            if (this.DayOfMonth > lastDayInMonth)
                return new DateTime(date.Year, date.Month, lastDayInMonth);
            return new DateTime(date.Year, date.Month, this.DayOfMonth);
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            if (this.MonthCount <= 0)
                throw new InvalidOperationException($"Invalid Month Count property value ({this.MonthCount})");
            if (this.DayOfMonth <= 0 || this.DayOfMonth > 31)
                throw new InvalidOperationException($"Invalid Day of Month property value ({this.DayOfMonth})");

            DateTime date = fromDate;

            while (!this.DateMatches(date))
                date = date.AddDays(1);

            date = date.AddMonths(-this.MonthCount);
            int lastDayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            if (this.DayOfMonth > lastDayInMonth)
                return new DateTime(date.Year, date.Month, lastDayInMonth);
            return new DateTime(date.Year, date.Month, this.DayOfMonth);
        }

        protected bool DateMatches(DateTime dt)
        {
            int lastDayInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);

            if (this.DayOfMonth > lastDayInMonth)
                return dt.Day == lastDayInMonth;

            return dt.Day == this.DayOfMonth;
        }

        public IDateProjectionTranslator GetTranslator()
        {
            return this._translator ?? (this._translator = new ProjectionTranslator(this));
        }

        private ProjectionTranslator _translator = null;


        #region RecurrenceTranslator Class

        public class ProjectionTranslator : IDateProjectionTranslator
        {
            public ProjectionTranslator(MonthlyProjection projection)
            {
                this._projectionObj = projection;
            }

            private readonly MonthlyProjection _projectionObj;

            public EDateProjectionType ProjectionType => this._projectionObj.ProjectionType;

            public int PeriodCount
            {
                get { return this._projectionObj.MonthCount; }
                set { this._projectionObj.MonthCount = value; }
            }

            public EDaysOfWeekFlags DaysOfWeekFlags { get; set; }

            public int DayOfMonth
            {
                get { return this._projectionObj.DayOfMonth; }
                set { this._projectionObj.DayOfMonth = value; }
            }

            public EDaysOfWeekExt DaysOfWeekExt { get; set; }

            public EWeeksInMonth WeeksInMonth { get; set; }

            public EMonth Month { get; set; }

        }

        #endregion

    }

}
