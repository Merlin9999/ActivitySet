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
        private HashSet<ActivityList> _setOfActivityLists;
        private HashSet<ActivityContext> _setOfContexts;
        private HashSet<Guid> _setOfSelectedContextGuids;
        private HashSet<ActivityList> SetOfActivityLists => this._setOfActivityLists ?? (this._setOfActivityLists = new HashSet<ActivityList>());
        private HashSet<ActivityContext> SetOfContexts => this._setOfContexts ?? (this._setOfContexts = new HashSet<ActivityContext>());
        private HashSet<Guid> SetOfSelectedContextGuids => this._setOfSelectedContextGuids ?? (this._setOfSelectedContextGuids = new HashSet<Guid>());


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

        public IEnumerable<Activity> UnfilteredActivities
        {
            get
            {
                if (!this.SetOfSelectedContextGuids.Any())
                    return this.ActivitySet;
                return this.ActivitySet
                    .Where(activity => !activity.ContextGuids.Any() || this.SetOfSelectedContextGuids.Overlaps(activity.ContextGuids));
            }
        }

        public IEnumerable<Activity> Activities => this.UnfilteredActivities;

        public ActivityList InBox { get; }


        public ActivityList AddNewList(string activityListName)
        {
            var list = new ActivityList(this) {Name = activityListName};
            this.SetOfActivityLists.Add(list);
            return list;
        }

        public void RemoveList(ActivityList list)
        {
            IEnumerable<Activity> activitiesInListToBeRemoved = this.UnfilteredActivities
                .Where(a => a.ActivityListGuid == list.Guid);
            foreach (Activity affectedActivity in activitiesInListToBeRemoved)
                affectedActivity.ActivityListGuid = null;
            this.SetOfActivityLists.Remove(list);
        }

        public IEnumerable<ActivityList> ActivityLists => this.SetOfActivityLists;


        public FluentlyMoveActivityToActivityList MoveActivity(Activity activityToMove)
        {
            return new FluentlyMoveActivityToActivityList(this, activityToMove);
        }

        public ActivityContext AddNewContext(string contextName)
        {
            var context = new ActivityContext() {Name = contextName};
            this.SetOfContexts.Add(context);
            return context;
        }

        public IEnumerable<ActivityContext> Contexts => this.SetOfContexts;

        public IEnumerable<Guid> SelectedContextGuids => this.SetOfSelectedContextGuids;

        public void RemoveContext(ActivityContext context)
        {
            //IEnumerable<Activity> activitiesInContextToBeRemoved = this.UnfilteredActivities
            //    .Where(a => a.ActivityListGuid == context.Guid);
            //foreach (Activity affectedActivity in activitiesInContextToBeRemoved)
            //    affectedActivity.ActivityContextGuid = null;

            this.SetOfContexts.Remove(context);
        }

        public void AddSelectedContexts(params ActivityContext[] contexts)
        {
            this.AddSelectedContexts(contexts.AsEnumerable());
        }

        public void AddSelectedContexts(IEnumerable<ActivityContext> contexts)
        {
            IEnumerable<ActivityContext> requestedContextsInBoard = contexts.Where(ctx => this.SetOfContexts.Contains(ctx));
            foreach (ActivityContext activityContext in requestedContextsInBoard)
                this.SetOfSelectedContextGuids.Add(activityContext.Guid);
        }

        public void RemoveSelectedContexts(params ActivityContext[] contexts)
        {
            this.RemoveSelectedContexts(contexts.AsEnumerable());
        }

        public void RemoveSelectedContexts(IEnumerable<ActivityContext> contexts)
        {
            List<ActivityContext> requestedContextsInBoard = contexts.Where(ctx => this.SetOfContexts.Contains(ctx)).ToList();
            foreach (Activity activity in this.ActivitySet)
                activity.RemoveContexts(requestedContextsInBoard);
            foreach (ActivityContext activityContext in requestedContextsInBoard)
                this.SetOfSelectedContextGuids.Remove(activityContext.Guid);
        }
    }
}
