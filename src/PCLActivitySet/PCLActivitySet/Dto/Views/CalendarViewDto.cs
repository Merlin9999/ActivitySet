using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Db;
using PCLActivitySet.Domain;

namespace PCLActivitySet.Dto.Views
{
    public class CalendarViewDto : AbstractLiteDbValue
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludeHistory { get; set; }
        public bool IncludeFuture { get; set; }
    }
}
