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

    public class HistoryViewItem : IViewItem
    {
        private readonly ActivityHistoryItem _historyItem;

        public HistoryViewItem(ActivityHistoryItem historyItem)
        {
            this._historyItem = historyItem;
        }

        public string Name => this._historyItem.Name;
        public DateTime? Date => this._historyItem.CompletedDate;
        public bool IsActive => false;
    }

    public class ProjectionViewItem : IViewItem
    {
        private readonly ActivityProjectionItem _projectionItem;

        public ProjectionViewItem(ActivityProjectionItem projectionItem)
        {
            this._projectionItem = projectionItem;
        }

        public string Name => this._projectionItem.Name;
        public DateTime? Date => this._projectionItem.DueDate;
        public bool IsActive => false;
    }
}