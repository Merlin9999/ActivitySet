using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class ActivityList : AbstractEntity<ActivityList>
    {
        protected readonly ActivityBoard OwningBoard;
        private DateTime? _focusDate;

        public ActivityList(ActivityBoard owningBoard)
        {
            if (owningBoard == null)
                throw new ArgumentNullException(nameof(owningBoard), $"The argument {nameof(owningBoard)} cannot be null.");
            this.OwningBoard = owningBoard;
            this.InternalActivityFilterList = new List<IActivityFilter>();
            this.InternalViewItemFilterList = new List<IViewItemFilter>();
        }

        public string Name { get; set; }

        public DateTime FocusDate => this.FocusDateTime.Date;

        public DateTime FocusDateTime
        {
            get { return this._focusDate ?? DateTime.Now; }
            set { this._focusDate = value; }
        }

        public void ResetFocusDateTimeToNow()
        {
            this._focusDate = null;
        }

        internal List<IActivityFilter> InternalActivityFilterList { get; }

        public FluentlyFilterActivities ActivityFilters => new FluentlyFilterActivities(this);

        public virtual IEnumerable<Activity> Activities => this.AppendActivityEnumerableWithFilter(this.OwningBoard.Activities.Where(activity => activity.ActivityListGuid == this.Guid));

        internal List<IViewItemFilter> InternalViewItemFilterList { get;}

        public virtual FluentlyFilterViewItems ViewItemFilters => new FluentlyFilterViewItems(this);

        public virtual IEnumerable<IViewItem> ViewItems => this.AppendViewItemsEnumerableWithFilter(this.Activities.Select(a => new ActivityViewItem(a)));

        protected virtual IEnumerable<Activity>  AppendActivityEnumerableWithFilter(IEnumerable<Activity> activities)
        {
            IEnumerable<Activity> retVal = activities;

            foreach (IActivityFilter filter in this.InternalActivityFilterList)
                retVal = filter.FilterImpl(retVal);

            return retVal;
        }

        protected virtual IEnumerable<IViewItem> AppendViewItemsEnumerableWithFilter(IEnumerable<IViewItem> viewItems)
        {
            IEnumerable<IViewItem> retVal = viewItems;

            foreach (IViewItemFilter filter in this.InternalViewItemFilterList)
                retVal = filter.FilterImpl(retVal);

            return retVal;
        }
    }
}
