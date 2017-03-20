using NUnit.Framework;
using PCLActivitySet.Views;
using System;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class HistoryViewItemTest
    {
        [Test]
        public void NameSameAsItemName()
        {
            string name = "Test Activity";
            var item = new ActivityHistoryItem() { Name = name };
            var viewItem = new HistoryViewItem(item, null);

            Assert.That(viewItem.Name, Is.EqualTo(name));
        }

        [Test]
        public void DateSameAsItemCompletedDate()
        {
            DateTime date = DateTime.Now;
            var item = new ActivityHistoryItem() { CompletedDate = date };
            var viewItem = new HistoryViewItem(item, null);

            Assert.That(viewItem.Date, Is.EqualTo(date));
        }

        [Test]
        public void IsActiveIsFalse()
        {
            var item = new ActivityHistoryItem();
            var viewItem = new HistoryViewItem(item, null);

            Assert.That(viewItem.IsActive, Is.False);
        }
    }
}
