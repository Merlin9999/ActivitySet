using System;
using System.Linq;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test
{

    [TestFixture]
    public class ActivitySetTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activitySet = new ActivitySet();
            Assert.That(activitySet.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activitySet = new ActivitySet();
            string testName = "Test Name";
            activitySet.Name = testName;
            Assert.That(activitySet.Name, Is.EqualTo(testName));
        }

        [Test]
        public void CanAddActivity()
        {
            var activitySet = new ActivitySet();
            activitySet.Add(new Activity() { Name = "New Activity" });
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(1));
        }

        [Test]
        public void CanAddTwoActivities()
        {
            var activitySet = new ActivitySet();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddingSameActivityTwiceYieldsOneActivity()
        {
            var activitySet = new ActivitySet();
            var activity = new Activity() { Name = "New Activity" };
            activitySet.Add(activity);
            activitySet.Add(activity);
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count, Is.EqualTo(1));
        }

        [Test]
        public void ClearRemovesAllActivities()
        {
            var activitySet = new ActivitySet();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            activitySet.Clear();
            Assert.That(activitySet.Any(), Is.False);
            Assert.That(activitySet.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveRemovesOneActivityIfGuidMatches()
        {
            var activitySet = new ActivitySet();
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
            var activitySet = new ActivitySet();
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
            var activitySet = new ActivitySet();
            var activity = new Activity() { Name = "First New Activity" };
            activitySet.Add(activity);
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Contains(activity), Is.True);
        }

        [Test]
        public void ContainsReturnsFalseWhenActivityGuidDoesntMatch()
        {
            var activitySet = new ActivitySet();
            var activity = new Activity() { Name = "Activity to not find" };
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Contains(activity), Is.False);
        }

        [Test]
        public void VerifyCopyTo()
        {
            var activitySet = new ActivitySet();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            var activityArray = new Activity[2];
            activitySet.CopyTo(activityArray, 0);
            Assert.That(activitySet, Is.EquivalentTo(activityArray));
        }

        [Test]
        public void ReadOnlyReturnsFalse()
        {
            Assert.That(new ActivitySet().IsReadOnly, Is.False);
        }
    }
}
