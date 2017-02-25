using System;
using System.Linq;

namespace PCLActivitySet
{
    public class FluentMoveActivity
    {
        private readonly ActivityBoard _board;
        private readonly Activity _activityToMove;

        public FluentMoveActivity(ActivityBoard board, Activity activityToMove)
        {
            if (!board.Contains(activityToMove))
                throw new ArgumentException($"Cannot move an {nameof(Activity)} that is not owned by the referenced {nameof(ActivityBoard)}.");
            this._board = board;
            this._activityToMove = activityToMove;
        }

        public void To(ActivityList activityList)
        {
            if (!this._board.ActivityLists.Contains(activityList))
                throw new ArgumentException($"Cannot move the {nameof(Activity)} to an {nameof(ActivityList)} that is not owned by the referenced {nameof(ActivityBoard)}.");
            this._activityToMove.ActivityListGuid = activityList.Guid;
        }
    }
}