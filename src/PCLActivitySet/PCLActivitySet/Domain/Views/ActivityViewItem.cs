using System;
using System.Diagnostics;

namespace PCLActivitySet.Domain.Views
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityViewItem : IViewItem
    {
        public ActivityViewItem(Activity activity)
        {
            this.Activity = activity;
        }

        public DateTime? Date => this.Activity.ActiveDueDate;
        public string Name => this.Activity.Name;
        public bool IsActive => this.Activity.IsActive;
        public Activity Activity { get; }
    }
}