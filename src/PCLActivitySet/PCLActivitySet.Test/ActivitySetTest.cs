using System.Linq;
using NUnit.Framework;

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
            Assert.That(activitySet.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanAddTwoActivities()
        {
            var activitySet = new ActivitySet();
            activitySet.Add(new Activity() { Name = "First New Activity" });
            activitySet.Add(new Activity() { Name = "Second New Activity" });
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddingSameActivityTwiceYieldsOneActivity()
        {
            var activitySet = new ActivitySet();
            var activity = new Activity() { Name = "New Activity" };
            activitySet.Add(activity);
            activitySet.Add(activity);
            Assert.That(activitySet.Any(), Is.True);
            Assert.That(activitySet.Count(), Is.EqualTo(1));
        }
    }
}
