using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Domain;

namespace PCLActivitySet.Dto
{
    public class ActivityBoardDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public List<ActivityGoal> Goals { get; set; }
    }
}
