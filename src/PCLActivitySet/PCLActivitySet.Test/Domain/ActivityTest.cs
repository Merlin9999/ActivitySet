using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain
{
    [TestFixture]
    public class ActivityTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activity = new Activity();
            activity.Name.Should().BeNull();
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activity = new Activity();
            string testName = "Test Name";
            activity.Name = testName;
            activity.Name.Should().Be(testName);
        }

        [Test]
        public void ActivityListGuidPropertyDefaultsToNull()
        {
            var activity = new Activity();
            activity.ActivityListGuid.Should().BeNull();
        }

        [Test]
        public void ActivityListGuidPropertyIsReadWrite()
        {
            var activity = new Activity();
            Guid testGuid = Guid.NewGuid();
            activity.ActivityListGuid = testGuid;
            activity.ActivityListGuid.Should().Be(testGuid);
        }

        [Test]
        public void ActiveDueDateDefaultsToNull()
        {
            var activity = new Activity();
            activity.ActiveDueDate.Should().BeNull();
        }

        [Test]
        public void ActiveDueDatePropertyIsReadWrite()
        {
            var activity = new Activity();
            activity.ActiveDueDate = DateTime.MaxValue;
            activity.ActiveDueDate.Should().Be(DateTime.MaxValue);
        }

        [Test]
        public void ActiveDueDatePropertyHandlesCompletionHistoryRewind()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.SignalCompleted(new DateTime(2017, 3, 2));
            activity.SignalCompleted(new DateTime(2017, 3, 3));
            activity.SignalCompleted(new DateTime(2017, 3, 4));
            activity.SignalCompleted(new DateTime(2017, 3, 5));
            activity.CompletionHistory.Count.Should().Be(5);

            activity.ActiveDueDate = new DateTime(2017, 3, 3);
            activity.CompletionHistory.Count.Should().Be(2);
        }

        [Test]
        public void ActiveDueDatePropertyLeavesCompletionHistoryWhenSetToNull()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.SignalCompleted(new DateTime(2017, 3, 2));
            activity.SignalCompleted(new DateTime(2017, 3, 3));
            activity.SignalCompleted(new DateTime(2017, 3, 4));
            activity.SignalCompleted(new DateTime(2017, 3, 5));
            activity.CompletionHistory.Count.Should().Be(5);

            activity.ActiveDueDate = null;
            activity.CompletionHistory.Count.Should().Be(5);
        }

        [Test]
        public void ResetActiveDueDateFromLastHistoryItem()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 2));

            activity.ActiveDueDate = null;
            activity.ActiveDueDate.Should().BeNull();

            activity.ResetActiveDueDateFromLastHistoryItem();
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 2));
        }

        [Test]
        public void ResetActiveDueDateWhenRecurFromDueDateAndHistoryDueDateIsNull()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 2));

            activity.Recurrence.RecurFromType = ERecurFromType.FromActiveDueDate;
            activity.ResetActiveDueDateFromLastHistoryItem();
            activity.ActiveDueDate.Should().BeNull();
        }

        [Test]
        public void ResetActiveDueDateWhenRecurFromCompletionDateAndHistoryDueDateIsNull()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1));
            activity.SignalCompleted(new DateTime(2017, 3, 1));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 2));

            var tempActiveDueDate = new DateTime(2017, 3, 28);
            activity.ActiveDueDate = tempActiveDueDate;
            activity.ActiveDueDate.Should().Be(tempActiveDueDate);

            activity.ResetActiveDueDateFromLastHistoryItem();
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 2));
        }

        [Test]
        public void LeadTimePropertyDefaultsToNull()
        {
            var activity = new Activity();
            activity.LeadTime.Should().BeNull();
        }

        [Test]
        public void LeadTimePropertyIsReadWrite()
        {
            var activity = new Activity();
            DateProjection projection = new DateProjection(new WeeklyProjection());
            activity.LeadTime = projection;
            activity.LeadTime.Should().BeSameAs(projection);
        }

        [Test]
        public void LeadTimeDateIsNullWhenActiveDueDateIsNull()
        {
            var activity = new Activity();
            DateProjection leadTimeProjection = new DateProjection(new DailyProjection() { DayCount = 7 });
            activity.ActiveDueDate = null;
            activity.LeadTime = leadTimeProjection;
            activity.LeadTimeDate.Should().BeNull();
        }

        [Test]
        public void LeadTimeDateIsNullWhenLeadTimeIsNull()
        {
            var activity = new Activity();
            activity.ActiveDueDate = new DateTime(2017, 2, 8);
            activity.LeadTime = null;
            activity.LeadTimeDate.Should().BeNull();
        }

        [Test]
        public void CompletionHistoryDefaultsToEmpty()
        {
            var activity = new Activity();
            activity.CompletionHistory.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void CompletionHistoryIsReadWrite()
        {
            var activity = new Activity();
            var newCompletionHistory = new List<ActivityHistoryItem>();
            activity.CompletionHistory.Should().NotBeSameAs(newCompletionHistory);
            activity.CompletionHistory = newCompletionHistory;
            activity.CompletionHistory.Should().BeSameAs(newCompletionHistory);
        }

        [Test]
        public void CompletionHistoryIsEmptyWhenAssignedNull()
        {
            var activity = new Activity();
            activity.CompletionHistory = null;
            activity.CompletionHistory.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void RecurrenceDefaultsToNull()
        {
            var activity = new Activity();
            activity.Recurrence.Should().BeNull();
        }

        [Test]
        public void RecurrenceIsReadWrite()
        {
            var activity = new Activity();
            DateRecurrence recurrence = new DateRecurrence();
            activity.Recurrence = recurrence;
            activity.Recurrence.Should().BeSameAs(recurrence);
        }

        [Test]
        public void SignalCompletedWithNoReccurence()
        {
            string activityName = "New Activity";
            DateTime origActiveDueDate = new DateTime(2017, 2, 28);
            DateTime completionDate = new DateTime(2017, 2, 14);
            var activity = new Activity() {Name = activityName, ActiveDueDate = origActiveDueDate};

            activity.SignalCompleted(completionDate);
            activity.CompletionHistory.Should().NotBeEmpty();
            activity.ActiveDueDate.Should().BeNull();

            ActivityHistoryItem item = activity.CompletionHistory.First();
            item.Name.Should().Be(activityName);
            item.DueDate.Should().Be(origActiveDueDate);
            item.CompletedDate.Should().Be(completionDate);
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
                RecurFromType = ERecurFromType.FromActiveDueDate,
                DateProjectionImpl = new MonthlyProjection() { MonthCount = 1, DayOfMonth = 28 },
            };

            activity.SignalCompleted(completionDate);
            activity.CompletionHistory.Should().NotBeEmpty();
            activity.ActiveDueDate.Should().Be(nextActiveDueDate);

            ActivityHistoryItem item = activity.CompletionHistory.First();
            item.Name.Should().Be(activityName);
            item.DueDate.Should().Be(origActiveDueDate);
            item.CompletedDate.Should().Be(completionDate);
        }

        [Test]
        public void GetProjectedFutureDueDates()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName)
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(1));
            List<ActivityProjectionItem> activityProjectedDueDates = activity.GetProjectedFutureDueDates(new DateTime(2017, 3, 5)).ToList();
            activityProjectedDueDates.Count.Should().Be(5);
            activityProjectedDueDates[0].Name.Should().Be(activityName);
            activityProjectedDueDates[0].DueDate.Should().Be(new DateTime(2017, 3, 1));
            activityProjectedDueDates[1].Name.Should().Be(activityName);
            activityProjectedDueDates[1].DueDate.Should().Be(new DateTime(2017, 3, 2));
            activityProjectedDueDates[2].Name.Should().Be(activityName);
            activityProjectedDueDates[2].DueDate.Should().Be(new DateTime(2017, 3, 3));
            activityProjectedDueDates[3].Name.Should().Be(activityName);
            activityProjectedDueDates[3].DueDate.Should().Be(new DateTime(2017, 3, 4));
            activityProjectedDueDates[4].Name.Should().Be(activityName);
            activityProjectedDueDates[4].DueDate.Should().Be(new DateTime(2017, 3, 5));
        }

        [Test]
        public void GetProjectedFutureDueDatesWithActiveDueDateNull()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(1));
            List<ActivityProjectionItem> activityProjectedDueDates = activity.GetProjectedFutureDueDates(new DateTime(2017, 3, 2)).ToList();
            activityProjectedDueDates.Count.Should().Be(0);
        }

        [Test]
        public void GetProjectedFutureDueDatesWithRecurrenceNull()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28));
            List<ActivityProjectionItem> activityProjectedDueDates = activity.GetProjectedFutureDueDates(new DateTime(2017, 3, 2)).ToList();
            activityProjectedDueDates.Count.Should().Be(0);
        }

        [Test]
        public void VerifyLeadTimeDate()
        {
            var activity = new Activity();
            DateProjection leadTimeProjection = new DateProjection(new DailyProjection() { DayCount = 7 });
            activity.ActiveDueDate = new DateTime(2017, 2, 8);
            activity.LeadTime = leadTimeProjection;
            activity.LeadTime.Should().BeSameAs(leadTimeProjection);
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 1));
        }

        [Test]
        public void VerifyLeadTimeDateAfterDateProjectionTranslation()
        {
            var activity = new Activity();
            DateProjection leadTimeProjection = new DateProjection(new WeeklyProjection() { WeekCount = 2, DaysOfWeek = EDaysOfWeekFlags.Monday});
            activity.ActiveDueDate = new DateTime(2017, 2, 28);
            activity.LeadTime = leadTimeProjection;
            activity.LeadTime.Should().BeSameAs(leadTimeProjection);
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 20));

            activity.LeadTime.ProjectionType = EDateProjectionType.Daily;
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 26));
        }

        [Test]
        public void FluentlyCreateAndImplicitlyConvertToActivity()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName);
            activity.Name.Should().Be(activityName);
        }

        [Test]
        public void FluentlyConvertActivityToFluentSyntax()
        {
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.DailyLeadTime(3);
            activity.LeadTime.DateProjectionImpl.GetTranslator().PeriodCount.Should().Be(3);
        }

        [Test]
        public void FluentlyChangeName()
        {
            string newName = "New Name";
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.Name(newName);
            activity.Name.Should().Be(newName);
        }

        [Test]
        public void FluentlyChangeActiveDueDate()
        {
            DateTime newDueDate = new DateTime(2017, 3, 21);
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.ActiveDueDate(newDueDate);
            activity.ActiveDueDate.Should().Be(newDueDate);
        }
        [Test]
        public void FluentlyCreateAndSetName()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName).ToActivity;
            activity.Name.Should().Be(activityName);
        }

        [Test]
        public void FluentlyCreateAndSetNameAndActiveDueDate()
        {
            string activityName = "New Activity";
            DateTime activityActiveDueDate = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew(activityName)
                .ActiveDueDate(activityActiveDueDate);
            activity.Name.Should().Be(activityName);
            activity.ActiveDueDate.Should().Be(activityActiveDueDate);
        }

        [Test]
        public void FluentlyCreateWithLeadTimeAsIDateProjection()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .LeadTime(new WeeklyProjection() { DaysOfWeek = EDaysOfWeekFlags.Monday, WeekCount = 1 });
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 27));
        }

        [Test]
        public void FluentlyCreateWithLeadTimeAsDateProjection()
        {
            var dateProjection = new DateProjection(EDateProjectionType.Weekly) {DaysOfWeekFlags = EDaysOfWeekFlags.Monday, PeriodCount = 1,};
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .LeadTime(dateProjection);
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 27));
        }

        [Test]
        public void FluentlyCreateWithDailyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(7);
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 21));
        }

        [Test]
        public void FluentlyCreateWithWeeklyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .WeeklyLeadTime(1, EDaysOfWeekFlags.Monday);
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 27));
        }

        [Test]
        public void FluentlyCreateWithMonthlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .MonthlyLeadTime(3, 8);
            activity.LeadTimeDate.Should().Be(new DateTime(2016, 12, 8));
        }

        [Test]
        public void FluentlyCreateWithMonthlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .MonthlyLeadTime(1, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            activity.LeadTimeDate.Should().Be(new DateTime(2017, 2, 13));
        }

        [Test]
        public void FluentlyCreateWithYearlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.March, 8);
            activity.LeadTimeDate.Should().Be(new DateTime(2016, 3, 8));
        }

        [Test]
        public void FluentlyCreateWithYearlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.April, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            activity.LeadTimeDate.Should().Be(new DateTime(2016, 4, 11));
        }

        [Test]
        public void FluentlyCreateAndAddToActivityBoard()
        {
            var activityBoard = new ActivityBoard();
            string activityName = "Activity Name";
            DateTime activityActiveDueDate = new DateTime(2017,2,28);
            Activity.FluentNew(activityName)
                .ActiveDueDate(activityActiveDueDate)
                .AddToBoard(activityBoard);
            activityBoard.UnfilteredActivities.Count().Should().Be(1);
            var activity = activityBoard.UnfilteredActivities.First();
            activity.Name.Should().Be(activityName);
            activity.ActiveDueDate.Should().Be(activityActiveDueDate);
        }

        [Test]
        public void FluentlyCreateRecurFromTypeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(7));
            activity.Recurrence.Should().NotBeNull();
        }

        [Test]
        public void FluentlyCreateRecurFromTypeAndMaxRecCountRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, 2, x => x.Daily(7));
            activity.Recurrence.Should().NotBeNull();
        }

        [Test]
        public void FluentlyCreateRecurFromTypeAndDateRangeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, new DateTime(2017,2,28), new DateTime(2017,3,29), x => x.Daily(7));
            activity.Recurrence.Should().NotBeNull();
        }

        [Test]
        public void FluentlyCreateWithRecurrenceAsIDateProjection()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate, x => new DailyProjection() { DayCount = 5 });
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 2, 19));
        }

        [Test]
        public void FluentlyCreateWithRecurrenceAsDateRecurrence()
        {
            var dateRecurrence = new DateRecurrence(EDateProjectionType.Daily, ERecurFromType.FromActiveDueDate) { PeriodCount = 4, };
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(dateRecurrence);
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 4));
        }

        [Test]
        public void FluentlyCreateWithDailyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(9));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 9));
        }

        [Test]
        public void FluentlyCreateWithWeeklyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x=> x.Weekly(1, EDaysOfWeekFlags.Monday));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 6));
        }

        [Test]
        public void FluentlyCreateWithMonthlyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Monthly(3, 8));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 5, 8));
        }

        [Test]
        public void FluentlyCreateWithMonthlyRelativeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromCompletedDate,  x => x.Monthly(2, EWeeksInMonth.Third, EDaysOfWeekExt.Monday));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 20));
        }

        [Test]
        public void FluentlyCreateWithYearlyRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Yearly(EMonth.March, 8));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 3, 8));
        }

        [Test]
        public void FluentlyCreateWithYearlyRelativeRecurrence()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Yearly(EMonth.April, EWeeksInMonth.Second, EDaysOfWeekExt.Monday));
            activity.SignalCompleted(new DateTime(2017, 2, 14));
            activity.ActiveDueDate.Should().Be(new DateTime(2017, 4, 10));
        }


        [Test]
        public void NonCompletedActivityWithNoRecurrenceIsActive()
        {
            Activity activity = Activity.FluentNew("New Activity");
            activity.IsActive.Should().BeTrue();
        }

        [Test]
        public void NonCompletedActivityWithRecurrenceIsActive()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(14));
            activity.IsActive.Should().BeTrue();
        }

        [Test]
        public void CompletedActivityWithNoRecurrenceIsInactive()
        {
            Activity activity = Activity.FluentNew("New Activity");
            activity.SignalCompleted(new DateTime(2017, 2, 26));
            activity.IsActive.Should().BeFalse();
        }

        [Test]
        public void CompletedActivityWithRecurrenceIsActive()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(14));
            activity.SignalCompleted(new DateTime(2017, 2, 26));
            activity.IsActive.Should().BeTrue();
        }

        [Test]
        public void CompletedActivityWithNoFutureRecurrenceIsInactive()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, 1, x => x.Daily(14));
            activity.SignalCompleted(new DateTime(2017, 2, 26));
            activity.IsActive.Should().BeFalse();
        }

        [Test]
        public void LastCompletedDateIsNullWhenNotEverCompleted()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, 1, x => x.Daily(14));
            activity.LastCompletedDate.Should().BeNull();
        }

        [Test]
        public void LastCompletedDateIsSetWhenCompleted()
        {
            Activity activity = Activity.FluentNew("New Activity")
                .Recurrence(ERecurFromType.FromCompletedDate, 1, x => x.Daily(14));
            activity.SignalCompleted(new DateTime(2017, 2, 26));
            activity.LastCompletedDate.Should().Be(new DateTime(2017, 2, 26));
        }
    }
}
