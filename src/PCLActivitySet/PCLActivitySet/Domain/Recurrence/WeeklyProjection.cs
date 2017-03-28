using System;
using System.Collections.Generic;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Domain.Recurrence
{
    public class WeeklyProjection : IDateProjection
    {
        public EDateProjectionType ProjectionType => EDateProjectionType.Weekly;

        /// <summary>
        /// Weekly recurrence on one or more days of the week.
        /// </summary>
        public int WeekCount { get; set; }
        public EDaysOfWeekFlags DaysOfWeek { get; set; }

        public DateTime GetNext(DateTime fromDate)
        {
            if (this.WeekCount <= 0)
                throw new InvalidOperationException($"Invalid Week Count property value ({this.WeekCount})");
            if (this.DaysOfWeek == EDaysOfWeekFlags.None)
                throw new InvalidOperationException("No days of the week selected.");

            DateTime date = fromDate;
            HashSet<EDaysOfWeekFlags> dowSet = DaysOfWeekFlags.AsSeperateValues(this.DaysOfWeek);

            // back up to the last week day that is in DaysOfWeek property and add (WeekCount * 7) days.
            while (!dowSet.Contains(DaysOfWeekFlags.ConvertFrom(date.DayOfWeek)))
                date = date.AddDays(-1);
            return date.AddDays(this.WeekCount * 7).Date;
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            if (this.WeekCount <= 0)
                throw new InvalidOperationException($"Invalid Week Count property value ({this.WeekCount})");
            if (this.DaysOfWeek == EDaysOfWeekFlags.None)
                throw new InvalidOperationException("No days of the week selected.");

            DateTime date = fromDate;
            HashSet<EDaysOfWeekFlags> dowSet = DaysOfWeekFlags.AsSeperateValues(this.DaysOfWeek);

            // move up to the next week day that is in DaysOfWeek property and subtract (WeekCount * 7) days.
            while (!dowSet.Contains(DaysOfWeekFlags.ConvertFrom(date.DayOfWeek)))
                date = date.AddDays(1);
            return date.AddDays(-(this.WeekCount * 7)).Date;
        }

        public IDateProjectionTranslator GetTranslator()
        {
            return this._translator ?? (this._translator = new ProjectionTranslator(this));
        }

        private ProjectionTranslator _translator = null;


        #region RecurrenceTranslator Class

        public class ProjectionTranslator : IDateProjectionTranslator
        {
            public ProjectionTranslator(WeeklyProjection projection)
            {
                this._projectionObj = projection;
            }

            private readonly WeeklyProjection _projectionObj;

            public EDateProjectionType ProjectionType => this._projectionObj.ProjectionType;

            public int PeriodCount
            {
                get { return this._projectionObj.WeekCount; }
                set { this._projectionObj.WeekCount = value; }
            }

            public EDaysOfWeekFlags DaysOfWeekFlags
            {
                get { return this._projectionObj.DaysOfWeek; }
                set { this._projectionObj.DaysOfWeek = value; }
            }

            public int DayOfMonth { get; set; }

            public EDaysOfWeekExt DaysOfWeekExt { get; set; }

            public EWeeksInMonth WeeksInMonth { get; set; }

            public EMonth Month { get; set; }

        }

        #endregion

    }

}
