using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivitySet : ICollection<Activity>
    {
        private readonly HashSet<Activity> _activitySet = new HashSet<Activity>();

        public string Name { get; set; }

        public int Count => this._activitySet.Count;

        public bool IsReadOnly => false;

        public void Add(Activity activity)
        {
            this._activitySet.Add(activity);
        }

        public void Clear()
        {
            this._activitySet.Clear();
        }

        public bool Contains(Activity item)
        {
            return this._activitySet.Contains(item);
        }

        public void CopyTo(Activity[] array, int arrayIndex)
        {
            this._activitySet.CopyTo(array, arrayIndex);
        }

        public bool Remove(Activity item)
        {
            return this._activitySet.Remove(item);
        }

        public void Add(string name)
        {
            this._activitySet.Add(new Activity() {Name = name});
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
