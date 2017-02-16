using System;

namespace PCLActivitySet.Recurrence
{
    /// <summary>
    /// Daily recurrence after a certain number of days.
    /// </summary>
    public class DailyProjection : IDateProjection
    {
        public EDateProjectionType ProjectionType => EDateProjectionType.Daily;

        public int DayCount { get; set; }

        public DateTime GetNext(DateTime fromDate)
        {
            if (this.DayCount <= 0)
                throw new InvalidOperationException($"Invalid Day Count property value ({this.DayCount})");

            return fromDate.AddDays(this.DayCount).Date;
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            if (this.DayCount <= 0)
                throw new InvalidOperationException($"Invalid Day Count property value ({this.DayCount})");

            return fromDate.AddDays(-this.DayCount).Date;
        }

        public IDateProjectionTranslator GetTranslator()
        {
            if (this._translator == null)
                this._translator = new ProjectionTranslator(this);

            return this._translator;
        }

        private ProjectionTranslator _translator = null;


        #region RecurrenceTranslator Class

        public class ProjectionTranslator : IDateProjectionTranslator
        {
            public ProjectionTranslator(DailyProjection projection)
            {
                this.projectionObj = projection;
            }

            private readonly DailyProjection projectionObj;

            public EDateProjectionType ProjectionType => this.projectionObj.ProjectionType;

            public int PeriodCount
            {
                get { return this.projectionObj.DayCount; }
                set { this.projectionObj.DayCount = value; }
            }

            public EDaysOfWeekFlags DaysOfWeekFlags { get; set; }

            public int DayOfMonth { get; set; }

            public EDaysOfWeekExt DaysOfWeekExt { get; set; }

            public EWeeksInMonth WeeksInMonth { get; set; }

            public EMonth Month { get; set; }

        }

        #endregion

    }

}
