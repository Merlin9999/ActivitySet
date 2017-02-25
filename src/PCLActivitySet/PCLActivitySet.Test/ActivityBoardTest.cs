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
            var activitySet = new ActivityBoard();
            Assert.That(activitySet.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activitySet = new ActivityBoard();
            string testName = "Test Name";
            activitySet.Name = testName;
            Assert.That(activitySet.Name, Is.EqualTo(testName));
        }

        [Test]
        public void CanAddActivity()
        {
            var activitySet = new ActivityBoard();
            activitySet.Add(new Activity() { Name = "New Activity" });
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(1));
        }

        [Test]
        public void CanAddTwoActivities()
        {
            var activitySet = new ActivityBoard();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddingSameActivityTwiceYieldsOneActivity()
        {
            var activitySet = new ActivityBoard();
            var activity = new Activity() { Name = "New Activity" };
            activitySet.Add(activity);
            activitySet.Add(activity);
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(1));
        }

        [Test]
        public void ClearRemovesAllActivities()
        {
            var activitySet = new ActivityBoard();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            activitySet.Clear();
            Assert.That(activitySet.Any(), Is.False);
            Assert.That(activitySet.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveRemovesOneActivityIfGuidMatches()
        {
            var activitySet = new ActivityBoard();
            var activity = new Activity() { Name = "First New Activity" };
            activitySet.Add(activity);
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            activitySet.Remove(activity);
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(1));
        }

        [Test]
        public void RemoveRemovesNoActivityIfGuidDoesntMatch()
        {
            var activitySet = new ActivityBoard();
            var activityToRemove = new Activity() { Name = "Activity to Remove" };
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            activitySet.Remove(activityToRemove);
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(2));
        }

        [Test]
        public void ContainsReturnsTrueWhenActivityGuidMatches()
        {
            var activitySet = new ActivityBoard();
            var activity = new Activity() { Name = "First New Activity" };
            activitySet.Add(activity);
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Contains(activity), Is.True);
        }

        [Test]
        public void ContainsReturnsFalseWhenActivityGuidDoesntMatch()
        {
            var activitySet = new ActivityBoard();
            var activity = new Activity() { Name = "Activity to not find" };
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Contains(activity), Is.False);
        }

        [Test]
        public void VerifyCopyTo()
        {
            var activitySet = new ActivityBoard();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            var activityArray = new Activity[2];
            activitySet.CopyTo(activityArray, 0);
            Assert.That(activitySet, Is.EquivalentTo(activityArray));
        }

        [Test]
        public void ReadOnlyReturnsFalse()
        {
            Assert.That(new ActivityBoard().IsReadOnly, Is.False);
        }

        [Test]
        public void ActivityAddedToInBoxListByDefault()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddTo(board)
                .ToActivity;
            Assert.That(board.InBox.Activities, Has.Member(activity));
        }

        [Test]
        public void CanCreateActivityList()
        {
            var board = new ActivityBoard();
            string activityListName = "Doing";
            ActivityList list = board.CreateList(activityListName);
            Assert.That(list.Name, Is.EqualTo(activityListName));
            Assert.That(board.ActivityLists, Has.Member(list));
        }

        [Test]
        public void CanMoveActivityToActivityList()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddTo(board)
                .ToActivity;
            ActivityList list = board.CreateList("Doing");
            board.Move(activity).To(list);
            Assert.That(list.Activities, Has.Member(activity));
            Assert.That(board.InBox.Activities, Has.No.Member(activity));
        }

        [Test]
        public void CannotMoveActivityToActivityListNotBelongingToBoard()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .ActiveDueDate(new DateTime(2017, 2, 28))
                .DailyLeadTime(3)
                .Recurrence(ERecurFromType.FromActiveDueDate, x => x.Daily(14))
                .AddTo(board)
                .ToActivity;
            ActivityList list = new ActivityList(board) {Name = "Bad Activity List"};
            Assert.That(() => board.Move(activity).To(list), Throws.TypeOf<ArgumentException>());
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
            ActivityList list = board.CreateList("Doing");
            Assert.That(() => board.Move(activity).To(list), Throws.TypeOf<ArgumentException>());
        }

    }
}
