using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ActivityTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activity = new Activity();
            string testName = "Test Name";
            activity.Name = testName;
            Assert.That(activity.Name, Is.EqualTo(testName));
        }

        [Test]
        public void ActiveDueDateDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.ActiveDueDate, Is.Null);
        }

        [Test]
        public void ActiveDueDatePropertyIsReadWrite()
        {
            var activity = new Activity();
            activity.ActiveDueDate = DateTime.MaxValue;
            Assert.That(activity.ActiveDueDate, Is.EqualTo(DateTime.MaxValue));
        }

        [Test]
        public void ActiveDueDatePropertyHandlesCompletionHistoryRewind()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.SignalCompleted(new DateTime(2017, 3, 2));
            activity.SignalCompleted(new DateTime(2017, 3, 3));
            activity.SignalCompleted(new DateTime(2017, 3, 4));
            activity.SignalCompleted(new DateTime(2017, 3, 5));
            Assert.That(activity.CompletionHistory.Count, Is.EqualTo(5));

            activity.ActiveDueDate = new DateTime(2017, 3, 3);
            Assert.That(activity.CompletionHistory.Count, Is.EqualTo(2));
        }

        [Test]
        public void ActiveDueDatePropertyLeavesCompletionHistoryWhenSetToNull()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.SignalCompleted(new DateTime(2017, 3, 2));
            activity.SignalCompleted(new DateTime(2017, 3, 3));
            activity.SignalCompleted(new DateTime(2017, 3, 4));
            activity.SignalCompleted(new DateTime(2017, 3, 5));
            Assert.That(activity.CompletionHistory.Count, Is.EqualTo(5));

            activity.ActiveDueDate = null;
            Assert.That(activity.CompletionHistory.Count, Is.EqualTo(5));
        }

        [Test]
        public void ResetActiveDueDateFromLastHistoryItem()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 2)));

            activity.ActiveDueDate = null;
            Assert.That(activity.ActiveDueDate, Is.Null);

            activity.ResetActiveDueDateFromLastHistoryItem();
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 2)));
        }

        [Test]
        public void LeadTimePropertyDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.LeadTime, Is.Null);
        }

        [Test]
        public void LeadTimePropertyIsReadWrite()
        {
            var activity = new Activity();
            DateProjection projection = new DateProjection(new WeeklyProjection());
            activity.LeadTime = projection;
            Assert.That(activity.LeadTime, Is.SameAs(projection));
        }

        [Test]
        public void LeadTimeDateIsNullWhenActiveDueDateIsNull()
        {
            var activity = new Activity();
            DateProjection leadTimeProjection = new DateProjection(new DailyProjection() { DayCount = 7 });
            activity.ActiveDueDate = null;
            activity.LeadTime = leadTimeProjection;
            Assert.That(activity.LeadTimeDate, Is.Null);
        }

        [Test]
        public void LeadTimeDateIsNullWhenLeadTimeIsNull()
        {
            var activity = new Activity();
            activity.ActiveDueDate = new DateTime(2017, 2, 8);
            activity.LeadTime = null;
            Assert.That(activity.LeadTimeDate, Is.Null);
        }

        [Test]
        public void CompletionHistoryDefaultsToEmpty()
        {
            var activity = new Activity();
            Assert.That(activity.CompletionHistory, Is.Not.Null);
            Assert.That(activity.CompletionHistory, Is.Empty);
        }

        [Test]
        public void CompletionHistoryIsReadWrite()
        {
            var activity = new Activity();
            var newCompletionHistory = new List<ActivityHistoryItem>();
            Assert.That(activity.CompletionHistory, Is.Not.SameAs(newCompletionHistory));
            activity.CompletionHistory = newCompletionHistory;
            Assert.That(activity.CompletionHistory, Is.SameAs(newCompletionHistory));
        }

        [Test]
        public void CompletionHistoryIsEmptyWhenAssignedNull()
        {
            var activity = new Activity();
            activity.CompletionHistory = null;
            Assert.That(activity.CompletionHistory, Is.Not.Null);
            Assert.That(activity.CompletionHistory, Is.Empty);
        }

        [Test]
        public void RecurrenceDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.Recurrence, Is.Null);
        }

        [Test]
        public void RecurrenceIsReadWrite()
        {
            var activity = new Activity();
            DateRecurrence recurrence = new DateRecurrence();
            activity.Recurrence = recurrence;
            Assert.That(activity.Recurrence, Is.SameAs(recurrence));
        }

        [Test]
        public void SignalCompletedWithNoReccurence()
        {
            string activityName = "New Activity";
            DateTime origActiveDueDate = new DateTime(2017, 2, 28);
            DateTime completionDate = new DateTime(2017, 2, 14);
            var activity = new Activity() {Name = activityName, ActiveDueDate = origActiveDueDate};

            activity.SignalCompleted(completionDate);
            Assert.That(activity.CompletionHistory, Is.Not.Empty);
            Assert.That(activity.ActiveDueDate, Is.Null);

            ActivityHistoryItem item = activity.CompletionHistory.First();
            Assert.That(item.Name, Is.EqualTo(activityName));
            Assert.That(item.DueDate, Is.EqualTo(origActiveDueDate));
            Assert.That(item.CompletedDate, Is.EqualTo(completionDate));
        }

        [Test]
        public void SignalCompletedWithReccurence()
        {
            string activityName = "New Activity";
            DateTime origActiveDueDate = new DateTime(2017, 2, 28);
            DateTime nextActiveDueDate = new DateTime(2017, 3, 28);
            DateTime completionDate = new DateTime(2017, 2, 14);
            var activity = new Activity() { Name = activityName, ActiveDueDate = origActiveDueDate };
            activity.Recurrence = new DateRecurrence()
            {
                RecurFromType = ERecurFromType.FromDueDate,
                DateProjectionImpl = new MonthlyProjection() { MonthCount = 1, DayOfMonth = 28 },
            };

            activity.SignalCompleted(completionDate);
            Assert.That(activity.CompletionHistory, Is.Not.Empty);
            Assert.That(activity.ActiveDueDate, Is.EqualTo(nextActiveDueDate));

            ActivityHistoryItem item = activity.CompletionHistory.First();
            Assert.That(item.Name, Is.EqualTo(activityName));
            Assert.That(item.DueDate, Is.EqualTo(origActiveDueDate));
            Assert.That(item.CompletedDate, Is.EqualTo(completionDate));
        }

        [Test]
        public void VerifyLeadTimeDate()
        {
            var activity = new Activity();
            DateProjection leadTimeProjection = new DateProjection(new DailyProjection() { DayCount = 7 });
            activity.ActiveDueDate = new DateTime(2017, 2, 8);
            activity.LeadTime = leadTimeProjection;
            Assert.That(activity.LeadTime, Is.SameAs(leadTimeProjection));
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 1)));
        }

        [Test]
        public void VerifyLeadTimeDateAfterDateProjectionTranslation()
        {
            var activity = new Activity();
            DateProjection leadTimeProjection = new DateProjection(new WeeklyProjection() { WeekCount = 2, DaysOfWeek = EDaysOfWeekFlags.Monday});
            activity.ActiveDueDate = new DateTime(2017, 2, 28);
            activity.LeadTime = leadTimeProjection;
            Assert.That(activity.LeadTime, Is.SameAs(leadTimeProjection));
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 20)));

            activity.LeadTime.ProjectionType = EDateProjectionType.Daily;
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 26)));
        }

        [Test]
        public void FluentlyCreateAndImplicitlyConvertToActivity()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName);
            Assert.That(activity.Name, Is.EqualTo(activityName));
        }

        [Test]
        public void FluentlyConvertActivityToFluentSyntax()
        {
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.DailyLeadTime(3);
            Assert.That(activity.LeadTime.DateProjectionImpl.GetTranslator().PeriodCount, Is.EqualTo(3));
        }

        [Test]
        public void FluentlyChangeName()
        {
            string newName = "New Name";
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.Name(newName);
            Assert.That(activity.Name, Is.EqualTo(newName));
        }

        [Test]
        public void FluentlyChangeActiveDueDate()
        {
            DateTime newDueDate = new DateTime(2017, 3, 21);
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.ActiveDueDate(newDueDate);
            Assert.That(activity.ActiveDueDate, Is.EqualTo(newDueDate));
        }
        [Test]
        public void FluentlyCreateAndSetName()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName).ToActivity;
            Assert.That(activity.Name, Is.EqualTo(activityName));
        }

        [Test]
        public void FluentlyCreateAndSetNameAndActiveDueDate()
        {
            string activityName = "New Activity";
            DateTime activityActiveDueDate = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew(activityName, activityActiveDueDate);
            Assert.That(activity.Name, Is.EqualTo(activityName));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(activityActiveDueDate));
        }

        [Test]
        public void FluentlyCreateWithLeadTimeAsIDateProjection()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .LeadTime(new WeeklyProjection() { DaysOfWeek = EDaysOfWeekFlags.Monday, WeekCount = 1 });
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyCreateWithLeadTimeAsDateProjection()
        {
            var dateProjection = new DateProjection(EDateProjectionType.Weekly) {DaysOfWeekFlags = EDaysOfWeekFlags.Monday, PeriodCount = 1,};
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .LeadTime(dateProjection);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyCreateWithDailyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .DailyLeadTime(7);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 21)));
        }

        [Test]
        public void FluentlyCreateWithWeeklyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .WeeklyLeadTime(1, EDaysOfWeekFlags.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyCreateWithMonthlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .MonthlyLeadTime(3, 8);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 12, 8)));
        }

        [Test]
        public void FluentlyCreateWithMonthlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .MonthlyLeadTime(1, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 13)));
        }

        [Test]
        public void FluentlyCreateWithYearlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.March, 8);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 3, 8)));
        }

        [Test]
        public void FluentlyCreateWithYearlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.April, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 4, 11)));
        }

        [Test]
        public void FluentlyCreateAndAddToActivitySet()
        {
            var activitySet = new ActivitySet();
            string activityName = "Activity Name";
            DateTime? activityActiveDueDate = new DateTime(2017,2,28);
            Activity.FluentNew(activityName, activityActiveDueDate)
                .AddTo(activitySet);
            Assert.That(activitySet.Count, Is.EqualTo(1));
            var activity = activitySet.First();
            Assert.That(activity.Name, Is.EqualTo(activityName));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(activityActiveDueDate));
        }

        [Test]
        public void FluentlyCreateRecurFromTypeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromDueDate, x => x.Daily(7));
            Assert.That(activity.Recurrence, Is.Not.Null);
        }

        [Test]
        public void FluentlyCreateRecurFromTypeAndMaxRecCountRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, 2, x => x.Daily(7));
            Assert.That(activity.Recurrence, Is.Not.Null);
        }

        [Test]
        public void FluentlyCreateRecurFromTypeAndDateRangeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, new DateTime(2017,2,28), new DateTime(2017,3,29), x => x.Daily(7));
            Assert.That(activity.Recurrence, Is.Not.Null);
        }

        [Test]
        public void FluentlyCreateWithRecurrenceAsIDateProjection()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => new DailyProjection() { DayCount = 5 });
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 2, 19)));
        }

        [Test]
        public void FluentlyCreateWithRecurrenceAsDateRecurrence()
        {
            var dateRecurrence = new DateRecurrence(EDateProjectionType.Daily, ERecurFromType.FromDueDate) { PeriodCount = 4, };
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(dateRecurrence);
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 4)));
        }

        [Test]
        public void FluentlyCreateWithDailyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromDueDate, x => x.Daily(9));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 9)));
        }

        [Test]
        public void FluentlyCreateWithWeeklyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromDueDate, x=> x.Weekly(1, EDaysOfWeekFlags.Monday));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 6)));
        }

        [Test]
        public void FluentlyCreateWithMonthlyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromDueDate, x => x.Monthly(3, 8));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 5, 8)));
        }

        [Test]
        public void FluentlyCreateWithMonthlyRelativeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate,  x => x.Monthly(2, EWeeksInMonth.Third, EDaysOfWeekExt.Monday));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 20)));
        }

        [Test]
        public void FluentlyCreateWithYearlyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromDueDate, x => x.Yearly(EMonth.March, 8));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 3, 8)));
        }

        [Test]
        public void FluentlyCreateWithYearlyRelativeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromDueDate, x => x.Yearly(EMonth.April, EWeeksInMonth.Second, EDaysOfWeekExt.Monday));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(new DateTime(2017, 4, 10)));
        }

    }
}
