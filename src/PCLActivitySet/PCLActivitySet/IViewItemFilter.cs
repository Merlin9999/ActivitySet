using System;
using System.Collections.Generic;
using System.Linq;

namespace PCLActivitySet
{
    public interface IViewItemFilter
    {
        Func<IEnumerable<IViewItem>, IEnumerable<IViewItem>> FilterImpl { get; }
    }

    public class DateRangeFilter : IViewItemFilter
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public DateRangeFilter(DateTime startDate, DateTime endDate)
        {
            this._startDate = startDate.Date;
            this._endDate = endDate.Date;
        }

        public Func<IEnumerable<IViewItem>, IEnumerable<IViewItem>> FilterImpl
        {
            get
            {
                return seq => seq.Where(viewItem => viewItem.Date != null
                                                    && this._startDate <= viewItem.Date
                                                    && viewItem.Date <= this._endDate);
            }
        }
    }
}