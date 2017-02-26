using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class FluentlyFilterActivities
    {
        private readonly ActivityList _activityList;

        public FluentlyFilterActivities(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public TFilter GetFilter<TFilter>()
            where TFilter : IActivityFilter
        {
            return this._activityList.InternalActivityFilterList.OfType<TFilter>().FirstOrDefault();
        }

        public void SetFilter<TFilter>(TFilter filterToAdd)
            where TFilter : IActivityFilter
        {
            this.RemoveFilter<TFilter>();
            this._activityList.InternalActivityFilterList.Add(filterToAdd);
        }

        public void RemoveFilter<TFilter>() where TFilter : IActivityFilter
        {
            List<TFilter> oldFiltersToRemove = this._activityList.InternalActivityFilterList.OfType<TFilter>().ToList();
            foreach (TFilter filterToRemove in oldFiltersToRemove)
                this._activityList.InternalActivityFilterList.Remove(filterToRemove);
        }

        public FluentlyFilterActivities Clear()
        {
            this._activityList.InternalActivityFilterList.Clear();
            return this;
        }

        public FluentlyFilterActivities ExcludeNonActive()
        {
            this.SetFilter(new ExcludeNonActiveWithDelayFilter(this._activityList, null));
            return this;
        }

        public FluentlyFilterActivities ExcludeNonActiveWithDelay(TimeSpan filterDelay)
        {
            this.SetFilter(new ExcludeNonActiveWithDelayFilter(this._activityList, filterDelay));
            return this;
        }

        public ExcludeNonActiveWithDelayFilter GetExcludeNonActiveWithDelay()
        {
            return this.GetFilter<ExcludeNonActiveWithDelayFilter>();
        }

        public FluentlyFilterActivities FocusDateRadar()
        {
            this.SetFilter(new FocusDateRadarFilter(this._activityList));
            return this;
        }
    }
}
