using System;

namespace PCLActivitySet
{
    public interface IViewItem
    {
        DateTime? Date { get; }
        string Name { get; }
        bool IsActive { get; }
    }

    public class ActivityViewItem : IViewItem
    {
        private readonly Activity _activity;

        public ActivityViewItem(Activity activity)
        {
            this._activity = activity;
        }

        public DateTime? Date => this._activity.ActiveDueDate;
        public string Name => this._activity.Name;
        public bool IsActive => this._activity.IsActive;
    }
}