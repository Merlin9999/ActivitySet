using System;

namespace PCLActivitySet
{
    public interface IViewItem
    {
        DateTime? Date { get; }
        string Name { get; }
        bool IsActive { get; }
        Activity Activity { get; }
    }
}