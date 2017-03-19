using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public interface IView
    {
        Func<IEnumerable<Activity>, IEnumerable<IViewItem>> ViewItemGenerator { get; }
    }
}