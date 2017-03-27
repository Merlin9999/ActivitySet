using System;
using System.Collections.Generic;
using System.Linq;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Domain
{
    public static class ActivityExtensions
    {
        public static void SignalCompleted(this Activity activity, DateTime dateCompleted)
        {
            dateCompleted = dateCompleted.Date;

            activity.CompletionHistory.Add(new ActivityHistoryItem(activity.Name, activity.ActiveDueDate, dateCompleted));
            activity.ActiveDueDate = activity.Recurrence != null && 
                (activity.ActiveDueDate != null || activity.Recurrence.RecurFromType == ERecurFromType.FromCompletedDate)
                ? activity.Recurrence.GetNext(activity.ActiveDueDate ?? DateTime.Today, dateCompleted, activity.CompletionHistory.Count)
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

        public static IEnumerable<ActivityProjectionItem> GetProjectedFutureDueDates(this Activity activity, DateTime endDate)
        {
            if (activity.Recurrence != null && activity.ActiveDueDate != null)
            {
                endDate = endDate.AddDays(1).Date;
                DateTime? dt = activity.Recurrence.GetNext(activity.ActiveDueDate.Value, activity.CompletionHistory.Count);
                int projectedDateCount = 1;

                while (dt.HasValue && dt.Value < endDate)
                {
                    yield return new ActivityProjectionItem(activity.Name, dt.Value);
                    dt = activity.Recurrence.GetNext(dt.Value, activity.CompletionHistory.Count + projectedDateCount++);
                }
            }
        }
    }
}
