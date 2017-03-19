using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet.Views
{
    public class CalendarView : IView
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludeHistory { get; set; }
        public bool IncludeFuture { get; set; }

        public Func<IEnumerable<Activity>, IEnumerable<IViewItem>> ViewItemGenerator => this.Generator;

        private IEnumerable<IViewItem> Generator(IEnumerable<Activity> activitySequence)
        {
            if (this.StartDate == null || this.EndDate == null)
                throw new InvalidOperationException(
                    $"The properties {nameof(this.StartDate)} and {nameof(this.EndDate)} must be assigned values.");

            if (this.EndDate <= this.StartDate)
                throw new InvalidOperationException(
                    $"{nameof(this.StartDate)} must be less than {nameof(this.EndDate)}.");
           
            List<Activity> activityList = activitySequence.ToList();

            if (this.IncludeHistory)
            {
                IEnumerable<HistoryViewItem> historyItems = activityList
                    .SelectMany(activity => activity.CompletionHistory)
                    .Where(historyItem =>
                        this.StartDate <= historyItem.CompletedDate
                        && historyItem.CompletedDate < this.EndDate)
                    .Select(historyItem => new HistoryViewItem(historyItem));
                foreach (HistoryViewItem item in historyItems)
                    yield return item;
            }

            IEnumerable<ActivityViewItem> activityItems = activityList
                .Select(activity => new ActivityViewItem(activity))
                .Where(viewItem =>
                    viewItem.Date != null
                    && this.StartDate <= viewItem.Date
                    && viewItem.Date < this.EndDate);
            foreach (ActivityViewItem item in activityItems)
                yield return item;

            if (this.IncludeFuture)
            {
                IEnumerable<ProjectionViewItem> futureItems = activityList
                    .SelectMany(activity => activity.GetProjectedFutureDueDates(this.EndDate.Value))
                    .Where(futureItem =>
                        this.StartDate <= futureItem.DueDate
                        && futureItem.DueDate < this.EndDate)
                    .Select(futureItem => new ProjectionViewItem(futureItem));
                foreach (ProjectionViewItem item in futureItems)
                    yield return item;
            }
        }
    }
}