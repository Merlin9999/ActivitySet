using PCLActivitySet.Views;

namespace PCLActivitySet.Fluent
{
    public class FluentlySetupExcludeNonActiveView
    {
        private readonly ActivityList _activityList;

        public FluentlySetupExcludeNonActiveView(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupExcludeNonActiveView Enable()
        {
            this._activityList.InternalActiveView = this.Get();
            return this;
        }

        public ExcludeNonActiveView Get() => this._activityList.InternalExcludeNonActiveView;
    }
}