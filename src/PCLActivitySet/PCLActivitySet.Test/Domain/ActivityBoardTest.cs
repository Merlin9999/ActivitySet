using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain
{

    [TestFixture]
    public class ActivityBoardTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activityBoard = new ActivityBoard();
            activityBoard.Name.Should().BeNull();
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activityBoard = new ActivityBoard();
            string testName = "Test Name";
            activityBoard.Name = testName;
            activityBoard.Name.Should().Be(testName);
        }

        [Test]
        public void CanAddActivity()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("New Activity").AddToBoard(activityBoard);
            activityBoard.UnfilteredActivities.Any().Should().BeTrue();
            activityBoard.UnfilteredActivities.Count().Should().Be(1);
        }

        [Test]
        public void CanAddTwoActivities()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.UnfilteredActivities.Any().Should().BeTrue();
            activityBoard.UnfilteredActivities.Count().Should().Be(2);
        }

        [Test]
        public void AddingSameActivityTwiceYieldsOneActivity()
        {
            var activityBoard = new ActivityBoard();
            var activity = new Activity() { Name = "New Activity" };
            Activity.FluentNew("New Activity")
                .AddToBoard(activityBoard)
                .AddToBoard(activityBoard);
            activityBoard.UnfilteredActivities.Any().Should().BeTrue();
            activityBoard.UnfilteredActivities.Count().Should().Be(1);
        }

        [Test]
        public void ClearRemovesAllActivities()
        {
            var activityBoard = new ActivityBoard();
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveAllActivities();
            activityBoard.UnfilteredActivities.Any().Should().BeFalse();
            activityBoard.UnfilteredActivities.Count().Should().Be(0);
        }

        [Test]
        public void RemoveRemovesOneActivityIfGuidMatches()
        {
            var activityBoard = new ActivityBoard();
            Activity activity = Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveActivity(activity);
            activityBoard.UnfilteredActivities.Any().Should().BeTrue();
            activityBoard.UnfilteredActivities.Count().Should().Be(1);
        }

        [Test]
        public void RemoveRemovesNoActivityIfGuidDoesntMatch()
        {
            var activityBoard = new ActivityBoard();
            var activityToRemove = new Activity() { Name = "Activity to Remove" };
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.RemoveActivity(activityToRemove);
            activityBoard.UnfilteredActivities.Any().Should().BeTrue();
            activityBoard.UnfilteredActivities.Count().Should().Be(2);
        }

        [Test]
        public void ContainsReturnsTrueWhenActivityGuidMatches()
        {
            var activityBoard = new ActivityBoard();
            Activity activity = Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.ContainsActivity(activity).Should().BeTrue();
        }

        [Test]
        public void ContainsReturnsFalseWhenActivityGuidDoesntMatch()
        {
            var activityBoard = new ActivityBoard();
            var activity = new Activity() { Name = "Activity to not find" };
            Activity.FluentNew("First New Activity").AddToBoard(activityBoard);
            Activity.FluentNew("Second New Activity").AddToBoard(activityBoard);
            activityBoard.ContainsActivity(activity).Should().BeFalse();
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
            board.InBox.Activities.Should().Contain(activity);
        }

        [Test]
        public void CanCreateActivityList()
        {
            var board = new ActivityBoard();
            string activityListName = "Doing";
            ActivityList list = board.AddNewList(activityListName);
            list.Name.Should().Be(activityListName);
            board.ActivityLists.Should().Contain(list);
        }

        [Test]
        public void CanRemoveActivityList()
        {
            var board = new ActivityBoard();
            string activityListName = "Doing";
            ActivityList list = board.AddNewList(activityListName);
            board.RemoveList(list);
            board.ActivityLists.Should().BeEmpty();
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
            board.InBox.Activities.Should().NotBeEmpty();
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
            list.Activities.Should().Contain(activity);
            board.InBox.Activities.Should().NotContain(activity);
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
            list.Activities.Should().NotContain(activity);
            board.InBox.Activities.Should().Contain(activity);
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
            Action action = () => board.MoveActivity(activity).ToList(list);
            action.ShouldThrow<ArgumentException>();
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
            Action action = () => board.MoveActivity(activity).ToList(list);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CanCreateContext()
        {
            var board = new ActivityBoard();
            board.AddNewContext("Context Name");
            board.Contexts.Should().NotBeEmpty();
        }

        [Test]
        public void CanRemoveContext()
        {
            var board = new ActivityBoard();
            ActivityContext context = board.AddNewContext("Context Name");
            board.RemoveContext(context);
            board.Contexts.Should().BeEmpty();
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

            activity.ContextGuids.Should().BeEmpty();
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

            board.SelectedContextGuids.Should().NotBeEmpty();
            board.SelectedContextGuids.Count().Should().Be(2);
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

            board.SelectedContextGuids.Should().NotBeEmpty();
            board.SelectedContextGuids.Count().Should().Be(3);
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

            activity.ContextGuids.Should().BeEmpty();
            board.SelectedContextGuids.Should().NotBeEmpty();
            board.Activities.Should().NotBeEmpty();
            board.Activities.Count().Should().Be(1);
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

            activity.ContextGuids.Should().NotBeEmpty();
            board.SelectedContextGuids.Should().BeEmpty();
            board.Activities.Should().NotBeEmpty();
            board.Activities.Count().Should().Be(1);
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

            activity.ContextGuids.Should().NotBeEmpty();
            board.SelectedContextGuids.Should().NotBeEmpty();
            board.Activities.Should().NotBeEmpty();
            board.Activities.Count().Should().Be(1);
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

            activity.ContextGuids.Should().NotBeEmpty();
            board.SelectedContextGuids.Should().NotBeEmpty();
            board.Activities.Should().BeEmpty();
        }

        [Test]
        public void CanCreateGoal()
        {
            var board = new ActivityBoard();
            string goalName = "New Goal";
            ActivityGoal goal = board.AddNewGoal(goalName);

            goal.Should().NotBeNull();
            goal.Name.Should().Be(goalName);
            board.Goals.Should().NotBeNull();
            board.Goals.Count().Should().Be(1);
            board.Goals.Should().Contain(goal);
        }

        [Test]
        public void CanRemoveGoal()
        {
            var board = new ActivityBoard();
            ActivityGoal goal = board.AddNewGoal("New Goal");
            board.RemoveGoal(goal);
            board.Goals.Should().BeEmpty();
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

            activity.GoalGuid.Should().NotBeNull();
            activity.GoalGuid.Should().Be(goal.Guid);
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

            activity.GoalGuid.Should().BeNull();
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

            activity.GoalGuid.Should().BeNull();
        }

        [Test]
        public void CannotMoveActivityToGoalNotBelongingToBoard()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            ActivityGoal goal = new ActivityGoal("Bad Goal");
            Action action = () => board.MoveActivity(activity).ToGoal(goal);
            action.ShouldThrow<ArgumentException>();
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
            
            foundGoal.Should().Be(newGoal);
        }

        [Test]
        public void GetGoalFromActivityWhenNoGoalIsAssigned()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            ActivityGoal foundGoal = board.GetGoalFromActivity(activity);

            foundGoal.Should().BeNull();
        }

        [Test]
        public void GetGoalFromActivityWhenGoalGuidIsInvalid()
        {
            var board = new ActivityBoard();
            Activity activity = Activity.FluentNew("An Activity")
                .AddToBoard(board);
            activity.GoalGuid = Guid.NewGuid();
            Action action = () => board.GetGoalFromActivity(activity);
            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetGoalsFromActivities()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("Activity 1")
                .AddToBoard(board);
            Activity activity2 = Activity.FluentNew("Activity 2")
               .AddToBoard(board);
            ActivityGoal goal1 = board.AddNewGoal("Goal 1");
            board.MoveActivity(activity1).ToGoal(goal1);
            board.MoveActivity(activity2).ToGoal(goal1);
            ILookup<ActivityGoal, Activity> goalLookup = board.GetGoalLookupFromActivities(activity1, activity2);

            goalLookup.Should().NotBeEmpty();
            goalLookup.Count.Should().Be(1);
            goalLookup.First().Key.Should().Be(goal1);
            goalLookup.First().Count().Should().Be(2);
            goalLookup.First().FirstOrDefault(activity => activity == activity1).Should().NotBeNull();
            goalLookup.First().FirstOrDefault(activity => activity == activity2).Should().NotBeNull();
        }

        [Test]
        public void GetGoalsFromActivitiesConsolidatesActivities()
        {
            var board = new ActivityBoard();
            Activity activity1 = Activity.FluentNew("Activity 1")
                .AddToBoard(board);
            ActivityGoal goal1 = board.AddNewGoal("Goal 1");
            board.MoveActivity(activity1).ToGoal(goal1);
            ILookup<ActivityGoal, Activity> goalLookup = board.GetGoalLookupFromActivities(activity1, activity1);

            goalLookup.Should().NotBeEmpty();
            goalLookup.Count.Should().Be(1);
            goalLookup.First().Key.Should().Be(goal1);
            goalLookup.First().Count().Should().Be(1);
            goalLookup.First().FirstOrDefault(activity => activity == activity1).Should().NotBeNull();
        }

    }
}
