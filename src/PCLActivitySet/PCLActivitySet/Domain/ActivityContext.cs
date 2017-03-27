using System.Diagnostics;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityContext : AbstractEntity
    {
        public string Name { get; set; }
    }
}