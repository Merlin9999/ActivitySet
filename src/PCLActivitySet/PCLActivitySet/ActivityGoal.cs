using System.Diagnostics;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityGoal : AbstractEntity<ActivityGoal>
    {
        public ActivityGoal(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}