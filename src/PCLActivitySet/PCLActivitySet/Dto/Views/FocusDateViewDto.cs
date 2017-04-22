using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Db;

namespace PCLActivitySet.Dto.Views
{
    public class FocusDateViewDto : AbstractLiteDbValue
    {
        public TimeSpan? CompletedFilterDelay { get; set; }
        public DateTime FocusDateTime { get; set; }
    }
}
