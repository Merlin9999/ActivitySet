using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PCLActivitySet.Domain.Fluent;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class Activity : AbstractDomainEntity<Activity>
    {
        private DateTime? _activeDueDate;
        private List<ActivityHistoryItem> _completionHistory;
        private readonly HashSet<Guid> _setOfContextGuids;

        public Activity()
        {
            this.CompletionHistory = new List<ActivityHistoryItem>();
            this._setOfContextGuids = new HashSet<Guid>();
        }

        public string Name { get; set; }
        
        internal Guid? ActivityListGuid { get; set; }

        internal Guid? GoalGuid { get; set; }

        public DateTime? ActiveDueDate
        {
            get { return this._activeDueDate; }
            set
            {
                if (value != null)
                    this.HandleRewind(value.Value);
                this._activeDueDate = value;
            }
        }

        public bool IsActive => this.ActiveDueDate != null || !this.CompletionHistory.Any();

        public DateProjection LeadTime { get; set; }

        public DateTime? LeadTimeDate =>
            this.LeadTime == null || this.ActiveDueDate == null
                ? (DateTime?) null
                : this.LeadTime.GetPrevious(this.ActiveDueDate.Value);

        public DateRecurrence Recurrence  { get; set; }
        
        public List<ActivityHistoryItem> CompletionHistory
        {
            get { return this._completionHistory ?? (this._completionHistory = new List<ActivityHistoryItem>()); }
            set { this._completionHistory = value; }
        }

        public FluentlyModifyActivity Fluently => new FluentlyModifyActivity(this);

        public IEnumerable<Guid> ContextGuids => this._setOfContextGuids;

        public DateTime? LastCompletedDate => this.CompletionHistory?.LastOrDefault()?.CompletedDate;

        public void AddContexts(params ActivityContext[] contexts)
        {
            this.AddContexts(contexts.AsEnumerable());
        }

        public void AddContexts(IEnumerable<ActivityContext> contexts)
        {
            foreach (Guid contextGuid in contexts.Select(ctx => ctx.Guid))
                this._setOfContextGuids.Add(contextGuid);
        }

        public void RemoveContexts(params ActivityContext[] contexts)
        {
            this.RemoveContexts(contexts.AsEnumerable());
        }

        public void RemoveContexts(IEnumerable<ActivityContext> contexts)
        {
            foreach (Guid contextGuid in contexts.Select(ctx => ctx.Guid))
                this._setOfContextGuids.Remove(contextGuid);
        }

        public static FluentlyModifyActivity FluentNew(string name)
        {
            return new FluentlyModifyActivity(new Activity() {Name = name});
        }

        public void UpdateDto(ActivityDto dto)
        {
            dto.Guid = this.Guid;
            dto.Name = this.Name;
            dto.ActivityListGuid = this.ActivityListGuid;
            dto.GoalGuid = this.GoalGuid;
            dto.ActiveDueDate = this.ActiveDueDate;

            if (this.LeadTime == null)
                dto.LeadTime = null;
            else
            {
                dto.LeadTime = new DateProjectionDto();
                this.LeadTime.UpdateDto(dto.LeadTime);
            }

            if (this.Recurrence == null)
                dto.Recurrence = null;
            else
            {
                dto.Recurrence = new DateRecurrenceDto();
                this.Recurrence.UpdateDto(dto.Recurrence);
            }

            dto.CompletionHistory = new List<ActivityHistoryItemDto>();
            foreach (ActivityHistoryItem historyItem in this.CompletionHistory)
                dto.CompletionHistory.Add(ActivityHistoryItem.ToDto(historyItem));
        }

        public void UpdateFromDto(ActivityDto dto)
        {
            this.Guid = dto.Guid;
            this.Name = dto.Name;
            this.ActivityListGuid = dto.ActivityListGuid;
            this.GoalGuid = dto.GoalGuid;
            this.ActiveDueDate = dto.ActiveDueDate;

            if (dto.LeadTime == null)
                this.LeadTime = null;
            else
            {
                this.LeadTime = new DateProjection();
                this.LeadTime.UpdateFromDto(dto.LeadTime);
            }

            if (dto.Recurrence == null)
                this.Recurrence = null;
            else
            {
                this.Recurrence = new DateRecurrence();
                this.Recurrence.UpdateFromDto(dto.Recurrence);
            }

            this.CompletionHistory = new List<ActivityHistoryItem>();
            foreach (ActivityHistoryItemDto historyItemDto in dto.CompletionHistory)
                this.CompletionHistory.Add(ActivityHistoryItem.FromDto(historyItemDto));
        }

        public static ActivityDto ToDto(Activity model)
        {
            var retVal = new ActivityDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static Activity FromDto(ActivityDto dto)
        {
            var retVal = new Activity();
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}