using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLActivitySet
{
    internal static class ActivityInternalExtensions
    {
        internal static void HandleRewind(this Activity activity, DateTime newActiveDueDate)
        {
            activity.CompletionHistory?.RemoveAll(item => item.DueDate >= newActiveDueDate || item.CompletedDate >= newActiveDueDate);
        }
    }
}
