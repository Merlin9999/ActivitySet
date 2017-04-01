using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Db;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Dto
{
    public class ActivityDto : AbstractLiteDbEntity
    {
        private List<ActivityHistoryItemDto> _completionHistory;

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public Guid? ActivityListGuid { get; set; }
        public Guid? GoalGuid { get; set; }
        public DateTime? ActiveDueDate { get; set; }
        public DateProjectionDto LeadTime { get; set; }
        public DateRecurrenceDto Recurrence { get; set; }

        public List<ActivityHistoryItemDto> CompletionHistory
        {
            get { return this._completionHistory ?? (this._completionHistory = new List<ActivityHistoryItemDto>()); }
            set { this._completionHistory = value; }
        }
    }
}
