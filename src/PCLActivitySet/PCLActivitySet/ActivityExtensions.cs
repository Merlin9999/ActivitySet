using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet
{
    public static class ActivityExtensions
    {
        public static void SignalCompleted(this Activity activity, DateTime dateCompleted)
        {
            dateCompleted = dateCompleted.Date;

            activity.CompletionHistory.Add(new ActivityHistoryItem(activity.Name, activity.ActiveDueDate, dateCompleted));
            activity.ActiveDueDate = activity.Recurrence != null && activity.ActiveDueDate != null
                ? activity.Recurrence.GetNext(activity.ActiveDueDate.Value, dateCompleted, activity.CompletionHistory.Count)
                : null;
        }

        public static void ResetActiveDueDateFromLastHistoryItem(this Activity activity)
        {
            if (activity.CompletionHistory.Any())
            {
                ActivityHistoryItem historyItem = activity.CompletionHistory.Last();
                activity.ActiveDueDate = historyItem.DueDate == null && activity.Recurrence.RecurFromType == ERecurFromType.FromActiveDueDate
                    ? null
                    : activity.Recurrence?.GetNext(historyItem.DueDate ?? DateTime.MaxValue, historyItem.CompletedDate, activity.CompletionHistory.Count);
            }
        }

    }
}
