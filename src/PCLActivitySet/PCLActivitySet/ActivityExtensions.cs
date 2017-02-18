using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLActivitySet
{
    public static class ActivityExtensions
    {
        public static void SignalCompleted(this Activity _this, DateTime dateCompleted)
        {
            dateCompleted = dateCompleted.Date;
            
            if (_this.CompletionHistory == null)
                _this.CompletionHistory = new List<ActivityHistoryItem>();

            _this.CompletionHistory.Add(new ActivityHistoryItem(_this.Name, _this.ActiveDueDate, dateCompleted));
            _this.ActiveDueDate = null;

            //DateTime? dt = _this.Recurrence != null
            //    ? _this.Recurrence.GetNext(action.ActiveDueDate, completedDate, action.ActionHistory.Count)
            //    : null;

            //_this.ActiveDueDate = dt ?? DateTime.MaxValue;
            //_this.ToDoDateInternalState = null;

        }
    }
}
