using System;
using System.Diagnostics;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class Activity : AbstractEntity<Activity>
    {
        public string Name { get; set; }
    }
}