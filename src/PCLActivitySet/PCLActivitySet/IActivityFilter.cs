using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public interface IActivityFilter
    {
        Func<IEnumerable<Activity>, IEnumerable<Activity>> FilterImpl { get; }
    }

    public class ExcludeNonActiveWithDelayFilter : IActivityFilter
    {
        private readonly ActivityList _activityList;

        public ExcludeNonActiveWithDelayFilter(ActivityList activityList, TimeSpan? delay)
        {
            this._activityList = activityList;
            this.Delay = delay;
        }

        public TimeSpan? Delay { get; }
        public Func<IEnumerable<Activity>, IEnumerable<Activity>> FilterImpl
        {
            get
            {
                return seq => seq.Where(activity =>
                {
                    DateTime? lastCompletedTimeStamp = activity.CompletionHistory?.LastOrDefault()?.TimeStamp;
                    bool activityIsActive = activity.IsActive;

                    if (this.Delay == null)
                        return activityIsActive;
                        
                    return activityIsActive || this.DelayHasNotExpired(lastCompletedTimeStamp);
                });
            }
        }

       private bool DelayHasNotExpired(DateTime? lastCompletedTimeStamp)
        {
            return (lastCompletedTimeStamp != null &&
                    lastCompletedTimeStamp.Value + this.Delay.Value > this._activityList.FocusDateTime);
        }
    }

    public class FocusDateRadarFilter : IActivityFilter
    {
        private readonly ActivityList _activityList;

        public FocusDateRadarFilter(ActivityList activityList)
        {
            this._activityList = activityList;
        }

        public Func<IEnumerable<Activity>, IEnumerable<Activity>> FilterImpl
        {
            get
            {
                DateTime focusDatePlus1 = this._activityList.FocusDate.AddDays(1).Date;

                return seq => seq.Where(activity =>
                {
                    if (activity.ActiveDueDate == null || activity.ActiveDueDate.Value < focusDatePlus1)
                        return true;
                    var leadTimeDate = activity.LeadTimeDate;
                    if (leadTimeDate != null && leadTimeDate < focusDatePlus1)
                        return true;

                    return false;
                });
            }
        }
    }
}