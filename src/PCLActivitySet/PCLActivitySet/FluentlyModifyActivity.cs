using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet
{
    public class FluentlyModifyActivity
    {
        private readonly Activity _activity;

        public Activity ToActivity => this._activity;

        public static implicit operator Activity(FluentlyModifyActivity _this)
        {
            return _this._activity;
        }

        public FluentlyModifyActivity(Activity activity)
        {
            this._activity = activity;
        }

        public FluentlyModifyActivity Name(string newName)
        {
            this._activity.Name = newName;
            return this;
        }

        public FluentlyModifyActivity ActiveDueDate(DateTime newActiveDueDate)
        {
            this._activity.ActiveDueDate = newActiveDueDate;
            return this;
        }

        public FluentlyModifyActivity AddTo(ActivityBoard activityBoard)
        {
            activityBoard.Add(this._activity);
            return this;
        }

        public FluentlyModifyActivity LeadTime(IDateProjection dateProjection)
        {
            this._activity.LeadTime = new DateProjection(dateProjection);
            return this;
        }

        public FluentlyModifyActivity LeadTime(DateProjection dateProjection)
        {
            this._activity.LeadTime = dateProjection;
            return this;
        }

        public FluentlyModifyActivity DailyLeadTime(int periodCount)
        {
            this._activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Daily(periodCount));
            return this;
        }

        public FluentlyModifyActivity WeeklyLeadTime(int periodCount, EDaysOfWeekFlags daysOfWeek)
        {
            this._activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Weekly(periodCount, daysOfWeek));
            return this;
        }

        public FluentlyModifyActivity MonthlyLeadTime(int periodCount, int dayOfMonth)
        {
            this._activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Monthly(periodCount, dayOfMonth));
            return this;
        }

        public FluentlyModifyActivity MonthlyLeadTime(int periodCount, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this._activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Monthly(periodCount, weeksInMonth, daysOfWeek));
            return this;
        }

        public FluentlyModifyActivity YearlyLeadTime(EMonth month, int dayOfMonth)
        {
            this._activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Yearly(month, dayOfMonth));
            return this;
        }

        public FluentlyModifyActivity YearlyLeadTime(EMonth month, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this._activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Yearly(month, weeksInMonth, daysOfWeek));
            return this;
        }

        public FluentlyModifyActivity Recurrence(DateRecurrence dateRecurrence)
        {
            this._activity.Recurrence = dateRecurrence;
            return this;
        }

        public FluentlyModifyActivity Recurrence(ERecurFromType recurFromType, Func<DateProjectionCreateHelper, IDateProjection> dateProjectionFactory)
        {
            IDateProjection dateProjection = dateProjectionFactory(new DateProjectionCreateHelper());
            this._activity.Recurrence = new DateRecurrence(dateProjection, recurFromType);
            return this;
        }

        public FluentlyModifyActivity Recurrence(ERecurFromType recurFromType, int maxRecurrenceCount, Func<DateProjectionCreateHelper, IDateProjection> dateProjectionFactory)
        {
            IDateProjection dateProjection = dateProjectionFactory(new DateProjectionCreateHelper());
            this._activity.Recurrence = new DateRecurrence(dateProjection, recurFromType, maxRecurrenceCount);
            return this;
        }

        public FluentlyModifyActivity Recurrence(ERecurFromType recurFromType, DateTime startDate, DateTime endDate, Func<DateProjectionCreateHelper, IDateProjection> dateProjectionFactory)
        {
            IDateProjection dateProjection = dateProjectionFactory(new DateProjectionCreateHelper());
            this._activity.Recurrence = new DateRecurrence(dateProjection, recurFromType, startDate, endDate);
            return this;
        }
    }
}
