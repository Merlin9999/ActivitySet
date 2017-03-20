using NUnit.Framework;
using System;
using System.Linq;
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
            Assert.That(activityBoard.UnfilteredActivities.Any(), Is.True);
            Assert.That(activityBoard.UnfilteredActivities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanAddTwoActivities()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            Assert.That(activityBoard.UnfilteredActivities.Any(), Is.True);
            Assert.That(activityBoard.UnfilteredActivities.Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddingSameActivityTwiceYieldsOneActivity()
        {
            var activityBoard = new ActivityBoard();
            var activity = new Activity() { Name = "New Activity" };
            Activity.FluentNew("New Activity")
                .AddToBoard(activityBoard)
                .AddToBoard(activityBoard);
            Assert.That(activityBoard.UnfilteredActivities.Any(), Is.True);
            Assert.That(activityBoard.UnfilteredActivities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ClearRemovesAllActivities()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveAllActivities();
            Assert.That(activityBoard.UnfilteredActivities.Any(), Is.False);
            Assert.That(activityBoard.UnfilteredActivities.Count(), Is.EqualTo(0));
        }

        [Test]
        public void RemoveRemovesOneActivityIfGuidMatches()
        {
            var activityBoard = new ActivityBoard();
            Activity activity = Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveActivity(activity);
            Assert.That(activityBoard.UnfilteredActivities.Any(), Is.True);
            Assert.That(activityBoard.UnfilteredActivities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void RemoveRemovesNoActivityIfGuidDoesntMatch()
        {
            var activityBoard = new ActivityBoard();
            var activityToRemove = new Activity() { Name = "Activity to Remove" };
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveActivity(activityToRemove);
            Assert.That(activityBoard.UnfilteredActivities.Any(), Is.True);
            Assert.That(activityBoard.UnfilteredActivities.Count(), Is.EqualTo(2));
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
        public void CannotMoveActivityNotBelongingToBoard()
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

        [Test]
        public void CanCreateContext()
        {
            var board = new ActivityBoard();
            board.AddNewContext("Context Name");
            Assert.That(board.Contexts, Is.Not.Empty);
        }

        [Test]
        public void CanRemoveContext()
        {
            var board = new ActivityBoard();
            ActivityContext context = board.AddNewContext("Context Name");
            board.RemoveContext(context);
            Assert.That(board.Contexts, Is.Empty);
        }

        [Test]
        public void RemovingContextsRemovesThoseContextsFromActivities()
        {
            var board = new ActivityBoard();
            ActivityContext context1 = board.AddNewContext("Context 1");
            ActivityContext context2 = board.AddNewContext("Context 2");
            board.AddSelectedContexts(context1, context2);
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board);
            activity.AddContexts(context2);
            board.RemoveContext(context2);

            Assert.That(activity.ContextGuids, Is.Empty);
        }

        [Test]
        public void CanSelectContexts()
        {
            var board = new ActivityBoard();
            board.AddNewContext("Context 1");
            board.AddNewContext("Context 2");
            board.AddNewContext("Context 3");
            ActivityContext context4 = board.AddNewContext("Context 4");
            ActivityContext context5 = board.AddNewContext("Context 5");
            board.AddSelectedContexts(context4, context5);

            Assert.That(board.SelectedContextGuids, Is.Not.Empty);
            Assert.That(board.SelectedContextGuids.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanDeselectContexts()
        {
            var board = new ActivityBoard();
            ActivityContext context1 = board.AddNewContext("Context 1");
            ActivityContext context2 = board.AddNewContext("Context 2");
            ActivityContext context3 = board.AddNewContext("Context 3");
            ActivityContext context4 = board.AddNewContext("Context 4");
            ActivityContext context5 = board.AddNewContext("Context 5");
            board.AddSelectedContexts(context1, context2, context3, context4, context5);
            board.RemoveSelectedContexts(context2, context4);

            Assert.That(board.SelectedContextGuids, Is.Not.Empty);
            Assert.That(board.SelectedContextGuids.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ActivityWithEmptyContextsShows()
        {
            var board = new ActivityBoard();
            ActivityContext context1 = board.AddNewContext("Context 1");
            ActivityContext context2 = board.AddNewContext("Context 2");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board);
            board.AddSelectedContexts(context1);

            Assert.That(activity.ContextGuids, Is.Empty);
            Assert.That(board.SelectedContextGuids, Is.Not.Empty);
            Assert.That(board.Activities, Is.Not.Empty);
            Assert.That(board.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ActivityWithContextShowsWhenNoContextSelected()
        {
            var board = new ActivityBoard();
            ActivityContext context1 = board.AddNewContext("Context 1");
            ActivityContext context2 = board.AddNewContext("Context 2");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board)
                .Contexts(context1);

            Assert.That(activity.ContextGuids, Is.Not.Empty);
            Assert.That(board.SelectedContextGuids, Is.Empty);
            Assert.That(board.Activities, Is.Not.Empty);
            Assert.That(board.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ActivityWithContextMatchingSelectedContextShows()
        {
            var board = new ActivityBoard();
            ActivityContext context1 = board.AddNewContext("Context 1");
            ActivityContext context2 = board.AddNewContext("Context 2");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board)
                .Contexts(context1);
            board.AddSelectedContexts(context1);

            Assert.That(activity.ContextGuids, Is.Not.Empty);
            Assert.That(board.SelectedContextGuids, Is.Not.Empty);
            Assert.That(board.Activities, Is.Not.Empty);
            Assert.That(board.Activities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ActivityWithContextNotMatchingSelectedContextDoesNotShow()
        {
            var board = new ActivityBoard();
            ActivityContext context1 = board.AddNewContext("Context 1");
            ActivityContext context2 = board.AddNewContext("Context 2");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board)
                .Contexts(context1);
            board.AddSelectedContexts(context2);

            Assert.That(activity.ContextGuids, Is.Not.Empty);
            Assert.That(board.SelectedContextGuids, Is.Not.Empty);
            Assert.That(board.Activities, Is.Empty);
        }

        [Test]
        public void CanCreateGoal()
        {
            var board = new ActivityBoard();
            string goalName = "New Goal";
            ActivityGoal goal = board.AddNewGoal(goalName);

            Assert.That(goal, Is.Not.Null);
            Assert.That(goal.Name, Is.EqualTo(goalName));
            Assert.That(board.Goals, Is.Not.Null);
            Assert.That(board.Goals.Count(), Is.EqualTo(1));
            Assert.That(board.Goals, Has.Member(goal));
        }

        [Test]
        public void CanRemoveGoal()
        {
            var board = new ActivityBoard();
            ActivityGoal goal = board.AddNewGoal("New Goal");
            board.RemoveGoal(goal);
            Assert.That(board.Goals, Is.Empty);
        }

        [Test]
        public void CanAssignActivityToGoal()
        {
            var board = new ActivityBoard();
            ActivityGoal goal = board.AddNewGoal("New Goal");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board);
            board.MoveActivity(activity)
                .ToGoal(goal);

            Assert.That(activity.GoalGuid, Is.Not.Null);
            Assert.That(activity.GoalGuid, Is.EqualTo(goal.Guid));
        }

        [Test]
        public void CanRemoveGoalFromActivity()
        {
            var board = new ActivityBoard();
            ActivityGoal goal = board.AddNewGoal("New Goal");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board);
            board.MoveActivity(activity)
                .ToGoal(goal)
                .ToGoal(null);

            Assert.That(activity.GoalGuid, Is.Null);
        }

        [Test]
        public void RemovingGoalClearsGoalFromActivities()
        {
            var board = new ActivityBoard();
            ActivityGoal goal = board.AddNewGoal("New Goal");
            Activity activity = Activity.FluentNew("New Activity")
                .AddToBoard(board);
            board.MoveActivity(activity)
                .ToGoal(goal);
            board.RemoveGoal(goal);

            Assert.That(activity.GoalGuid, Is.Null);
        }

        [Test]
        public void CannotMoveActivityToGoalNotBelongingToBoard()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            ActivityGoal goal = new ActivityGoal("Bad Goal");
            Assert.That(() => board.MoveActivity(activity).ToGoal(goal), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetGoalFromActivity()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            ActivityGoal newGoal = board.AddNewGoal("New Goal");
            board.MoveActivity(activity).ToGoal(newGoal);
            ActivityGoal foundGoal = board.GetGoalFromActivity(activity);
            
            Assert.That(foundGoal, Is.EqualTo(newGoal));
        }

        [Test]
        public void GetGoalFromActivityWhenNoGoalIsAssigned()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            ActivityGoal foundGoal = board.GetGoalFromActivity(activity);

            Assert.That(foundGoal, Is.Null);
        }

        [Test]
        public void GetGoalFromActivityWhenGoalGuidIsInvalid()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            activity.GoalGuid = Guid.NewGuid();
            Assert.That(() => board.GetGoalFromActivity(activity), Throws.TypeOf<InvalidOperationException>());
        }
    }
}
