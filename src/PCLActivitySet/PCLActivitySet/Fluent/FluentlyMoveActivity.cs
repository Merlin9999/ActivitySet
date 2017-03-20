using System;
using System.Linq;

namespace PCLActivitySet.Fluent
{
    public class FluentlyMoveActivity
    {
        private readonly ActivityBoard _board;
        private readonly Activity _activityToMove;

        public FluentlyMoveActivity(ActivityBoard board, Activity activityToMove)
        {
            if (!board.ContainsActivity(activityToMove))
                throw new ArgumentException($"Cannot move an {nameof(Activity)} that is not owned by the referenced {nameof(ActivityBoard)}.");
            this._board = board;
            this._activityToMove = activityToMove;
        }

        public FluentlyMoveActivity ToList(ActivityList activityList)
        {
            if (activityList == this._board.InBox || activityList == null)
                this._activityToMove.ActivityListGuid = null;
            else
            {
                if (!this._board.ActivityLists.Contains(activityList))
                    throw new ArgumentException($"Cannot move the {nameof(Activity)} to an {nameof(ActivityList)} that is not owned by the referenced {nameof(ActivityBoard)}.");

                this._activityToMove.ActivityListGuid = activityList.Guid;

            }
            return this;
        }

        public FluentlyMoveActivity ToGoal(ActivityGoal goal)
        {
            if (goal == null)
                this._activityToMove.GoalGuid = null;
            else
            {
                if (!this._board.Goals.Contains(goal))
                    throw new ArgumentException($"Cannot move the {nameof(Activity)} to an {nameof(ActivityGoal)} that is not owned by the referenced {nameof(ActivityBoard)}.");

                this._activityToMove.GoalGuid = goal.Guid;
            }

            return this;
        }
    }
}