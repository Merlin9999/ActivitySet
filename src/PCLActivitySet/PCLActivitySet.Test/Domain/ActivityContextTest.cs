using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;

namespace PCLActivitySet.Test.Domain
{
    [TestFixture]
    class ActivityContextTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activityList = new ActivityContext();
            activityList.Name.Should().BeNull();
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activityList = new ActivityContext();
            string testName = "Test Name";
            activityList.Name = testName;
            activityList.Name.Should().Be(testName);
        }
    }
}
