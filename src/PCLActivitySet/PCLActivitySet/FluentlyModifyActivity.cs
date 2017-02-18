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

        public FluentlyModifyActivity LeadTime(IDateProjection projection)
        {
            this._activity.LeadTime = projection;
            return this;
        }

        public FluentlyModifyActivity DailyLeadTime(int dayCount)
        {
            this._activity.LeadTime = new DailyProjection() { DayCount = dayCount};
            return this;
        }

        public FluentlyModifyActivity WeeklyLeadTime(int weekCount, EDaysOfWeekFlags daysOfWeek)
        {
            this._activity.LeadTime = new WeeklyProjection() {DaysOfWeek = daysOfWeek, WeekCount = weekCount};
            return this;
        }

        public FluentlyModifyActivity MonthlyLeadTime(int monthCount, int dayOfMonth)
        {
            this._activity.LeadTime = new MonthlyProjection() {MonthCount = monthCount, DayOfMonth = dayOfMonth};
            return this;
        }

        public FluentlyModifyActivity MonthlyLeadTime(int monthCount, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this._activity.LeadTime = new MonthlyRelativeProjection() { MonthCount = monthCount, WeeksInMonth = weeksInMonth, DaysOfWeekExt = daysOfWeek};
            return this;
        }

        public FluentlyModifyActivity YearlyLeadTime(EMonth month, int dayOfMonth)
        {
            this._activity.LeadTime = new YearlyProjection() { Month = month, DayOfMonth = dayOfMonth };
            return this;
        }

        public FluentlyModifyActivity YearlyLeadTime(EMonth month, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this._activity.LeadTime = new YearlyRelativeProjection() { Month = month, WeeksInMonth = weeksInMonth, DaysOfWeekExt = daysOfWeek };
            return this;
        }
    }
}
