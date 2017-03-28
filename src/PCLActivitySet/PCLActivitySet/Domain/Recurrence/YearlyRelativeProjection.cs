using System;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Domain.Recurrence
{
    /// <summary>
    /// Yearly recurrence on a relative day.
    /// </summary>
    public class YearlyRelativeProjection : IDateProjection
    {
        public EDateProjectionType ProjectionType => EDateProjectionType.YearlyRelative;

        public EMonth Month { get; set; }
        public EWeeksInMonth WeeksInMonth { get; set; }
        public EDaysOfWeekExt DaysOfWeekExt { get; set; }

        public DateTime GetNext(DateTime fromDate)
        {
            DateTime date = fromDate;

            while (!this.MonthMatches(date))
                date = date.AddMonths(-1);

            date = date.AddYears(1);
            return RelativeDate.GetDate(date.Year, date.Month, this.WeeksInMonth, this.DaysOfWeekExt);
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            DateTime date = fromDate;

            while (!this.MonthMatches(date))
                date = date.AddMonths(1);

            date = date.AddYears(-1);
            return RelativeDate.GetDate(date.Year, date.Month, this.WeeksInMonth, this.DaysOfWeekExt);
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
            public ProjectionTranslator(YearlyRelativeProjection projection)
            {
                this._projectionObj = projection;
            }

            private readonly YearlyRelativeProjection _projectionObj;

            public EDateProjectionType ProjectionType => this._projectionObj.ProjectionType;

            public int PeriodCount { get; set; }

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

            public EMonth Month
            {
                get { return this._projectionObj.Month; }
                set { this._projectionObj.Month = value; }
            }

        }

        #endregion

    }

}
