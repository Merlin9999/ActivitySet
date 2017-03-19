using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test
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

        //[Test]
        //public void CalendarViewWithIncludedHistoryDateRange()
        //{ 
        //    var board = new ActivityBoard();
        //    DateTime startDate = new DateTime(2017, 2, 28);
        //    DateTime endDate = startDate.AddDays(5);
        //    DateTime dueDate = startDate.AddDays(1);
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(dueDate)
        //        .Recurrence(ERecurFromType.FromCompletedDate, x => x.Daily(1))
        //        .AddToBoard(board);
        //    board.InBox.AssignView.DateRange(startDate, endDate)
        //        .IncludeHistory()
        //        //.IncludeFuture()
        //        ;
        //    Assert.That(board.InBox.ViewItems, Is.Not.Empty);
        //    Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(2));
        //}



    }
}
