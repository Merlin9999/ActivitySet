using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PCLActivitySet.Fluent;
using PCLActivitySet.Views;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
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
}
