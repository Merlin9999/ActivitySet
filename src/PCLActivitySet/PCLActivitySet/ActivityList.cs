using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class ActivityList : AbstractEntity<ActivityList>
    {
        protected readonly ActivityBoard OwningBoard;
        private ExcludeNonActiveView _excludeNonActiveView;
        private FocusDateView _focusDateView;
        private CalendarView _calendarView;

        public ActivityList(ActivityBoard owningBoard)
        {
            if (owningBoard == null)
                throw new ArgumentNullException(nameof(owningBoard), $"The argument {nameof(owningBoard)} cannot be null.");
            this.OwningBoard = owningBoard;
        }

        public string Name { get; set; }

        public virtual IEnumerable<Activity> Activities => this.OwningBoard.Activities.Where(activity => activity.ActivityListGuid == this.Guid);

        public virtual FluentlySelectViewMode ViewModes => new FluentlySelectViewMode(this);

        public virtual IEnumerable<IViewItem> ViewItems => this.GenerateViewItems();

        internal ExcludeNonActiveView InternalExcludeNonActiveView => this._excludeNonActiveView ?? (this._excludeNonActiveView = new ExcludeNonActiveView());
        internal FocusDateView InternalFocusDateView => this._focusDateView ?? (this._focusDateView = new FocusDateView());
        internal CalendarView InternalCalendarView => this._calendarView ?? (this._calendarView = new CalendarView());

        internal IView InternalActiveView { get; set; }

        protected virtual IEnumerable<IViewItem> GenerateViewItems()
        {
            IView view = this.InternalActiveView;
            return view == null 
                ? this.Activities.Select(activity => new ActivityViewItem(activity)) 
                : view.ViewItemGenerator(this.Activities);
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
