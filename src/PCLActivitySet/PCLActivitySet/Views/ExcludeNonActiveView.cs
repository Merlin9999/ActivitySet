using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet.Views
{
    public class ExcludeNonActiveView : IView
    {
        public Func<IEnumerable<Activity>, IEnumerable<IViewItem>> ViewItemGenerator
        {
            get
            {
                return activitySequence => activitySequence
                    .Where(activity => activity.IsActive)
                    .Select(activity => new ActivityViewItem(activity));
            }
        }
    }
}