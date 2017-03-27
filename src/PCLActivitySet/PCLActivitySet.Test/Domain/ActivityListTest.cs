using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCLActivitySet.Domain;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Domain.Views;

namespace PCLActivitySet.Test.Domain
{
    [TestFixture]
    class ActivityListTest
    {
        [Test]
        public void CannotCreateActivityListWithNullActivityBoard()
        {
            Assert.That(() => new ActivityList(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activityList = new ActivityList(new ActivityBoard());
            Assert.That(activityList.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activityList = new ActivityList(new ActivityBoard());
            string testName = "Test Name";
            activityList.Name = testName;
            Assert.That(activityList.Name, Is.EqualTo(testName));
        }

        [Test]
        public void ViewItemsEmptyWhenActivitiesEmpty()
        {
            var board = new ActivityBoard();
            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void ViewItemsContainsAddedActivities()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board);
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateTimePropertyDefaultsToNowWhichAutoUpdates()
        {
            var activityList = new ActivityList(new ActivityBoard());
            var focusDateTime = activityList.ViewModes.FocusDateView.Get().FocusDateTime;
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));
            Assert.That(focusDateTime, Is.LessThan(activityList.ViewModes.FocusDateView.Get().FocusDateTime));
        }

        [Test]
        public void FocusDateTimePropertyIsReadWrite()
        {
            var activityList = new ActivityList(new ActivityBoard());
            DateTime now = DateTime.Now;
            activityList.ViewModes.FocusDateView.Get().FocusDateTime = now;
            Assert.That(activityList.ViewModes.FocusDateView.Get().FocusDateTime, Is.EqualTo(now));
        }

        [Test]
        public void FocusDateTimePropertyAutoUpdatesWhenReset()
        {
            var activityList = new ActivityList(new ActivityBoard());
            FocusDateView focusDateView = activityList.ViewModes.FocusDateView.Get();
            focusDateView.FocusDateTime = DateTime.Now + TimeSpan.FromDays(1);
            focusDateView.ResetFocusDateTimeToNow();
            var focusDateTime = focusDateView.FocusDateTime;
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));
            Assert.That(focusDateTime, Is.LessThan(focusDateView.FocusDateTime));
        }

        [Test]
        public void FocusDatePropertyIsDateOfFocusDateTime()
        {
            var activityList = new ActivityList(new ActivityBoard());
            DateTime yesterday = DateTime.Now.AddDays(-1);
            activityList.ViewModes.FocusDateView.Get().FocusDateTime = yesterday;
            Assert.That(activityList.ViewModes.FocusDateView.Get().FocusDate, Is.EqualTo(yesterday.Date));
        }

