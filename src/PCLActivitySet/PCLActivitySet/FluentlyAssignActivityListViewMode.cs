using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class FluentlySelectViewMode
    {
        private readonly ActivityList _activityList;

        public FluentlySelectViewMode(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupExcludeNonActiveView ExcludeNonActiveView => new FluentlySetupExcludeNonActiveView(this._activityList);
        public FluentlySetupFocusDateView FocusDateView => new FluentlySetupFocusDateView(this._activityList);
        public FluentlySetupCalendarView CalendarView => new FluentlySetupCalendarView(this._activityList);

        public FluentlySelectViewMode Clear()
        {
            this._activityList.InternalActiveView = null;
            return this;
        }
    }

    public class FluentlySetupCalendarView
    {
        private readonly ActivityList _activityList;

        public FluentlySetupCalendarView(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupCalendarView DateRange(DateTime startDate, DateTime endDate)
        {
            CalendarView view = this._activityList.InternalCalendarView;
            view.StartDate = startDate;
            view.EndDate = endDate;
            return this;
        }

        public FluentlySetupCalendarView Enable()
        {
            this._activityList.InternalActiveView = this.Get();
            return this;
        }

        private CalendarView Get() => this._activityList.InternalCalendarView;
    }

    public class CalendarView : IView
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Func<IEnumerable<Activity>, IEnumerable<IViewItem>> ViewItemGenerator => this.Generator;

        private IEnumerable<IViewItem> Generator(IEnumerable<Activity> activitySequence)
        {
            if (this.StartDate == null || this.EndDate == null)
                throw new InvalidOperationException(
                    $"The properties {nameof(this.StartDate)} and {nameof(this.EndDate)} must be assigned values.");

            if (this.EndDate < this.StartDate)
                throw new InvalidOperationException(
                    $"{nameof(this.StartDate)} must be less than or equal to {nameof(this.EndDate)}.");

            return activitySequence
                .Select(activity => new ActivityViewItem(activity))
                .Where(viewItem => viewItem.Date != null
                                   && this.StartDate <= viewItem.Date
                                   && viewItem.Date <= this.EndDate);
        }
    }

    public class FluentlySetupFocusDateView
    {
        private readonly ActivityList _activityList;

        public FluentlySetupFocusDateView(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupFocusDateView FocusDateTime(DateTime focusDateTime)
        {
            this._activityList.InternalFocusDateView.FocusDateTime = focusDateTime;
            return this;
        }

        public FluentlySetupFocusDateView CompletedFilterDelay(TimeSpan completedFilterDelay)
        {
            this.Get().CompletedFilterDelay = completedFilterDelay;
            return this;
        }

        public FluentlySetupFocusDateView Enable()
        {
            this._activityList.InternalActiveView = this.Get();
            return this;
        }

        public FocusDateView Get() => this._activityList.InternalFocusDateView;
    }

    public class FocusDateView : IView
    {
        private DateTime? _focusDate;

        public TimeSpan? CompletedFilterDelay { get; set; }

        public DateTime FocusDate => this.FocusDateTime.Date;

        public DateTime FocusDateTime
        {
            get { return this._focusDate ?? DateTime.Now; }
            set { this._focusDate = value; }
        }

        public void ResetFocusDateTimeToNow()
        {
            this._focusDate = null;
        }

        public Func<IEnumerable<Activity>, IEnumerable<IViewItem>> ViewItemGenerator
        {
            get
            {
                DateTime focusDatePlus1 = this.FocusDate.AddDays(1).Date;

                return activitySequence => activitySequence

                    .Where(activity =>
                    {
                        if (activity.ActiveDueDate == null || activity.ActiveDueDate.Value < focusDatePlus1)
                            return true;
                        var leadTimeDate = activity.LeadTimeDate;
                        if (leadTimeDate != null && leadTimeDate < focusDatePlus1)
                            return true;

                        return false;
                    })

                    .Where(activity =>
                    {
                        DateTime? lastCompletedTimeStamp = activity.CompletionHistory?.LastOrDefault()?.TimeStamp;
                        bool activityIsActive = activity.IsActive;

                        if (this.CompletedFilterDelay == null)
                            return activityIsActive;

                        return activityIsActive || this.DelayHasNotExpired(lastCompletedTimeStamp);
                    })

                    .Select(activity => new ActivityViewItem(activity));
            }
        }

        private bool DelayHasNotExpired(DateTime? lastCompletedTimeStamp)
        {
            return (lastCompletedTimeStamp != null &&
                    lastCompletedTimeStamp.Value + this.CompletedFilterDelay.Value > this.FocusDateTime);
        }
    }

    public class FluentlySetupExcludeNonActiveView
    {
        private readonly ActivityList _activityList;

        public FluentlySetupExcludeNonActiveView(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public FluentlySetupExcludeNonActiveView Enable()
        {
            this._activityList.InternalActiveView = this.Get();
            return this;
        }

        public ExcludeNonActiveView Get() => this._activityList.InternalExcludeNonActiveView;
    }

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

    //public class FluentlyAssignActivityListViewMode
    //{
    //    private readonly ActivityList _activityList;

    //    public FluentlyAssignActivityListViewMode(ActivityList activityList)
    //    {
    //        this._activityList = activityList;
    //    }

        //public TFilter GetFilter<TFilter>()
        //    where TFilter : IViewItemFilter
        //{
        //    return this._activityList.InternalViewItemFilterList.OfType<TFilter>().FirstOrDefault();
        //}

        //public void SetFilter<TFilter>(TFilter filterToAdd)
        //    where TFilter : IViewItemFilter
        //{
        //    this.RemoveFilter<TFilter>();
        //    this._activityList.InternalViewItemFilterList.Add(filterToAdd);
        //}

        //public void RemoveFilter<TFilter>() 
        //    where TFilter : IViewItemFilter
        //{
        //    List<TFilter> oldFiltersToRemove = this._activityList.InternalActivityFilterList.OfType<TFilter>().ToList();
        //    foreach (TFilter filterToRemove in oldFiltersToRemove)
        //        this._activityList.InternalViewItemFilterList.Remove(filterToRemove);
        //}

        //public FluentlyAssignActivityListViewMode Clear()
        //{
        //    this._activityList.InternalViewItemFilterList.Clear();
        //    return this;
        //}

        //public FluentlyAssignActivityListViewMode DateRangeZ(DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();

        //    //this.SetFilter(new DateRangeFilter(startDate, endDate));
        //    //return this;
        //}

        //public FluentlyAssignActivityListViewMode IncludeHistoryZ()
        //{
        //    throw new NotImplementedException();
        //}
    //}
}