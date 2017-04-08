using System;
using System.Collections.Generic;
using System.Linq;
using PCLActivitySet.Dto.Views;

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

        public void UpdateDto(FocusDateViewDto dto)
        {
            dto.CompletedFilterDelay = this.CompletedFilterDelay;
            dto.FocusDateTime = this.FocusDateTime;
        }

        public void UpdateFromDto(FocusDateViewDto dto)
        {
            this.CompletedFilterDelay = dto.CompletedFilterDelay;
            this.FocusDateTime = dto.FocusDateTime;
        }

        public static FocusDateViewDto ToDto(FocusDateView model)
        {
            var retVal = new FocusDateViewDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static FocusDateView FromDto(FocusDateViewDto dto)
        {
            var retVal = new FocusDateView();
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}