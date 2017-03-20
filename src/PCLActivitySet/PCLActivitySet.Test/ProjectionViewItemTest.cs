using NUnit.Framework;
using PCLActivitySet.Views;
using System;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ProjectionViewItemTest
    {
        [Test]
        public void NameSameAsItemName()
        {
            string name = "Test Activity";
            var item = new ActivityProjectionItem() { Name = name };
            var viewItem = new ProjectionViewItem(item, null);

            Assert.That(viewItem.Name, Is.EqualTo(name));
        }

        [Test]
        public void DateSameAsActivityItemDueDate()
        {
            DateTime date = DateTime.Now;
            var item = new ActivityProjectionItem() { DueDate = date };
            var viewItem = new ProjectionViewItem(item, null);

            Assert.That(viewItem.Date, Is.EqualTo(date));
        }

        [Test]
        public void IsActiveIsFalse()
        {
            var item = new ActivityProjectionItem();
            var viewItem = new ProjectionViewItem(item, null);

            Assert.That(viewItem.IsActive, Is.False);
        }
    }
}
