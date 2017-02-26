using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class Activity : AbstractEntity<Activity>
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
        
        public Guid? ActivityListGuid { get; set; }

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
            get { return this._completionHistory; }
            set { this._completionHistory = value ?? new List<ActivityHistoryItem>(); }
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
    }
}