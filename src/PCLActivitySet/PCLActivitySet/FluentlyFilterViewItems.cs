using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class FluentlyFilterViewItems
    {
        private readonly ActivityList _activityList;

        public FluentlyFilterViewItems(ActivityList activityList)
        {
            this._activityList = activityList;
        }



        public TFilter GetFilter<TFilter>()
            where TFilter : IViewItemFilter
        {
            return this._activityList.InternalViewItemFilterList.OfType<TFilter>().FirstOrDefault();
        }

        public void SetFilter<TFilter>(TFilter filterToAdd)
            where TFilter : IViewItemFilter
        {
            this.RemoveFilter<TFilter>();
            this._activityList.InternalViewItemFilterList.Add(filterToAdd);
        }

        public void RemoveFilter<TFilter>() 
            where TFilter : IViewItemFilter
        {
            List<TFilter> oldFiltersToRemove = this._activityList.InternalActivityFilterList.OfType<TFilter>().ToList();
            foreach (TFilter filterToRemove in oldFiltersToRemove)
                this._activityList.InternalViewItemFilterList.Remove(filterToRemove);
        }

        public FluentlyFilterViewItems Clear()
        {
            this._activityList.InternalViewItemFilterList.Clear();
            return this;
        }

        public FluentlyFilterViewItems DateRange(DateTime startDate, DateTime endDate)
        {
            this.SetFilter(new DateRangeFilter(startDate, endDate));
            return this;
        }

        public FluentlyFilterViewItems IncludeHistory()
        {
            throw new NotImplementedException();
        }
    }
}