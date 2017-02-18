using System;
using System.Collections.Generic;
using System.Linq;
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

        public FluentlyModifyActivity AddTo(ActivitySet activitySet)
        {
            activitySet.Add(this._activity);
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
            this._activity.LeadTime = new DateProjection(new DailyProjection() { DayCount = periodCount});
            return this;
        }

        public FluentlyModifyActivity WeeklyLeadTime(int periodCount, EDaysOfWeekFlags daysOfWeek)
        {
            this._activity.LeadTime = new DateProjection(new WeeklyProjection() {DaysOfWeek = daysOfWeek, WeekCount = periodCount});
            return this;
        }

        public FluentlyModifyActivity MonthlyLeadTime(int periodCount, int dayOfMonth)
        {
            this._activity.LeadTime = new DateProjection(new MonthlyProjection() {MonthCount = periodCount, DayOfMonth = dayOfMonth});
            return this;
        }

        public FluentlyModifyActivity MonthlyLeadTime(int periodCount, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this._activity.LeadTime = new DateProjection(new MonthlyRelativeProjection() { MonthCount = periodCount, WeeksInMonth = weeksInMonth, DaysOfWeekExt = daysOfWeek});
            return this;
        }

        public FluentlyModifyActivity YearlyLeadTime(EMonth month, int dayOfMonth)
        {
            this._activity.LeadTime = new DateProjection(new YearlyProjection() { Month = month, DayOfMonth = dayOfMonth });
            return this;
        }

        public FluentlyModifyActivity YearlyLeadTime(EMonth month, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this._activity.LeadTime = new DateProjection(new YearlyRelativeProjection() { Month = month, WeeksInMonth = weeksInMonth, DaysOfWeekExt = daysOfWeek });
            return this;
        }
    }
}
