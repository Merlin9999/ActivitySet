using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PCLActivitySet.Dto;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class InBoxActivityList : ActivityList
    {
        public InBoxActivityList()
        {
        }

        public InBoxActivityList(ActivityBoard owningBoard)
            : base(owningBoard)
        {
        }

        public override IEnumerable<Activity> Activities => this.OwningBoard.Activities.Where(
            activity => activity.ActivityListGuid == null);


        public virtual void UpdateDto(InBoxActivityListDto dto)
        {
            base.UpdateDto(dto);
        }

        public virtual void UpdateFromDto(InBoxActivityListDto dto)
        {
            base.UpdateFromDto(dto);
        }

        public static InBoxActivityListDto ToDto(InBoxActivityList model)
        {
            var retVal = new InBoxActivityListDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static InBoxActivityList FromDto(InBoxActivityListDto dto, ActivityBoard owningBoard)
        {
            var retVal = owningBoard == null ? new InBoxActivityList() : new InBoxActivityList(owningBoard);
            retVal.UpdateFromDto(dto);
            return retVal;
        }

    }
}