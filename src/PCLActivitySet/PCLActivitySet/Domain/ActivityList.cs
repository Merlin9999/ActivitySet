using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PCLActivitySet.Domain.Fluent;
using PCLActivitySet.Domain.Views;
using PCLActivitySet.Dto;

namespace PCLActivitySet.Domain
{
    [DebuggerDisplay("{Name} : {GetType().Name}")]
    public class ActivityList : AbstractDomainEntity<ActivityList>
    {
        protected readonly ActivityBoard OwningBoard;
        private ExcludeNonActiveView _excludeNonActiveView;
        private FocusDateView _focusDateView;
        private CalendarView _calendarView;

        public ActivityList(ActivityBoard owningBoard)
        {
            if (owningBoard == null)
                throw new ArgumentNullException(nameof(owningBoard), $"The argument {nameof(owningBoard)} cannot be null.");
            this.OwningBoard = owningBoard;
        }

        protected ActivityList()
        {
            this.OwningBoard = null;
        }

        public virtual string Name { get; set; }

        public virtual IEnumerable<Activity> Activities => this.OwningBoard.Activities.Where(activity => activity.ActivityListGuid == this.Guid);

        public virtual FluentlySelectViewMode ViewModes => new FluentlySelectViewMode(this);

        public virtual IEnumerable<IViewItem> ViewItems => this.GenerateViewItems();

        internal virtual ExcludeNonActiveView InternalExcludeNonActiveView => this._excludeNonActiveView ?? (this._excludeNonActiveView = new ExcludeNonActiveView());
        internal virtual FocusDateView InternalFocusDateView => this._focusDateView ?? (this._focusDateView = new FocusDateView());
        internal virtual CalendarView InternalCalendarView => this._calendarView ?? (this._calendarView = new CalendarView());

        internal virtual IView InternalActiveView { get; set; }

        protected virtual IEnumerable<IViewItem> GenerateViewItems()
        {
            IView view = this.InternalActiveView;
            return view == null 
                ? this.Activities.Select(activity => new ActivityViewItem(activity)) 
                : view.ViewItemGenerator(this.Activities);
        }

        public virtual void UpdateDto(ActivityListDto dto)
        {
            dto.Name = this.Name;

            if (this.InternalCalendarView == null)
                dto.InternalCalendarView = null;
            else
                this.InternalCalendarView.UpdateDto(dto.InternalCalendarView);

            if (this.InternalExcludeNonActiveView == null)
                dto.InternalExcludeNonActiveView = null;
            else
                this.InternalExcludeNonActiveView.UpdateDto(dto.InternalExcludeNonActiveView);

            if (this.InternalFocusDateView == null)
                dto.InternalFocusDateView = null;
            else
                this.InternalFocusDateView.UpdateDto(dto.InternalFocusDateView);

            if (this.InternalActiveView == null)
                dto.SelectedView = null;
            else if (ReferenceEquals(this.InternalActiveView, this.InternalCalendarView))
                dto.SelectedView = EActvitiyListViewType.CalendarViewDto;
            else if (ReferenceEquals(this.InternalActiveView, this.InternalExcludeNonActiveView))
                dto.SelectedView = EActvitiyListViewType.ExcludeNonActiveViewDto;
            else if (ReferenceEquals(this.InternalActiveView, this.InternalFocusDateView))
                dto.SelectedView = EActvitiyListViewType.FocusDateViewDto;
            else
                throw new NotSupportedException($"Unsupported value for {nameof(this.InternalActiveView)}");
        }

        public virtual void UpdateFromDto(ActivityListDto dto)
        {
            this.Name = dto.Name;

            this.InternalCalendarView.UpdateDto(dto.InternalCalendarView);
            this.InternalExcludeNonActiveView.UpdateFromDto(dto.InternalExcludeNonActiveView);
            this.InternalFocusDateView.UpdateFromDto(dto.InternalFocusDateView);

            switch (dto.SelectedView)
            {
                case null:
                    this.InternalActiveView = null;
                    break;
                case EActvitiyListViewType.CalendarViewDto:
                    this.InternalActiveView = this.InternalCalendarView;
                    break;
                case EActvitiyListViewType.ExcludeNonActiveViewDto:
                    this.InternalActiveView = this.InternalExcludeNonActiveView;
                    break;
                case EActvitiyListViewType.FocusDateViewDto:
                    this.InternalActiveView = this.InternalFocusDateView;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported value for {nameof(dto.SelectedView)} ({dto.SelectedView})");
            }
        }

        public static ActivityListDto ToDto(ActivityList model)
        {
            var retVal = new ActivityListDto();
            model.UpdateDto(retVal);
            return retVal;
        }

        public static ActivityList FromDto(ActivityListDto dto, ActivityBoard owningBoard)
        {
            var retVal = owningBoard == null ? new ActivityList() : new ActivityList(owningBoard);
            retVal.UpdateFromDto(dto);
            return retVal;
        }
    }
}

