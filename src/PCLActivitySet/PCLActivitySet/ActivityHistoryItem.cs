using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLActivitySet
{
    public class ActivityHistoryItem
    {
        public ActivityHistoryItem()
        {
        }

        public ActivityHistoryItem(string name, DateTime? dueDate, DateTime completionDate)
        {
            this.Name = name;
            this.DueDate = dueDate;
            this.CompletedDate = completionDate;
        }

        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}


