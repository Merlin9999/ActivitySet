using System.Diagnostics;
using PCLActivitySet.Dto;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityGoal : AbstractDomainEntity<ActivityGoal>
    {
        private ActivityGoal()
        {
            
        }

        public ActivityGoal(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public void UpdateDto(ActivityGoalDto dto)
        {
            dto.Guid = this.Guid;
            dto.Name = this.Name;
        }

        public void UpdateFromDto(ActivityGoalDto dto)
        {
            this.Guid = dto.Guid;
            this.Name = dto.Name;
        }

        public static ActivityGoalDto ToDto(ActivityGoal model)
        {
            var retVal = new ActivityGoalDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static ActivityGoal FromDto(ActivityGoalDto dto)
        {
            var retVal = new ActivityGoal();
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}