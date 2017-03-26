using System;
//using System.Runtime.Serialization;
//using System.Xml.Serialization;

namespace PCLActivitySet.Recurrence
{
    public class DateProjection
    {
        public DateProjection()
        {
            this.DateProjectionImpl = new DailyProjection();
        }

        public DateProjection(EDateProjectionType recurType)
        {
            this.DateProjectionImpl = CreateDateProjectionImpl(recurType);
        }

        public DateProjection(IDateProjection recurObj)
        {
            this.DateProjectionImpl = recurObj;
        }

        public DateTime GetNext(DateTime fromDate)
        {
            return this.DateProjectionImpl.GetNext(fromDate);
        }

        public DateTime GetPrevious(DateTime fromDate)
        {
            return this.DateProjectionImpl.GetPrevious(fromDate);
        }
        
        //[XmlIgnore]
        //[IgnoreDataMember]
        //[ScriptIgnore]
        //[JsonIgnore]
        public IDateProjection DateProjectionImpl { get; set; }

        public static string ToShortDescription(DateProjection proj)
        {
            if (proj == null)
                return "None";

            switch (proj.ProjectionType)
            {
            	case EDateProjectionType.MonthlyRelative:
                    return "Monthly Relative";

                case EDateProjectionType.YearlyRelative:
                    return "Yearly Relative";

                default:
                    return proj.ProjectionType.ToString();
            }
        }

        public EDateProjectionType ProjectionType
        {
            get { return this.DateProjectionImpl.GetTranslator().ProjectionType; }
            set
            {
                if (this.DateProjectionImpl.ProjectionType != value)
                    this.DateProjectionImpl = CreateDateProjectionImpl(value, this.DateProjectionImpl);
            }
        }

        // DailyRecurrence           => DayCount
        // WeeklyRecurrence          => WeekCount
        // MonthlyRecurrence         => MonthCount
        // MonthlyRelativeRecurrence => MonthCount
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => 
        public int PeriodCount
        {
            get { return this.DateProjectionImpl.GetTranslator().PeriodCount; }
            set { this.DateProjectionImpl.GetTranslator().PeriodCount = value; }
        }

        // DailyRecurrence           => 
        // WeeklyRecurrence          => DaysOfWeek
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => 
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => 
        public EDaysOfWeekFlags DaysOfWeekFlags
        {
            get { return this.DateProjectionImpl.GetTranslator().DaysOfWeekFlags; }
            set { this.DateProjectionImpl.GetTranslator().DaysOfWeekFlags = value; }
        }


        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => DayOfMonth
        // MonthlyRelativeRecurrence => 
        // YearlyRecurrence          => DayOfMonth
        // YearlyRelativeRecurrence  => 
        public int DayOfMonth
        {
            get { return this.DateProjectionImpl.GetTranslator().DayOfMonth; }
            set { this.DateProjectionImpl.GetTranslator().DayOfMonth = value; }
        }


        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => DaysOfWeekExt
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => DaysOfWeekExt
        public EDaysOfWeekExt DaysOfWeekExt
        {
            get { return this.DateProjectionImpl.GetTranslator().DaysOfWeekExt; }
            set { this.DateProjectionImpl.GetTranslator().DaysOfWeekExt = value; }
        }


        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => WeeksInMonth
        // YearlyRecurrence          => 
        // YearlyRelativeRecurrence  => WeeksInMonth
        public EWeeksInMonth WeeksInMonth
        {
            get { return this.DateProjectionImpl.GetTranslator().WeeksInMonth; }
            set { this.DateProjectionImpl.GetTranslator().WeeksInMonth = value; }
        }


        // DailyRecurrence           => 
        // WeeklyRecurrence          => 
        // MonthlyRecurrence         => 
        // MonthlyRelativeRecurrence => 
        // YearlyRecurrence          => Month
        // YearlyRelativeRecurrence  => Month
        public EMonth Month
        {
            get { return this.DateProjectionImpl.GetTranslator().Month; }
            set { this.DateProjectionImpl.GetTranslator().Month = value; }
        }

        public static IDateProjection CreateDateProjectionImpl(EDateProjectionType type)
        {
            switch (type)
            {
                case EDateProjectionType.Daily:
                    return new DailyProjection();
                case EDateProjectionType.Weekly:
                    return new WeeklyProjection();
                case EDateProjectionType.Monthly:
                    return new MonthlyProjection();
                case EDateProjectionType.MonthlyRelative:
                    return new MonthlyRelativeProjection();
                case EDateProjectionType.Yearly:
                    return new YearlyProjection();
                case EDateProjectionType.YearlyRelative:
                    return new YearlyRelativeProjection();
                default:
                    throw new InvalidOperationException($"Unrecognized value ({type}) for the Date Projection type enum ({typeof(EDateProjectionType)}).");
            }
        }

        public static IDateProjection CreateDateProjectionImpl(EDateProjectionType newType, IDateProjection oldProjectionObj)
        {
            IDateProjection newProjectionObj = CreateDateProjectionImpl(newType);

            newProjectionObj.GetTranslator().PeriodCount     = oldProjectionObj.GetTranslator().PeriodCount;
            newProjectionObj.GetTranslator().DaysOfWeekFlags = oldProjectionObj.GetTranslator().DaysOfWeekFlags;
            newProjectionObj.GetTranslator().DayOfMonth      = oldProjectionObj.GetTranslator().DayOfMonth;
            newProjectionObj.GetTranslator().DaysOfWeekExt   = oldProjectionObj.GetTranslator().DaysOfWeekExt;
            newProjectionObj.GetTranslator().WeeksInMonth    = oldProjectionObj.GetTranslator().WeeksInMonth;
            newProjectionObj.GetTranslator().Month           = oldProjectionObj.GetTranslator().Month;

            return newProjectionObj;
        }

    }

}
