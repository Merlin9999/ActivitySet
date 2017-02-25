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
    }
}
