using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test
{

    [TestFixture]
    public class ActivityBoardTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activityBoard = new ActivityBoard();
            Assert.That(activityBoard.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activityBoard = new ActivityBoard();
            string testName = "Test Name";
            activityBoard.Name = testName;
            Assert.That(activityBoard.Name, Is.EqualTo(testName));
        }

        [Test]
        public void CanAddActivity()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("New Activity").AddToBoard(activityBoard);
            Assert.That(activityBoard.Activities.Any(), Is.True);
            Assert.That(activityBoard.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanAddTwoActivities()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            Assert.That(activityBoard.Activities.Any(), Is.True);
            Assert.That(activityBoard.Activities.Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddingSameActivityTwiceYieldsOneActivity()
        {
            var activityBoard = new ActivityBoard();
            var activity = new Activity() { Name = "New Activity" };
            Activity.FluentNew("New Activity")
                .AddToBoard(activityBoard)
                .AddToBoard(activityBoard);
            Assert.That(activityBoard.Activities.Any(), Is.True);
            Assert.That(activityBoard.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ClearRemovesAllActivities()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveAllActivities();
            Assert.That(activityBoard.Activities.Any(), Is.False);
            Assert.That(activityBoard.Activities.Count(), Is.EqualTo(0));
        }

        [Test]
        public void RemoveRemovesOneActivityIfGuidMatches()
        {
            var activityBoard = new ActivityBoard();
            Activity activity = Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveActivity(activity);
            Assert.That(activityBoard.Activities.Any(), Is.True);
            Assert.That(activityBoard.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void RemoveRemovesNoActivityIfGuidDoesntMatch()
        {
            var activityBoard = new ActivityBoard();
            var activityToRemove = new Activity() { Name = "Activity to Remove" };
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveActivity(activityToRemove);
            Assert.That(activityBoard.Activities.Any(), Is.True);
            Assert.That(activityBoard.Activities.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ContainsReturnsTrueWhenActivityGuidMatches()
        {
            var activityBoard = new ActivityBoard();
            Activity activity = Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            Assert.That(activityBoard.ContainsActivity(activity), Is.True);
        }

        [Test]
        public void ContainsReturnsFalseWhenActivityGuidDoesntMatch()
        {
            var activityBoard = new ActivityBoard();
            var activity = new Activity() { Name = "Activity to not find" };
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            Assert.That(activityBoard.ContainsActivity(activity), Is.False);
        }

        [Test]
        public void ActivityAddedToInBoxListByDefault()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddToBoard(board)
                .ToActivity;
            Assert.That(board.InBox.Activities, Has.Member(activity));
        }

        [Test]
        public void CanCreateActivityList()
        {
            var board = new ActivityBoard();
            string activityListName = "Doing";
            ActivityList list = board.AddNewList(activityListName);
            Assert.That(list.Name, Is.EqualTo(activityListName));
            Assert.That(board.ActivityLists, Has.Member(list));
        }

        [Test]
        public void CanRemoveActivityList()
        {
            var board = new ActivityBoard();
            string activityListName = "Doing";
            ActivityList list = board.AddNewList(activityListName);
            board.RemoveList(list);
            Assert.That(board.ActivityLists, Is.Empty);
        }

        [Test]
        public void RemovingActivityListMovesRelatedActivitiesToInBox()
        {
            var board = new ActivityBoard();
            string activityListName = "Doing";
            ActivityList list = board.AddNewList(activityListName);
            Activity activity = Activity.FluentNew("New Activity").AddToBoard(board);
            board.MoveActivity(activity).ToList(list);
            board.RemoveList(list);
            Assert.That(board.InBox.Activities, Is.Not.Empty);
        }
        [Test]
        public void CanMoveActivityToActivityList()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddToBoard(board)
                .ToActivity;
            ActivityList list = board.AddNewList("Doing");
            board.MoveActivity(activity).ToList(list);
            Assert.That(list.Activities, Has.Member(activity));
            Assert.That(board.InBox.Activities, Has.No.Member(activity));
        }

        [Test]
        public void CanMoveActivityBackToInBox()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddToBoard(board)
                .ToActivity;
            ActivityList list = board.AddNewList("Doing");
            board.MoveActivity(activity).ToList(list);
            board.MoveActivity(activity).ToList(board.InBox);
            Assert.That(list.Activities, Has.No.Member(activity));
            Assert.That(board.InBox.Activities, Has.Member(activity));
        }

        [Test]
        public void CannotMoveActivityToActivityListNotBelongingToBoard()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddToBoard(board)
                .ToActivity;
            ActivityList list = new ActivityList(board) {Name = "Bad Activity List"};
            Assert.That(() => board.MoveActivity(activity).ToList(list), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CannotMoveActivityNotBelongingToBoardToActivityList()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .ToActivity;
            ActivityList list = board.AddNewList("Doing");
            Assert.That(() => board.MoveActivity(activity).ToList(list), Throws.TypeOf<ArgumentException>());
        }

    }
}
