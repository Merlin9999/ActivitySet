using System;

namespace PCLActivitySet
{
    public class ActivityProjectionItem
    {
        public ActivityProjectionItem()
        {
        }

        public ActivityProjectionItem(string name, DateTime dueDate)
        {
            this.Name = name;
            this.DueDate = dueDate;
        }

        public string Name { get; set; }
        public DateTime DueDate { get; set; }
    }

}
