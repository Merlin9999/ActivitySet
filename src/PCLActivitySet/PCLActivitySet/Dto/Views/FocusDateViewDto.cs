using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLActivitySet.Dto.Views
{
    public class FocusDateViewDto
    {
        public TimeSpan? CompletedFilterDelay { get; set; }
        public DateTime FocusDateTime { get; set; }
    }
}
