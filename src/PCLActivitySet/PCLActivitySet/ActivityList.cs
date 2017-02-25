using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class ActivityList : AbstractEntity<ActivityList>
    {
        protected readonly ActivityBoard _owningBoard;

        public ActivityList(ActivityBoard owningBoard)
        {
            if (owningBoard == null)
                throw new ArgumentNullException(nameof(owningBoard), $"The argument {nameof(owningBoard)} cannot be null.");
            this._owningBoard = owningBoard;
        }

        public string Name { get; set; }

        public virtual IEnumerable<Activity> Activities => this._owningBoard.Activities.Where(activity => activity.ActivityListGuid == this.Guid);
    }
}