        [Test]
        public void ClearingActiveViewShowsAllViewItems()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ViewModes.ExcludeNonActiveView.Enable();
            board.InBox.ViewModes.Clear();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ExcludeNonActiveView()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ViewModes.ExcludeNonActiveView.Enable();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateViewWithInactiveActivitiesWithFocusDateInThePast()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(DateTime.Now.AddDays(-2))
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }


        [Test]
        public void FocusDateViewWithInactiveActivitiesWithViewDelayNotExpired()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ViewModes.FocusDateView
                .CompletedFilterDelay(TimeSpan.FromHours(4))
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
        }

        [Test]
        public void FocusDateViewWithInactiveActivitiesWithFilterDelayExpired()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ViewModes.FocusDateView
                .CompletedFilterDelay(TimeSpan.FromMilliseconds(1))
                .Enable();
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateViewWithFocusDateSameAsActiveDueDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime).AddToBoard(board);
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(focusDateTime)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateViewWithFocusDateAfterActiveDueDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(-1))
                .AddToBoard(board);
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(focusDateTime)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateViewWithFocusDateBeforeActiveDueDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(1))
                .AddToBoard(board);
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(focusDateTime)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void FocusDateViewWithFocusDateSameAsLeadTimeDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(1))
                .DailyLeadTime(1)
                .AddToBoard(board);
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(focusDateTime)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateViewWithFocusDateAfterLeadTimeDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(1))
                .DailyLeadTime(2)
                .AddToBoard(board);
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(focusDateTime)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FocusDateViewWithFocusDateBeforeLeadTimeDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(2))
                .DailyLeadTime(1)
                .AddToBoard(board);
            board.InBox.ViewModes.FocusDateView
                .FocusDateTime(focusDateTime)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void CalendarViewWithNoDateRange()
        {
            var board = new ActivityBoard();
            board.InBox.ViewModes.CalendarView.Enable();
            Assert.That(() => board.InBox.ViewItems.Any(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void CalendarViewWithEndDateBeforeStartDate()
        {
            var board = new ActivityBoard();
            DateTime now = DateTime.Now;
            board.InBox.ViewModes.CalendarView
                .DateRange(now, now.AddDays(-1))
                .Enable();
            Assert.That(() => board.InBox.ViewItems.Any(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void CalendarViewWithActivityWithinDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(2);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CalendarViewWithActivityOnStartOfDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CalendarViewWithActivityOnEndOfDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = endDate;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CalendarViewWithActivityBeforeStartOfDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(-1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void CalendarViewWithActivityAfterEndOfDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = endDate.AddDays(1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void CalendarViewWithIncludedHistoryWithinDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(2);
            DateTime completedDate = startDate.AddDays(1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            activity.SignalCompleted(completedDate);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeHistory()
                .Enable();

            DateTime expectedActiveDueDate = completedDate.AddDays(2);
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == completedDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == completedDate), Is.TypeOf<HistoryViewItem>());
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == expectedActiveDueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == expectedActiveDueDate), Is.TypeOf<ActivityViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedHistoryItemOnStartDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(1);
            DateTime completedDate = startDate;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            activity.SignalCompleted(completedDate);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeHistory()
                .Enable();

            DateTime expectedActiveDueDate = completedDate.AddDays(2);
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == completedDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == completedDate), Is.TypeOf<HistoryViewItem>());
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == expectedActiveDueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == expectedActiveDueDate), Is.TypeOf<ActivityViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedHistoryItemOnEndDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(1);
            DateTime completedDate = endDate;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            activity.SignalCompleted(completedDate);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeHistory()
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == completedDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == completedDate), Is.TypeOf<HistoryViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedHistoryItemBeforeStartDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(1);
            DateTime completedDate = startDate.AddDays(-1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            activity.SignalCompleted(completedDate);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeHistory()
                .Enable();

            DateTime expectedActiveDueDate = completedDate.AddDays(2);
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == expectedActiveDueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == expectedActiveDueDate), Is.TypeOf<ActivityViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedHistoryItemAfterEndDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(1);
            DateTime completedDate = endDate.AddDays(1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            activity.SignalCompleted(completedDate);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeHistory()
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void CalendarViewWithIncludedFutureWithinDateRange()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            DateTime dueDate = startDate.AddDays(1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeFuture()
                .Enable();

            DateTime recur1DueDate = dueDate.AddDays(2);
            DateTime recur2DueDate = recur1DueDate.AddDays(2);
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(3));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == dueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == dueDate), Is.TypeOf<ActivityViewItem>());
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur1DueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur1DueDate), Is.TypeOf<ProjectionViewItem>());
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur2DueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur2DueDate), Is.TypeOf<ProjectionViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedFutureOnStartDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 3, 1);
            DateTime endDate = startDate.AddDays(3);
            DateTime dueDate = startDate.AddDays(-2);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeFuture()
                .Enable();

            DateTime recur1DueDate = dueDate.AddDays(2);
            DateTime recur2DueDate = recur1DueDate.AddDays(2);
            Assert.That(recur1DueDate, Is.EqualTo(startDate));
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur1DueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur1DueDate), Is.TypeOf<ProjectionViewItem>());
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur2DueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur2DueDate), Is.TypeOf<ProjectionViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedFutureOnEndDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 3, 1);
            DateTime endDate = startDate.AddDays(3);
            DateTime dueDate = startDate.AddDays(-1);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeFuture()
                .Enable();

            DateTime recur1DueDate = dueDate.AddDays(2);
            DateTime recur2DueDate = recur1DueDate.AddDays(2);
            Assert.That(recur2DueDate, Is.EqualTo(endDate));
            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur1DueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur1DueDate), Is.TypeOf<ProjectionViewItem>());
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur2DueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == recur2DueDate), Is.TypeOf<ProjectionViewItem>());
        }

        [Test]
        public void CalendarViewWithIncludedFutureBeforeStartDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 3, 1);
            DateTime endDate = startDate.AddDays(3);
            DateTime dueDate = startDate.AddDays(-3);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, 1, x => x.Daily(2))
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeFuture()
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Empty);
        }

        [Test]
        public void CalendarViewWithIncludedFutureAfterEndDate()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 3, 1);
            DateTime endDate = startDate.AddDays(3);
            DateTime dueDate = startDate.AddDays(2);
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(dueDate)
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeFuture()
                .Enable();

            Assert.That(board.InBox.ViewItems, Is.Not.Empty);
            Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == dueDate), Is.Not.Null);
            Assert.That(board.InBox.ViewItems.FirstOrDefault(x => x.Date == dueDate), Is.TypeOf<ActivityViewItem>());
        }

        [Test]
        public void GetGoalsForItemsFromAView()
        {
            var board = new ActivityBoard();
            DateTime startDate = new DateTime(2017, 2, 28);
            DateTime endDate = startDate.AddDays(5);
            Activity activity1 = Activity.FluentNew("Activity 1")
                .ActiveDueDate(startDate.AddDays(1))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(2))
                .AddToBoard(board);
            activity1.SignalCompleted(startDate.AddDays(1));
            Activity activity2 = Activity.FluentNew("Activity 2")
                .ActiveDueDate(startDate.AddDays(2))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1))
                .AddToBoard(board);
            activity2.SignalCompleted(startDate.AddDays(3));
            Activity activity3 = Activity.FluentNew("Activity 3")
                .ActiveDueDate(startDate.AddDays(2))
                .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1))
                .AddToBoard(board);
            activity3.SignalCompleted(startDate);
            activity3.SignalCompleted(startDate.AddDays(1));
            activity3.SignalCompleted(startDate.AddDays(2));
            ActivityGoal goal1 = board.AddNewGoal("Goal 1");
            ActivityGoal goal2 = board.AddNewGoal("Goal 2");
            board.MoveActivity(activity1).ToGoal(goal1);
            board.MoveActivity(activity2).ToGoal(goal2);
            board.MoveActivity(activity3).ToGoal(goal1);
            board.InBox.ViewModes.CalendarView
                .DateRange(startDate, endDate)
                .IncludeHistory()
                .IncludeFuture()
                .Enable();
            IEnumerable<Activity> activities = board.InBox.ViewItems.Select(vi => vi.Activity);
            ILookup<ActivityGoal, Activity> goalLookup = board.GetGoalLookupFromActivities(activities);

            Assert.That(goalLookup, Is.Not.Empty);
            Assert.That(goalLookup.Count, Is.EqualTo(2));
            Assert.That(goalLookup[goal1], Is.Not.Empty);
            Assert.That(goalLookup[goal1].Count(), Is.EqualTo(2));
            Assert.That(goalLookup[goal1], Has.Member(activity1));
            Assert.That(goalLookup[goal1], Has.Member(activity3));
            Assert.That(goalLookup[goal2], Is.Not.Empty);
            Assert.That(goalLookup[goal2].Count(), Is.EqualTo(1));
            Assert.That(goalLookup[goal2], Has.Member(activity2));
        }
    }
}
