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
        public void FluentlyAddWithLeadTimeAsIDateProjection()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .LeadTime(new WeeklyProjection() { DaysOfWeek = EDaysOfWeekFlags.Monday, WeekCount = 1 });
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyAddWithLeadTimeAsDateProjection()
        {
            var dateProjection = new DateProjection(EDateProjectionType.Weekly) {DaysOfWeekFlags = EDaysOfWeekFlags.Monday, PeriodCount = 1,};
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .LeadTime(dateProjection);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyAddWithDailyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .DailyLeadTime(7);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 21)));
        }

        [Test]
        public void FluentlyAddWithWeeklyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .WeeklyLeadTime(1, EDaysOfWeekFlags.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyAddWithMonthlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .MonthlyLeadTime(3, 8);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 12, 8)));
        }

        [Test]
        public void FluentlyAddWithMonthlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .MonthlyLeadTime(1, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 13)));
        }

        [Test]
        public void FluentlyAddWithYearlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.March, 8);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 3, 8)));
        }

        [Test]
        public void FluentlyAddWithYearlyRelativeLeadTime()
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
    }
}
