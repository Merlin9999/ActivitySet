using System.Collections;
using System.Collections.Generic;

namespace PCLActivitySet
{
    public class ActivitySet : IEnumerable<Activity>
    {
        private HashSet<Activity> _activitySet = new HashSet<Activity>();

        public string Name { get; set; }

        public void Add(Activity activity)
        {
            this._activitySet.Add(activity);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<Activity> GetEnumerator()
        {
            return this._activitySet.GetEnumerator();
        }
    }
}