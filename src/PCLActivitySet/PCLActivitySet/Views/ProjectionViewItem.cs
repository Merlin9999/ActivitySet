using System;
using System.Diagnostics;

namespace PCLActivitySet.Views
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
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