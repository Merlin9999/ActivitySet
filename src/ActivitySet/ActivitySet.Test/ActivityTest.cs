using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActivitySet.Test
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
