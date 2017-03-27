using System;
using PCLActivitySet.Domain.Views;

namespace PCLActivitySet.Domain.Fluent
{
    public class FluentlySetupCalendarView
    {
        private readonly ActivityList _activityList;

        public FluentlySetupCalendarView(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupCalendarView DateRange(DateTime startDate, DateTime endDate)
        {
            CalendarView view = this._activityList.InternalCalendarView;
            view.StartDate = startDate.Date;
            view.EndDate = endDate.Date.AddDays(1);
            return this;
        }

        public FluentlySetupCalendarView IncludeHistory(bool includeHistory = true)
        {
            CalendarView view = this._activityList.InternalCalendarView;
            view.IncludeHistory = includeHistory;
            return this;
        }

        public FluentlySetupCalendarView IncludeFuture(bool includeFuture = true)
        {
            CalendarView view = this._activityList.InternalCalendarView;
            view.IncludeFuture = includeFuture;
            return this;
        }

        public FluentlySetupCalendarView Enable()
        {
            this._activityList.InternalActiveView = this.Get();
            return this;
        }

        private CalendarView Get() => this._activityList.InternalCalendarView;
    }
}