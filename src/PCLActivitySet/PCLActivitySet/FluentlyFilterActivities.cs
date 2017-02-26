using System;
using System.Linq;

namespace PCLActivitySet
{
    public class FluentlyFilterActivities
    {
        private ActivityList _activityList;

        public FluentlyFilterActivities(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public TFilter GetFilter<TFilter>()
        {
            return this._activityList.InternalActivityFilterList.OfType<TFilter>().FirstOrDefault();
        }

        public FluentlyFilterActivities Clear()
        {
            this._activityList.InternalActivityFilterList.Clear();
            return this;
        }

        public FluentlyFilterActivities ExcludeNonActive()
        {
            this._activityList.InternalActivityFilterList.Add(new ExcludeNonActiveFilter());
            return this;
        }

        public FluentlyFilterActivities ExcludeNonActiveWithDelay(TimeSpan filterDelay)
        {
            this._activityList.InternalActivityFilterList.Add(new ExcludeNonActiveWithDelayFilter(this._activityList, filterDelay));
            return this;
        }

        public ExcludeNonActiveWithDelayFilter GetExcludeNonActiveWithDelay()
        {
            return this.GetFilter<ExcludeNonActiveWithDelayFilter>();
        }
    }
}
