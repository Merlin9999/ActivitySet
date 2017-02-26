using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class ActivityList : AbstractEntity<ActivityList>
    {
        protected readonly ActivityBoard OwningBoard;

        public ActivityList(ActivityBoard owningBoard)
        {
            if (owningBoard == null)
                throw new ArgumentNullException(nameof(owningBoard), $"The argument {nameof(owningBoard)} cannot be null.");
            this.OwningBoard = owningBoard;
            this.InternalActivityFilterList = new List<Func<IEnumerable<Activity>, IEnumerable<Activity>>>();
        }

        public string Name { get; set; }

        internal List<Func<IEnumerable<Activity>, IEnumerable<Activity>>> InternalActivityFilterList { get; }

        public FluentlyActivityFilter ActivityFilter => new FluentlyActivityFilter(this);

        public virtual IEnumerable<Activity> Activities => this.AppendActivityEnumerableWithFilter(this.OwningBoard.Activities.Where(activity => activity.ActivityListGuid == this.Guid));

        protected virtual IEnumerable<Activity>  AppendActivityEnumerableWithFilter(IEnumerable<Activity> activities)
        {
            IEnumerable<Activity> retVal = activities;

            foreach (var filterFunc in this.InternalActivityFilterList)
                retVal = filterFunc(retVal);

            return retVal;
        }
    }
}