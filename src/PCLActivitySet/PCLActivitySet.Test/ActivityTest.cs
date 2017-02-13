using NUnit.Framework;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ActivityTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activity = new Activity();
            string testName = "Test Name";
            activity.Name = testName;
            Assert.That(activity.Name, Is.EqualTo(testName));
        }
    }
}
