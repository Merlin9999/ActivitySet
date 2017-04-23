using System.Diagnostics;
using PCLActivitySet.Dto;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityContext : AbstractDomainEntity<ActivityContext>
    {
        public string Name { get; set; }

        public void UpdateDto(ActivityContextDto dto)
        {
            dto.Guid = this.Guid;
            dto.Name = this.Name;
        }

        public void UpdateFromDto(ActivityContextDto dto)
        {
            this.Guid = dto.Guid;
            this.Name = dto.Name;
        }

        public static ActivityContextDto ToDto(ActivityContext model)
        {
            var retVal = new ActivityContextDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static ActivityContext FromDto(ActivityContextDto dto)
        {
            var retVal = new ActivityContext();
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}