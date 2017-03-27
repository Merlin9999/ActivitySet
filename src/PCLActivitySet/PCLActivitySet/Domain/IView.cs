using System;
using System.Collections.Generic;

namespace PCLActivitySet.Domain
{
    public interface IView
    {
        Func<IEnumerable<Activity>, IEnumerable<IViewItem>> ViewItemGenerator { get; }
    }
}