using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class Activity : AbstractEntity<Activity>
    {
        public Activity()
        {
            this.CompletionHistory = new List<ActivityHistoryItem>();
        }

        public string Name { get; set; }
        public DateTime? ActiveDueDate { get; set; }
        public DateProjection LeadTime { get; set; }

        public DateTime? LeadTimeDate =>
            this.LeadTime == null || this.ActiveDueDate == null
                ? (DateTime?) null
                : this.LeadTime.GetPrevious(this.ActiveDueDate.Value);

        //public DateRecurrence Recurrence  { get; set; }

        public FluentlyModifyActivity Fluently => new FluentlyModifyActivity(this);

        public List<ActivityHistoryItem> CompletionHistory { get; set; }

        public static FluentlyModifyActivity FluentNew(string name, DateTime? activeDueDate = null)
        {
            return new FluentlyModifyActivity(new Activity() {Name = name, ActiveDueDate = activeDueDate});
        }
    }
}