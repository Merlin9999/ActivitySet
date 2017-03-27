using System;
using System.Diagnostics;

namespace PCLActivitySet.Domain.Views
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ProjectionViewItem : IViewItem
    {
        private readonly ActivityProjectionItem _projectionItem;

        public ProjectionViewItem(ActivityProjectionItem projectionItem, Activity activity)
        {
            this._projectionItem = projectionItem;
            this.Activity = activity;
        }

        public string Name => this._projectionItem.Name;
        public DateTime? Date => this._projectionItem.DueDate;
        public bool IsActive => false;
        public Activity Activity { get; }
    }
}