using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ActivityViewItemTest
    {
        [Test]
        public void NameSameAsActivityName()
        {
            string activityName = "Test Activity";
            Activity activity = Activity.FluentNew(activityName);
            var viewItem = new ActivityViewItem(activity);

            Assert.That(viewItem.Name, Is.EqualTo(activityName));
        }

        [Test]
        public void DateSameAsActivityActiveDueDate()
        {
            DateTime activityActiveDueDate = DateTime.Now;
            Activity activity = Activity.FluentNew("Activity Name").ActiveDueDate(activityActiveDueDate);
            var viewItem = new ActivityViewItem(activity);

            Assert.That(viewItem.Date, Is.EqualTo(activityActiveDueDate));
        }

        [Test]
        public void IsActiveSameAsActivityIsActive()
        {
            DateTime activityActiveDueDate = DateTime.Now;
            Activity activity = Activity.FluentNew("Activity Name").ActiveDueDate(activityActiveDueDate);
            var viewItem = new ActivityViewItem(activity);

            Assert.That(viewItem.IsActive, Is.EqualTo(activity.IsActive));
        }
    }
}
