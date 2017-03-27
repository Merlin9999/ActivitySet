using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet.Domain.Views
{
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
}