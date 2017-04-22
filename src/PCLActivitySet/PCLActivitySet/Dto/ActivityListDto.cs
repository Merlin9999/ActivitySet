using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Db;
using PCLActivitySet.Dto.Views;

namespace PCLActivitySet.Dto
{
    public class ActivityListDto : AbstractLiteDbValue
    {
        private ExcludeNonActiveViewDto _internalExcludeNonActiveView;
        private FocusDateViewDto _internalFocusDateView;
        private CalendarViewDto _internalCalendarView;

        public string Name { get; set; }

        public ExcludeNonActiveViewDto InternalExcludeNonActiveView
        {
            get => this._internalExcludeNonActiveView ?? (this._internalExcludeNonActiveView = new ExcludeNonActiveViewDto());
            set => this._internalExcludeNonActiveView = value;
        }

        public FocusDateViewDto InternalFocusDateView
        {
            get => this._internalFocusDateView ?? (this._internalFocusDateView = new FocusDateViewDto());
            set => this._internalFocusDateView = value;
        }

        public CalendarViewDto InternalCalendarView
        {
            get => this._internalCalendarView ?? (this._internalCalendarView = new CalendarViewDto());
            set => this._internalCalendarView = value;
        }

        public EActvitiyListViewType? SelectedView { get; set; }
    }
}
