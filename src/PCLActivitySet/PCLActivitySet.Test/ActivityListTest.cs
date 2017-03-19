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
        public void FocusDateTimePropertyDefaultsToNowWhichAutoUpdates()
        {
            var activityList = new ActivityList(new ActivityBoard());
            var focusDateTime = activityList.FocusDateTime;
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));
            Assert.That(focusDateTime, Is.LessThan(activityList.FocusDateTime));
        }

        [Test]
        public void FocusDateTimePropertyIsReadWrite()
        {
            var activityList = new ActivityList(new ActivityBoard());
            DateTime now = DateTime.Now;
            activityList.FocusDateTime = now;
            Assert.That(activityList.FocusDateTime, Is.EqualTo(now));
        }

        [Test]
        public void FocusDateTimePropertyAutoUpdatesWhenReset()
        {
            var activityList = new ActivityList(new ActivityBoard());
            activityList.FocusDateTime = DateTime.Now + TimeSpan.FromDays(1);
            activityList.ResetFocusDateTimeToNow();
            var focusDateTime = activityList.FocusDateTime;
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));
            Assert.That(focusDateTime, Is.LessThan(activityList.FocusDateTime));
        }

        [Test]
        public void FocusDatePropertyIsDateOfFocusDateTime()
        {
            var activityList = new ActivityList(new ActivityBoard());
            DateTime yesterday = DateTime.Now.AddDays(-1);
            activityList.FocusDateTime = yesterday;
            Assert.That(activityList.FocusDate, Is.EqualTo(yesterday.Date));
        }

        //[Test]
        //public void ClearingActivityFiltersShowsAllActivities()
        //{
        //    var board = new ActivityBoard();
        //    Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
        //    Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
        //    activity1.SignalCompleted(new DateTime(2017, 2, 28));
        //    board.InBox.AssignView.ExcludeNonActive();
        //    board.InBox.AssignView.Clear();

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));
        //}

        //[Test]
        //public void GetFilterThatWasApplied()
        //{
        //    var board = new ActivityBoard();
        //    ExcludeNonActiveWithDelayFilter filter =
        //        board.InBox.AssignView
        //            .ExcludeNonActiveWithDelay(TimeSpan.FromHours(4))
        //            .GetExcludeNonActiveWithDelay();

        //    Assert.That(filter, Is.Not.Null);
        //    Assert.That(filter.Delay, Is.EqualTo(TimeSpan.FromHours(4)));
        //}

        //[Test]
        //public void GetFilterThatWasNotApplied()
        //{
        //    var board = new ActivityBoard();
        //    ExcludeNonActiveWithDelayFilter filter =
        //        board.InBox.AssignView
        //            .GetExcludeNonActiveWithDelay();

        //    Assert.That(filter, Is.Null);
        //}

        //[Test]
        //public void OverwriteFilterWithUpdatedFilterOfSameType()
        //{
        //    var board = new ActivityBoard();
        //    ExcludeNonActiveWithDelayFilter filter = board.InBox.AssignView
        //        .ExcludeNonActiveWithDelay(TimeSpan.FromMinutes(10))
        //        .FocusDateRadar()
        //        .ExcludeNonActiveWithDelay(TimeSpan.FromHours(4))
        //        .GetExcludeNonActiveWithDelay();
        //    Assert.That(filter.Delay, Is.Not.EqualTo(TimeSpan.FromMinutes(10)));
        //    Assert.That(filter.Delay, Is.EqualTo(TimeSpan.FromHours(4)));
        //}

        //[Test]
        //public void FilteringInactiveActivities()
        //{
        //    var board = new ActivityBoard();
        //    Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
        //    Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
        //    activity1.SignalCompleted(new DateTime(2017, 2, 28));
        //    board.InBox.AssignView.ExcludeNonActive();

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void FilteringInactiveActivitiesWithFocusDateInThePast()
        //{
        //    var board = new ActivityBoard();
        //    Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
        //    Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
        //    activity1.SignalCompleted(new DateTime(2017, 2, 28));
        //    board.InBox.AssignView.ExcludeNonActive();
        //    board.InBox.FocusDateTime = DateTime.Now.AddDays(-2);

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}


        //[Test]
        //public void FilteringInactiveActivitiesWithFilterDelayNotExpired()
        //{
        //    var board = new ActivityBoard();
        //    Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
        //    Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
        //    activity1.SignalCompleted(new DateTime(2017, 2, 28));
        //    board.InBox.AssignView.ExcludeNonActiveWithDelay(TimeSpan.FromHours(4));

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));
        //}

        //[Test]
        //public void FilteringInactiveActivitiesWithFilterDelayExpired()
        //{
        //    var board = new ActivityBoard();
        //    Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
        //    Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
        //    activity1.SignalCompleted(new DateTime(2017, 2, 28));
        //    board.InBox.AssignView.ExcludeNonActiveWithDelay(TimeSpan.FromMilliseconds(1));
        //    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void FilteringFocusDateRadarWhenFocusDateSameAsActiveDueDate()
        //{
        //    var board = new ActivityBoard();
        //    DateTime focusDateTime = new DateTime(2017, 2, 28);
        //    board.InBox.FocusDateTime = focusDateTime;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(focusDateTime).AddToBoard(board);
        //    board.InBox.AssignView.FocusDateRadar();

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void FilteringFocusDateRadarWhenFocusDateAfterActiveDueDate()
        //{
        //    var board = new ActivityBoard();
        //    DateTime focusDateTime = new DateTime(2017, 2, 28);
        //    board.InBox.FocusDateTime = focusDateTime;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(focusDateTime.AddDays(-1))
        //        .AddToBoard(board);
        //    board.InBox.AssignView.FocusDateRadar();

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void FilteringFocusDateRadarWhenFocusDateBeforeActiveDueDate()
        //{
        //    var board = new ActivityBoard();
        //    DateTime focusDateTime = new DateTime(2017, 2, 28);
        //    board.InBox.FocusDateTime = focusDateTime;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(focusDateTime.AddDays(1))
        //        .AddToBoard(board);
        //    board.InBox.AssignView.FocusDateRadar();

        //    Assert.That(board.InBox.Activities, Is.Empty);
        //}

        //[Test]
        //public void FilteringFocusDateRadarWhenFocusDateSameAsLeadTimeDate()
        //{
        //    var board = new ActivityBoard();
        //    DateTime focusDateTime = new DateTime(2017, 2, 28);
        //    board.InBox.FocusDateTime = focusDateTime;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(focusDateTime.AddDays(1))
        //        .DailyLeadTime(1)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.FocusDateRadar();

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void FilteringFocusDateRadarWhenFocusDateAfterLeadTimeDate()
        //{
        //    var board = new ActivityBoard();
        //    DateTime focusDateTime = new DateTime(2017, 2, 28);
        //    board.InBox.FocusDateTime = focusDateTime;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(focusDateTime.AddDays(1))
        //        .DailyLeadTime(2)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.FocusDateRadar();

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void FilteringFocusDateRadarWhenFocusDateBeforeLeadTimeDate()
        //{
        //    var board = new ActivityBoard();
        //    DateTime focusDateTime = new DateTime(2017, 2, 28);
        //    board.InBox.FocusDateTime = focusDateTime;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(focusDateTime.AddDays(2))
        //        .DailyLeadTime(1)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.FocusDateRadar();

        //    Assert.That(board.InBox.Activities, Is.Empty);
        //}

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

        //[Test]
        //public void ViewItemFiltersActivityWithinDateRange()
        //{
        //    var board = new ActivityBoard();
        //    DateTime startDate = new DateTime(2017, 2, 28);
        //    DateTime endDate = startDate.AddDays(5);
        //    DateTime dueDate = startDate.AddDays(2);
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(dueDate)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.DateRange(startDate, endDate);
        //    Assert.That(board.InBox.ViewItems, Is.Not.Empty);
        //    Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void ViewItemFiltersActivityOnStartOfDateRange()
        //{
        //    var board = new ActivityBoard();
        //    DateTime startDate = new DateTime(2017, 2, 28);
        //    DateTime endDate = startDate.AddDays(5);
        //    DateTime dueDate = startDate;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(dueDate)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.DateRange(startDate, endDate);
        //    Assert.That(board.InBox.ViewItems, Is.Not.Empty);
        //    Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void ViewItemFiltersActivityOnEndOfDateRange()
        //{
        //    var board = new ActivityBoard();
        //    DateTime startDate = new DateTime(2017, 2, 28);
        //    DateTime endDate = startDate.AddDays(5);
        //    DateTime dueDate = endDate;
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(dueDate)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.DateRange(startDate, endDate);
        //    Assert.That(board.InBox.ViewItems, Is.Not.Empty);
        //    Assert.That(board.InBox.ViewItems.Count(), Is.EqualTo(1));
        //}

        //[Test]
        //public void ViewItemFiltersActivityBeforeStartOfDateRange()
        //{
        //    var board = new ActivityBoard();
        //    DateTime startDate = new DateTime(2017, 2, 28);
        //    DateTime endDate = startDate.AddDays(5);
        //    DateTime dueDate = startDate.AddDays(-1);
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(dueDate)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.DateRange(startDate, endDate);
        //    Assert.That(board.InBox.ViewItems, Is.Empty);
        //}

        //[Test]
        //public void ViewItemFiltersActivityAfterEndOfDateRange()
        //{
        //    var board = new ActivityBoard();
        //    DateTime startDate = new DateTime(2017, 2, 28);
        //    DateTime endDate = startDate.AddDays(5);
        //    DateTime dueDate = endDate.AddDays(1);
        //    Activity activity = Activity.FluentNew("New Activity")
        //        .ActiveDueDate(dueDate)
        //        .AddToBoard(board);
        //    board.InBox.AssignView.DateRange(startDate, endDate);
        //    Assert.That(board.InBox.ViewItems, Is.Empty);
        //}

        //[Test]
        //public void ViewItemFiltersIncludesHistoryDateRange()
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
