namespace PCLActivitySet.Domain.Fluent
{
    public class FluentlySelectViewMode
    {
        private readonly ActivityList _activityList;

        public FluentlySelectViewMode(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupExcludeNonActiveView ExcludeNonActiveView => new FluentlySetupExcludeNonActiveView(this._activityList);
        public FluentlySetupFocusDateView FocusDateView => new FluentlySetupFocusDateView(this._activityList);
        public FluentlySetupCalendarView CalendarView => new FluentlySetupCalendarView(this._activityList);

        public FluentlySelectViewMode Clear()
        {
            this._activityList.InternalActiveView = null;
            return this;
        }
    }
}