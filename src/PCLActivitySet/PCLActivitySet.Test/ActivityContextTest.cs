using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PCLActivitySet.Test
{
    [TestFixture]
    class ActivityContextTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activityList = new ActivityContext();
            Assert.That(activityList.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activityList = new ActivityContext();
            string testName = "Test Name";
            activityList.Name = testName;
            Assert.That(activityList.Name, Is.EqualTo(testName));
        }
    }
}
