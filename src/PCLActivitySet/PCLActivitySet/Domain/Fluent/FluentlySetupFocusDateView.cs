using System;
using PCLActivitySet.Domain.Views;

namespace PCLActivitySet.Domain.Fluent
{
    public class FluentlySetupFocusDateView
    {
        private readonly ActivityList _activityList;

        public FluentlySetupFocusDateView(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupFocusDateView FocusDateTime(DateTime focusDateTime)
        {
            this._activityList.InternalFocusDateView.FocusDateTime = focusDateTime;
            return this;
        }

        public FluentlySetupFocusDateView CompletedFilterDelay(TimeSpan completedFilterDelay)
        {
            this.Get().CompletedFilterDelay = completedFilterDelay;
            return this;
        }

        public FluentlySetupFocusDateView Enable()
        {
            this._activityList.InternalActiveView = this.Get();
            return this;
        }

        public FocusDateView Get() => this._activityList.InternalFocusDateView;
    }
}