using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet
{
    public abstract class AbstractFluentlyModifyActivity<TReturn>
        where TReturn : AbstractFluentlyModifyActivity<TReturn>
    {
        protected Activity Activity;

        public Activity ToActivity => this.Activity;

        public static implicit operator Activity(AbstractFluentlyModifyActivity<TReturn> _this)
        {
            return _this.Activity;
        }

        protected AbstractFluentlyModifyActivity(Activity activity)
        {
            this.Activity = activity;
        }

        public FluentlyModifyActivityAndBoard AddToBoard(ActivityBoard activityBoard)
        {
            activityBoard.AddActivity(this.Activity);
            return new FluentlyModifyActivityAndBoard(this, activityBoard);
        }

        public TReturn Name(string newName)
        {
            this.Activity.Name = newName;
            return (TReturn) this;
        }

        public TReturn ActiveDueDate(DateTime newActiveDueDate)
        {
            this.Activity.ActiveDueDate = newActiveDueDate;
            return (TReturn) this;
        }

        public TReturn LeadTime(IDateProjection dateProjection)
        {
            this.Activity.LeadTime = new DateProjection(dateProjection);
            return (TReturn) this;
        }

        public TReturn LeadTime(DateProjection dateProjection)
        {
            this.Activity.LeadTime = dateProjection;
            return (TReturn) this;
        }

        public TReturn DailyLeadTime(int periodCount)
        {
            this.Activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Daily(periodCount));
            return (TReturn) this;
        }

        public TReturn WeeklyLeadTime(int periodCount, EDaysOfWeekFlags daysOfWeek)
        {
            this.Activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Weekly(periodCount, daysOfWeek));
            return (TReturn) this;
        }

        public TReturn MonthlyLeadTime(int periodCount, int dayOfMonth)
        {
            this.Activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Monthly(periodCount, dayOfMonth));
            return (TReturn) this;
        }

        public TReturn MonthlyLeadTime(int periodCount, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this.Activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Monthly(periodCount, weeksInMonth, daysOfWeek));
            return (TReturn) this;
        }

        public TReturn YearlyLeadTime(EMonth month, int dayOfMonth)
        {
            this.Activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Yearly(month, dayOfMonth));
            return (TReturn) this;
        }

        public TReturn YearlyLeadTime(EMonth month, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            this.Activity.LeadTime = new DateProjection(new DateProjectionCreateHelper().Yearly(month, weeksInMonth, daysOfWeek));
            return (TReturn) this;
        }

        public TReturn Recurrence(DateRecurrence dateRecurrence)
        {
            this.Activity.Recurrence = dateRecurrence;
            return (TReturn) this;
        }

        public TReturn Recurrence(ERecurFromType recurFromType, Func<DateProjectionCreateHelper, IDateProjection> dateProjectionFactory)
        {
            IDateProjection dateProjection = dateProjectionFactory(new DateProjectionCreateHelper());
            this.Activity.Recurrence = new DateRecurrence(dateProjection, recurFromType);
            return (TReturn) this;
        }

        public TReturn Recurrence(ERecurFromType recurFromType, int maxRecurrenceCount, Func<DateProjectionCreateHelper, IDateProjection> dateProjectionFactory)
        {
            IDateProjection dateProjection = dateProjectionFactory(new DateProjectionCreateHelper());
            this.Activity.Recurrence = new DateRecurrence(dateProjection, recurFromType, maxRecurrenceCount);
            return (TReturn) this;
        }

        public TReturn Recurrence(ERecurFromType recurFromType, DateTime startDate, DateTime endDate, Func<DateProjectionCreateHelper, IDateProjection> dateProjectionFactory)
        {
            IDateProjection dateProjection = dateProjectionFactory(new DateProjectionCreateHelper());
            this.Activity.Recurrence = new DateRecurrence(dateProjection, recurFromType, startDate, endDate);
            return (TReturn) this;
        }
    }

    public class FluentlyModifyActivity : AbstractFluentlyModifyActivity<FluentlyModifyActivity>
    {
        public FluentlyModifyActivity(Activity activity) 
            : base(activity)
        {
        }
    }

    public class FluentlyModifyActivityAndBoard : AbstractFluentlyModifyActivity<FluentlyModifyActivityAndBoard>
    {
        protected ActivityBoard Board;

        public FluentlyModifyActivityAndBoard(Activity activity, ActivityBoard board) 
            : base(activity)
        {
            this.Board = board;
        }

        public FluentlyModifyActivityAndBoard Contexts(params ActivityContext[] contexts)
        {
            return this.Contexts(contexts.AsEnumerable());
        }

        public FluentlyModifyActivityAndBoard Contexts(IEnumerable<ActivityContext> contexts)
        {
            IEnumerable<ActivityContext> requestedContextsInBoard = contexts.Where(ctx => this.Board.Contexts.Contains(ctx));
            this.Activity.AddContexts(requestedContextsInBoard);
            return this;
        }
    }
}
