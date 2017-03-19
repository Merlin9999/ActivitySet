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

        public virtual IEnumerable<IViewItem> ViewItems => this.GenerateViewItems();

        protected virtual IEnumerable<Activity> AppendActivityEnumerableWithFilter(IEnumerable<Activity> activities)
        {
            IEnumerable<Activity> retVal = activities;

            foreach (IActivityFilter filter in this.InternalActivityFilterList)
                retVal = filter.FilterImpl(retVal);

            return retVal;
        }

        protected virtual IEnumerable<IViewItem> GenerateViewItems()
        {

            IEnumerable<IViewItem> retVal =  this.Activities.Select(a => new ActivityViewItem(a));

            //IEnumerable<IViewItem> retVal = Enumerable.Empty<IViewItem>();

            foreach (IViewItemFilter filter in this.InternalViewItemFilterList)
                retVal = filter.FilterImpl(retVal);

            return retVal;
        }
    }

    //public class ViewItemsGenerator
    //{
    //    private readonly ActivityList _activityList;

    //    public ViewItemsGenerator(ActivityList activityList)
    //    {
    //        this._activityList = activityList;
    //    }

    //    public bool IncludeHistory { get; set; }
    //    public bool IncludeFuture { get; set; }

    //    Func<IEnumerable<IViewItem>> Generator()
    //    {
    //        IEnumerable<ActionViewItem> actionHistoryItems =
    //            from action in this.Actions
    //            where this.IncludeActionHistory && (this.ProjectOrGoal == null || action.Parent == this.ProjectOrGoal) && this.Contexts.Contains(action.Context)
    //            from historyItem in action.ActionHistory
    //            where startDate <= historyItem.CompletedDate && historyItem.CompletedDate < endDate
    //            select new ActionViewItem(historyItem, action);

    //        foreach (ActionViewItem item in actionHistoryItems)
    //            yield return item;

    //        IEnumerable<ActionViewItem> activeActionItems = this.Actions
    //            .Where(action => action.IsActive())
    //            .Where(action => this.Contexts.Contains(action.Context))
    //            .Where(action => this.ProjectOrGoal == null || action.Parent == this.ProjectOrGoal)
    //            .Where(action => startDate <= action.ActiveDueDate && action.ActiveDueDate < endDate)
    //            .Select(action => new ActionViewItem(action));

    //        foreach (ActionViewItem item in activeActionItems)
    //            yield return item;

    //        IEnumerable<ActionViewItem> projectedActionItems =
    //            from action in this.Actions
    //            where this.IncludeActionProjection && action.IsActive() && (this.ProjectOrGoal == null || action.Parent == this.ProjectOrGoal) && this.Contexts.Contains(action.Context)
    //            from actionProjectionItem in action.GetProjectedFutureDueDates(this.EndDate)
    //            where startDate <= actionProjectionItem.DueDate && actionProjectionItem.DueDate < endDate
    //            select new ActionViewItem(actionProjectionItem, action);

    //        foreach (ActionViewItem item in projectedActionItems)
    //            yield return item;
    //    }
    //}
}
