using System;
using System.Linq;

namespace PCLActivitySet
{
    public class FluentlyActivityFilter
    {
        private ActivityList _activityList;

        public FluentlyActivityFilter(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlyActivityFilter FilterOutNonActive()
        {
            this._activityList.InternalActivityFilterList.Add(seq => seq.Where(activity => activity.IsActive));
            return this;
        }

        public FluentlyActivityFilter FilterOutNonActiveWithDelay(TimeSpan filterDelay)
        {
            throw new NotImplementedException();

            //this._activityList.InternalActivityFilterList.Add(seq => seq.Where(activity => activity.IsActive && DateTime.Now > activity.));
            //return this;
        }

        public FluentlyActivityFilter Clear()
        {
            this._activityList.InternalActivityFilterList.Clear();
            return this;
        }
    }
}
