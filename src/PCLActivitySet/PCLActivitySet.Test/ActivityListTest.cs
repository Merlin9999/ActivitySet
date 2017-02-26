using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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

        [Test]
        public void ClearingActivityFiltersShowsAllActivities()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ActivityFilters.ExcludeNonActive();
            board.InBox.ActivityFilters.Clear();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetFilterThatWasApplied()
        {
            var board = new ActivityBoard();
            ExcludeNonActiveWithDelayFilter filter =
                board.InBox.ActivityFilters
                    .ExcludeNonActiveWithDelay(TimeSpan.FromHours(4))
                    .GetExcludeNonActiveWithDelay();

            Assert.That(filter, Is.Not.Null);
            Assert.That(filter.Delay, Is.EqualTo(TimeSpan.FromHours(4)));
        }

        [Test]
        public void GetFilterThatWasNotApplied()
        {
            var board = new ActivityBoard();
            ExcludeNonActiveWithDelayFilter filter =
                board.InBox.ActivityFilters
                    .ExcludeNonActive()
                    .GetExcludeNonActiveWithDelay();

            Assert.That(filter, Is.Null);
        }

        [Test]
        public void OverwriteFilterWithUpdatedFilterOfSameType()
        {
            var board = new ActivityBoard();
            ExcludeNonActiveWithDelayFilter filter = board.InBox.ActivityFilters
                .ExcludeNonActiveWithDelay(TimeSpan.FromMinutes(10))
                .FocusDateRadar()
                .ExcludeNonActiveWithDelay(TimeSpan.FromHours(4))
                .GetExcludeNonActiveWithDelay();
            Assert.That(filter.Delay, Is.Not.EqualTo(TimeSpan.FromMinutes(10)));
            Assert.That(filter.Delay, Is.EqualTo(TimeSpan.FromHours(4)));
        }

        [Test]
        public void FilteringInactiveActivities()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ActivityFilters.ExcludeNonActive();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilteringInactiveActivitiesWithFilterDelayNotExpired()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ActivityFilters.ExcludeNonActiveWithDelay(TimeSpan.FromHours(4));

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));
        }

        [Test]
        public void FilteringInactiveActivitiesWithFilterDelayExpired()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ActivityFilters.ExcludeNonActiveWithDelay(TimeSpan.FromMilliseconds(1));
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2));

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilteringFocusDateRadarWhenFocusDateSameAsActiveDueDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            board.InBox.FocusDateTime = focusDateTime;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime).AddToBoard(board);
            board.InBox.ActivityFilters.FocusDateRadar();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilteringFocusDateRadarWhenFocusDateAfterActiveDueDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            board.InBox.FocusDateTime = focusDateTime;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(-1))
                .AddToBoard(board);
            board.InBox.ActivityFilters.FocusDateRadar();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilteringFocusDateRadarWhenFocusDateBeforeActiveDueDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            board.InBox.FocusDateTime = focusDateTime;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(1))
                .AddToBoard(board);
            board.InBox.ActivityFilters.FocusDateRadar();

            Assert.That(board.InBox.Activities, Is.Empty);
        }

        [Test]
        public void FilteringFocusDateRadarWhenFocusDateSameAsLeadTimeDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            board.InBox.FocusDateTime = focusDateTime;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(1))
                .DailyLeadTime(1)
                .AddToBoard(board);
            board.InBox.ActivityFilters.FocusDateRadar();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilteringFocusDateRadarWhenFocusDateAfterLeadTimeDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            board.InBox.FocusDateTime = focusDateTime;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(1))
                .DailyLeadTime(2)
                .AddToBoard(board);
            board.InBox.ActivityFilters.FocusDateRadar();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilteringFocusDateRadarWhenFocusDateBeforeLeadTimeDate()
        {
            var board = new ActivityBoard();
            DateTime focusDateTime = new DateTime(2017, 2, 28);
            board.InBox.FocusDateTime = focusDateTime;
            Activity activity = Activity.FluentNew("New Activity")
                .ActiveDueDate(focusDateTime.AddDays(2))
                .DailyLeadTime(1)
                .AddToBoard(board);
            board.InBox.ActivityFilters.FocusDateRadar();

            Assert.That(board.InBox.Activities, Is.Empty);
        }

    }
}
