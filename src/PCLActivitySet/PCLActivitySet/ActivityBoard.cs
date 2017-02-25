using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityBoard
    {
        internal HashSet<Activity> ActivitySet { get; } = new HashSet<Activity>();
        private List<ActivityList> _listOfActivityLists;
        private readonly ActivityList _inBox;
        private List<ActivityList> ListOfActivityLists => this._listOfActivityLists ?? (this._listOfActivityLists = new List<ActivityList>());

        public ActivityBoard()
        {
            this._inBox = new InBoxActivityList(this) {Name = "InBox"};
        }

        public string Name { get; set; }

        public IEnumerable<Activity> Activities => this.ActivitySet;

        public ActivityList InBox => this._inBox;

        public IEnumerable<ActivityList> ActivityLists => this.ListOfActivityLists;

        public ActivityList AddNewList(string activityListName)
        {
            var list = new ActivityList(this) {Name = activityListName};
            this.ListOfActivityLists.Add(list);
            return list;
        }

        public FluentMoveActivityToActivityList MoveActivity(Activity activityToMove)
        {
            return new FluentMoveActivityToActivityList(this, activityToMove);
        }

        public void RemoveAllActivities()
        {
            this.ActivitySet.Clear();
        }

        public void RemoveActivity(Activity activity)
        {
            this.ActivitySet.Remove(activity);
        }

        public bool ContainsActivity(Activity activity)
        {
            return this.ActivitySet.Contains(activity);
        }
    }
}
