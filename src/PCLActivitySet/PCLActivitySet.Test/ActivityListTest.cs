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
        public void FilteringInactiveActivities()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ActivityFilter.FilterOutNonActive();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        }

        //[Test]
        //public void FilteringInactiveActivitiesWithFilterDelay()
        //{
        //    var board = new ActivityBoard();
        //    Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
        //    Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
        //    activity1.SignalCompleted(new DateTime(2017, 2, 28));
        //    board.InBox.ActivityFilter.FilterOutNonActiveWithDelay(TimeSpan.FromHours(4));

        //    Assert.That(board.InBox.Activities, Is.Not.Empty);
        //    Assert.That(board.InBox.Activities.Count(), Is.EqualTo(1));
        //}

        [Test]
        public void ClearingActivityFiltersShowsAllActivities()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("New Activity 1").AddToBoard(board);
            Activity activity2 = Activity.FluentNew("New Activity 2").AddToBoard(board);
            activity1.SignalCompleted(new DateTime(2017, 2, 28));
            board.InBox.ActivityFilter.FilterOutNonActive();
            board.InBox.ActivityFilter.Clear();

            Assert.That(board.InBox.Activities, Is.Not.Empty);
            Assert.That(board.InBox.Activities.Count(), Is.EqualTo(2));
        }

        //[Test]
        //public void UseIncludeInactiveActifityFilter()
        //{
        //    Assert.Fail("Finish Me!");
        //}




    }
}
