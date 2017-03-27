using System;

namespace PCLActivitySet.Domain
{
    public interface IViewItem
    {
        DateTime? Date { get; }
        string Name { get; }
        bool IsActive { get; }
        Activity Activity { get; }
    }
}