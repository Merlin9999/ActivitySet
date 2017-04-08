using System;
using System.Collections.Generic;
using System.Linq;
using PCLActivitySet.Dto.Views;

namespace PCLActivitySet.Domain.Views
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
                    .SelectMany(
                        activity => activity.CompletionHistory,
                        (activity, historyItem) => new {Activity = activity, HistoryItem = historyItem})
                    .Where(x =>
                        this.StartDate <= x.HistoryItem.CompletedDate
                        && x.HistoryItem.CompletedDate < this.EndDate)
                    .Select(x => new HistoryViewItem(x.HistoryItem, x.Activity));
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
                    .SelectMany(
                        activity => activity.GetProjectedFutureDueDates(this.EndDate.Value),
                        (activity, projectionItem) => new {Activity = activity, ProjectionItem = projectionItem})
                    .Where(x =>
                        this.StartDate <= x.ProjectionItem.DueDate
                        && x.ProjectionItem.DueDate < this.EndDate)
                    .Select(x => new ProjectionViewItem(x.ProjectionItem, x.Activity));
                foreach (ProjectionViewItem item in futureItems)
                    yield return item;
            }
        }

        public void UpdateDto(CalendarViewDto dto)
        {
            dto.StartDate = this.StartDate;
            dto.EndDate = this.EndDate;
            dto.IncludeHistory = this.IncludeHistory;
            dto.IncludeFuture = this.IncludeFuture;
        }

        public void UpdateFromDto(CalendarViewDto dto)
        {
            this.StartDate = dto.StartDate;
            this.EndDate = dto.EndDate;
            this.IncludeHistory = dto.IncludeHistory;
            this.IncludeFuture = dto.IncludeFuture;
        }

        public static CalendarViewDto ToDto(CalendarView model)
        {
            var retVal = new CalendarViewDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static CalendarView FromDto(CalendarViewDto dto)
        {
            var retVal = new CalendarView();
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}