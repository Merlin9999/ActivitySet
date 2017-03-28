using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLActivitySet.Dto.Recurrence
{
    public class DateRecurrenceDto : DateProjectionDto
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public ERecurFromType RecurFromType { get; set; }
        public int MaxRecurrenceCount { get; set; }

        public DateTime StartDate
        {
            get { return this._startDate; }
            set { this._startDate = value.Date; }
        }

        public DateTime EndDate
        {
            get { return this._endDate; }
            set { this._endDate = value.Date; }
        }
    }
}
