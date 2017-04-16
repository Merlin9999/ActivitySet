using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;
using PCLActivitySet.Domain.Views;

namespace PCLActivitySet.Test.Domain
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

            viewItem.Name.Should().Be(activityName);
        }

        [Test]
        public void DateSameAsActivityActiveDueDate()
        {
            DateTime activityActiveDueDate = DateTime.Now;
            Activity activity = Activity.FluentNew("Activity Name").ActiveDueDate(activityActiveDueDate);
            var viewItem = new ActivityViewItem(activity);

            viewItem.Date.Should().Be(activityActiveDueDate);
        }

        [Test]
        public void IsActiveSameAsActivityIsActive()
        {
            DateTime activityActiveDueDate = DateTime.Now;
            Activity activity = Activity.FluentNew("Activity Name").ActiveDueDate(activityActiveDueDate);
            var viewItem = new ActivityViewItem(activity);

            viewItem.IsActive.Should().Be(activity.IsActive);
        }
    }
}
