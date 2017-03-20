using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PCLActivitySet.Fluent;

namespace PCLActivitySet
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityBoard
    {
        private HashSet<Activity> ActivitySet { get; } = new HashSet<Activity>();
        private HashSet<ActivityList> _setOfActivityLists;
        private HashSet<ActivityContext> _setOfContexts;
        private HashSet<ActivityGoal> _setOfGoals;
        private HashSet<Guid> _setOfSelectedContextGuids;
        private HashSet<ActivityList> SetOfActivityLists => this._setOfActivityLists ?? (this._setOfActivityLists = new HashSet<ActivityList>());
        private HashSet<ActivityContext> SetOfContexts => this._setOfContexts ?? (this._setOfContexts = new HashSet<ActivityContext>());
        private HashSet<Guid> SetOfSelectedContextGuids => this._setOfSelectedContextGuids ?? (this._setOfSelectedContextGuids = new HashSet<Guid>());
        private HashSet<ActivityGoal> SetOfGoals => this._setOfGoals ?? (this._setOfGoals = new HashSet<ActivityGoal>());


        public ActivityBoard()
        {
            this.InBox = new InBoxActivityList(this) {Name = "InBox"};
        }

        public string Name { get; set; }

        public IEnumerable<Activity> UnfilteredActivities => this.ActivitySet;

        public IEnumerable<Activity> Activities
        {
            get
            {
                if (!this.SetOfSelectedContextGuids.Any())
                    return this.UnfilteredActivities;
                return this.UnfilteredActivities
                    .Where(activity =>
                        !activity.ContextGuids.Any()
                        || this.SetOfSelectedContextGuids.Overlaps(activity.ContextGuids));
            }
}

        public ActivityList InBox { get; }

        public IEnumerable<ActivityList> ActivityLists => this.SetOfActivityLists;

        public IEnumerable<ActivityContext> Contexts => this.SetOfContexts;

        public IEnumerable<Guid> SelectedContextGuids => this.SetOfSelectedContextGuids;

        public IEnumerable<ActivityGoal> Goals => this.SetOfGoals;

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

        public FluentlyMoveActivity MoveActivity(Activity activityToMove)
        {
            return new FluentlyMoveActivity(this, activityToMove);
        }

        public ActivityContext AddNewContext(string contextName)
        {
            var context = new ActivityContext() {Name = contextName};
            this.SetOfContexts.Add(context);
            return context;
        }

        public void RemoveContext(ActivityContext context)
        {
            if (this.SetOfContexts.Contains(context))
            {
                foreach (Activity activity in this.UnfilteredActivities)
                    activity.RemoveContexts(context);
                this.SetOfContexts.Remove(context);
            }
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
            foreach (ActivityContext activityContext in requestedContextsInBoard)
                this.SetOfSelectedContextGuids.Remove(activityContext.Guid);
        }

        public ActivityGoal AddNewGoal(string goalName)
        {
            var goal = new ActivityGoal(goalName);
            this.SetOfGoals.Add(goal);
            return goal;
        }

        public void RemoveGoal(ActivityGoal goal)
        {
            if (this.SetOfGoals.Contains(goal))
            {
                IEnumerable<Activity> affectedActivities = this.UnfilteredActivities
                    .Where(activity => activity.GoalGuid == goal.Guid);
                foreach (Activity activity in affectedActivities)
                    activity.GoalGuid = null;

                this.SetOfGoals.Remove(goal);
            }
        }

        public ActivityGoal GetGoalFromActivity(Activity activity)
        {
            if (activity.GoalGuid == null)
                return null;

            return this.GetGoalLookupFromActivities(activity).FirstOrDefault()?.Key;
        }

        public ILookup<ActivityGoal, Activity> GetGoalLookupFromActivities(params Activity[] activities)
        {
            return this.GetGoalLookupFromActivities(activities.AsEnumerable());
        }

        public ILookup<ActivityGoal, Activity> GetGoalLookupFromActivities(IEnumerable<Activity> activities)
        {
            ILookup<Guid, ActivityGoal> goalLookup = this.SetOfGoals.ToLookup(goal => goal.Guid);

            var activityGoalList = activities
                .Where(activity => activity.GoalGuid != null)
                .Select(
                    activity => new {Activity = activity, Goal = goalLookup[activity.GoalGuid.Value].FirstOrDefault()})
                .ToList();

            if (activityGoalList.Any(x => x.Goal == null))
                throw new InvalidOperationException($"Found an {nameof(Activity.GoalGuid)} that does not belong to an {nameof(ActivityGoal)} owned by the {nameof(ActivityBoard)}.");

            var goalToActivitiesLookup = activityGoalList
                .ToLookup(x => x.Goal, x => x.Activity);

            return goalToActivitiesLookup;
        }
    }
}
