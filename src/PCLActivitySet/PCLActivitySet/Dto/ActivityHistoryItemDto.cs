using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Db;

namespace PCLActivitySet.Dto
{
    public class ActivityHistoryItemDto : AbstractLiteDbValue
    {
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
