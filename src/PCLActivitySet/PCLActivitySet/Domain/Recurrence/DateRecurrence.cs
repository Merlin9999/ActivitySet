using System;

namespace PCLActivitySet.Domain.Recurrence
{
    public class DateRecurrence : DateProjection
    {
        private int _maxRecurrenceCount;
        private DateTime _startDate;
        private DateTime _endDate;

        public DateRecurrence()
        {
            this.MaxRecurrenceCount = 0;
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MaxValue;
            this.RecurFromType = ERecurFromType.FromActiveDueDate;
        }

        public DateRecurrence(EDateProjectionType recurType, ERecurFromType recurFrom)
            : base(recurType)
        {
            this.MaxRecurrenceCount = 0;
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MaxValue;
            this.RecurFromType = recurFrom;
        }

        public DateRecurrence(EDateProjectionType recurType, ERecurFromType recurFrom, int maxRecurrenceCount)
            : base(recurType)
        {
            this.MaxRecurrenceCount = maxRecurrenceCount;
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MaxValue;
            this.RecurFromType = recurFrom;
        }

        public DateRecurrence(EDateProjectionType recurType, ERecurFromType recurFrom, DateTime startDate, DateTime endDate)
            : base(recurType)
        {
            this.MaxRecurrenceCount = 0;

            this.StartDate = startDate;
            this.EndDate = endDate;

            if (this.StartDate > this.EndDate)
                throw new ArgumentException($"Start date ({startDate}) is after the end date ({endDate}).");

            this.RecurFromType = recurFrom;
        }

        public DateRecurrence(IDateProjection recurObj, ERecurFromType recurFrom)
            : base(recurObj)
        {
            this.MaxRecurrenceCount = 0;
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MaxValue;
            this.RecurFromType = recurFrom;
        }

        public DateRecurrence(IDateProjection recurObj, ERecurFromType recurFrom, int maxRecurrenceCount)
            : base(recurObj)
        {
            this.MaxRecurrenceCount = maxRecurrenceCount;
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MaxValue;
            this.RecurFromType = recurFrom;
        }

        public DateRecurrence(IDateProjection recurObj, ERecurFromType recurFrom, DateTime startDate, DateTime endDate)
            : base(recurObj)
        {
            this.MaxRecurrenceCount = 0;

            this.StartDate = startDate;
            this.EndDate = endDate;

            if (this.StartDate > this.EndDate)
                throw new ArgumentException($"Start date ({startDate}) is after the end date ({endDate}).");

            this.RecurFromType = recurFrom;
        }

        public DateTime? GetNext(DateTime dueDate, int timesCompleted)
        {
            DateTime dt = this.GetNext(dueDate);
            return this.IsProjectedDateValid(dt, timesCompleted) ? dt : (DateTime?)null;
        }

        public DateTime? GetNext(DateTime dueDate, DateTime completionDate, int timesCompleted)
        {
            DateTime dt;
            switch (this.RecurFromType)
            {
                case ERecurFromType.FromActiveDueDate:
                    dt = this.GetNext(dueDate);
                    break;
                case ERecurFromType.FromCompletedDate:
                    dt = this.GetNext(completionDate);
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized value for RecurFromType property ({this.RecurFromType})!");
            }

            return this.IsProjectedDateValid(dt, timesCompleted) ? dt : (DateTime?)null;
        }


        private bool IsProjectedDateValid(DateTime projectedDate, int timesCompleted)
        {
            return (this.MaxRecurrenceCount == 0 || timesCompleted < this.MaxRecurrenceCount) &&
                (projectedDate >= this.StartDate && projectedDate < (this.EndDate <= DateTime.MaxValue.AddDays(-1) ? this.EndDate.AddDays(1) : DateTime.MaxValue));
        }

        public ERecurFromType RecurFromType { get; set; }

        public int MaxRecurrenceCount
        {
            get { return this._maxRecurrenceCount; }
            set { this._maxRecurrenceCount = value < 0 ? 0 : value; }
        }

        public DateTime StartDate
        {
            get { return this._startDate; }
            set { this._startDate = value.Date; }
        }

        public DateTime EndDate
        {
            get { return this._endDate; }
            set { this._endDate = value.Date; }
        }

    }

}
