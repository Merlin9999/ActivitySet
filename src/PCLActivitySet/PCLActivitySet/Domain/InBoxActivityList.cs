using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class InBoxActivityList : ActivityList
    {
        public InBoxActivityList(ActivityBoard owningBoard)
            : base(owningBoard)
        {
        }

        public override IEnumerable<Activity> Activities => this.OwningBoard.Activities.Where(activity => activity.ActivityListGuid == null);
    }
}