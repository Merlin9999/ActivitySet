using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityBoard
    {
        private HashSet<Activity> ActivitySet { get; } = new HashSet<Activity>();
        private List<ActivityList> _listOfActivityLists;
        private List<ActivityContext> _listOfActivityContexts;
        private List<ActivityList> ListOfActivityLists => this._listOfActivityLists ?? (this._listOfActivityLists = new List<ActivityList>());
        private List<ActivityContext> ListOfActivityContexts => this._listOfActivityContexts ?? (this._listOfActivityContexts = new List<ActivityContext>());

        public ActivityBoard()
        {
            this.InBox = new InBoxActivityList(this) {Name = "InBox"};
        }

        public string Name { get; set; }

        public void AddActivity(Activity activity)
        {
            this.ActivitySet.Add(activity);
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

        public IEnumerable<Activity> UnfilteredActivities => this.ActivitySet;

        public IEnumerable<Activity> Activities => this.UnfilteredActivities;

        public ActivityList InBox { get; }

        public ActivityList AddNewList(string activityListName)
        {
            var list = new ActivityList(this) {Name = activityListName};
            this.ListOfActivityLists.Add(list);
            return list;
        }

        public void RemoveList(ActivityList list)
        {
            IEnumerable<Activity> activitiesInListToBeRemoved = this.UnfilteredActivities
                .Where(a => a.ActivityListGuid == list.Guid);
            foreach (Activity affectedActivity in activitiesInListToBeRemoved)
                affectedActivity.ActivityListGuid = null;
            this.ListOfActivityLists.Remove(list);
        }

        public IEnumerable<ActivityList> ActivityLists => this.ListOfActivityLists;

        public FluentlyMoveActivityToActivityList MoveActivity(Activity activityToMove)
        {
            return new FluentlyMoveActivityToActivityList(this, activityToMove);
        }

        public ActivityContext AddNewContext(string contextName)
        {
            var context = new ActivityContext() {Name = contextName};
            this.ListOfActivityContexts.Add(context);
            return context;
        }

        public IEnumerable<ActivityContext> ActivityContexts => this.ListOfActivityContexts;

        public void RemoveContext(ActivityContext context)
        {
            //IEnumerable<Activity> activitiesInContextToBeRemoved = this.UnfilteredActivities
            //    .Where(a => a.ActivityListGuid == context.Guid);
            //foreach (Activity affectedActivity in activitiesInContextToBeRemoved)
            //    affectedActivity.ActivityContextGuid = null;

            this.ListOfActivityContexts.Remove(context);
        }
    }
}
