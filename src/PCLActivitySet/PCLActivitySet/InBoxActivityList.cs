using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public class InBoxActivityList : ActivityList
    {
        public InBoxActivityList(ActivityBoard owningBoard) 
            : base(owningBoard)
        {
        }

        public override IEnumerable<Activity> Activities => this.OwningBoard.Activities.Where(activity => activity.ActivityListGuid == null);
    }
}