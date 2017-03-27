using System;

namespace PCLActivitySet.Domain
{
    internal static class ActivityInternalExtensions
    {
        internal static void HandleRewind(this Activity activity, DateTime newActiveDueDate)
        {
            activity.CompletionHistory?.RemoveAll(item => item.DueDate >= newActiveDueDate || item.CompletedDate >= newActiveDueDate);
        }
    }
}
