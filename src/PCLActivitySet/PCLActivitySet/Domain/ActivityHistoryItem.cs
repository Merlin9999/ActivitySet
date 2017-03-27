﻿using System;
using System.Diagnostics;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityHistoryItem
    {
        public ActivityHistoryItem()
        {
        }

        public ActivityHistoryItem(string name, DateTime? dueDate, DateTime completionDate, DateTime? timeStamp = null)
        {
            this.Name = name;
            this.DueDate = dueDate;
            this.CompletedDate = completionDate;
            this.TimeStamp = timeStamp ?? DateTime.Now;
        }

        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

