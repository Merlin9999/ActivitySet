using System;

namespace PCLActivitySet.Recurrence
{
    /// <summary>
    /// Monthly recurrence on a relative day.
    /// </summary>
    public class MonthlyRelativeProjection : IDateProjection
    {
        public EDateProjectionType ProjectionType => EDateProjectionType.MonthlyRelative;

        public int MonthCount { get; set; }
        public EWeeksInMonth WeeksInMonth { get; set; }
        public EDaysOfWeekExt DaysOfWeekExt { get; set; }

        public DateTime GetNext(DateTime fromDate)
        {
            if (this.MonthCount <= 0)
                throw new InvalidOperationException($"Invalid Month Count property value ({this.MonthCount})");

            DateTime date = fromDate;

            while (!this.DateMatches(date))
                date = date.AddDays(-1);

            date = date.AddMonths(this.MonthCount);
            return RelativeDate.GetDate(date.Year, date.Month, this.WeeksInMonth, this.DaysOfWeekExt);
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            if (this.MonthCount <= 0)
                throw new InvalidOperationException($"Invalid Month Count property value ({this.MonthCount})");

            DateTime date = fromDate;

            while (!this.DateMatches(date))
                date = date.AddDays(1);

            date = date.AddMonths(-this.MonthCount);
            return RelativeDate.GetDate(date.Year, date.Month, this.WeeksInMonth, this.DaysOfWeekExt);
        }

        protected bool DateMatches(DateTime date)
        {
            DateTime matchDate = RelativeDate.GetDate(date.Year, date.Month, this.WeeksInMonth, this.DaysOfWeekExt);
            return matchDate == date.Date;
        }

        public IDateProjectionTranslator GetTranslator()
        {
            return this._translator ?? (this._translator = new ProjectionTranslator(this));
        }

        private ProjectionTranslator _translator = null;


        #region RecurrenceTranslator Class

        public class ProjectionTranslator : IDateProjectionTranslator
        {
            public ProjectionTranslator(MonthlyRelativeProjection projection)
            {
                this._projectionObj = projection;
            }

            private readonly MonthlyRelativeProjection _projectionObj;

            public EDateProjectionType ProjectionType => this._projectionObj.ProjectionType;

            public int PeriodCount
            {
                get { return this._projectionObj.MonthCount; }
                set { this._projectionObj.MonthCount = value; }
            }

            public EDaysOfWeekFlags DaysOfWeekFlags { get; set; }

            public int DayOfMonth { get; set; }

            public EDaysOfWeekExt DaysOfWeekExt
            {
                get { return this._projectionObj.DaysOfWeekExt; }
                set { this._projectionObj.DaysOfWeekExt = value; }
            }

            public EWeeksInMonth WeeksInMonth
            {
                get { return this._projectionObj.WeeksInMonth; }
                set { this._projectionObj.WeeksInMonth = value; }
            }

            public EMonth Month { get; set; }

        }

        #endregion

    }

}
