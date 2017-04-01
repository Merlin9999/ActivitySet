using System;
using System.Diagnostics;
using PCLActivitySet.Dto;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityHistoryItem
    {
        public ActivityHistoryItem()
        {
        }

        public ActivityHistoryItem(string name, DateTime? dueDate, DateTime completionDate, DateTime? timeStamp = null)
        {
            this.Name = name;
            this.DueDate = dueDate;
            this.CompletedDate = completionDate;
            this.TimeStamp = timeStamp ?? DateTime.Now;
        }

        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime TimeStamp { get; set; }

        public void UpdateDto(ActivityHistoryItemDto dto)
        {
            dto.Name = this.Name;
            dto.DueDate = this.DueDate;
            dto.CompletedDate = this.CompletedDate;
            dto.TimeStamp = this.TimeStamp;
        }

        public void UpdateFromDto(ActivityHistoryItemDto dto)
        {
            this.Name = dto.Name;
            this.DueDate = dto.DueDate;
            this.CompletedDate = dto.CompletedDate;
            this.TimeStamp = dto.TimeStamp;
        }

        public static ActivityHistoryItemDto ToDto(ActivityHistoryItem model)
        {
            var retVal = new ActivityHistoryItemDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static ActivityHistoryItem FromDto(ActivityHistoryItemDto dto)
        {
            var retVal = new ActivityHistoryItem();
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}


