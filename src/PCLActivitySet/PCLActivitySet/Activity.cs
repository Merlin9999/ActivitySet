using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class Activity : AbstractEntity<Activity>
    {
        public string Name { get; set; }
        public DateTime? ActiveDueDate { get; set; }
        public DateProjection LeadTime { get; set; }
        public DateTime? LeadTimeDate =>
            this.LeadTime == null || this.ActiveDueDate == null
                ? (DateTime?) null
                : this.LeadTime.GetPrevious(this.ActiveDueDate.Value);

        public FluentlyModifyActivity Fluently => new FluentlyModifyActivity(this);

        public static FluentlyModifyActivity FluentNew(string name, DateTime? activeDueDate = null)
        {
            return new FluentlyModifyActivity(new Activity() {Name = name, ActiveDueDate = activeDueDate});
        }
    }
}