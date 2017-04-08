using System;
using System.Collections.Generic;
using System.Linq;
using PCLActivitySet.Dto.Views;

namespace PCLActivitySet.Domain.Views
{
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

        public void UpdateDto(ExcludeNonActiveViewDto dto)
        {
        }

        public void UpdateFromDto(ExcludeNonActiveViewDto dto)
        {
        }

        public static ExcludeNonActiveViewDto ToDto(ExcludeNonActiveView model)
        {
            var retVal = new ExcludeNonActiveViewDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static ExcludeNonActiveView FromDto(ExcludeNonActiveViewDto dto)
        {
            var retVal = new ExcludeNonActiveView();
            retVal.UpdateFromDto(dto);
            return retVal;
        }

    }
}