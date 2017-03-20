using System;
using System.Diagnostics;

namespace PCLActivitySet.Views
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class HistoryViewItem : IViewItem
    {
        private readonly ActivityHistoryItem _historyItem;

        public HistoryViewItem(ActivityHistoryItem historyItem, Activity activity)
        {
            this._historyItem = historyItem;
            this.Activity = activity;
        }

        public string Name => this._historyItem.Name;
        public DateTime? Date => this._historyItem.CompletedDate;
        public bool IsActive => false;
        public Activity Activity { get; }
    }
}