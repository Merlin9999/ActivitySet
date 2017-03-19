using NUnit.Framework;
using System;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ActivityHistoryItemTest
    {
        [Test]
        public void FullConstructorFullyInitializesProperties()
        {
            string itemName = "Item Name";
            DateTime dueDate = new DateTime(2017, 2, 28);
            DateTime completionDate = new DateTime(2017, 2, 14);
            var item = new ActivityHistoryItem(itemName, dueDate, completionDate);
            Assert.That(item.Name, Is.EqualTo(itemName));
            Assert.That(item.DueDate, Is.EqualTo(dueDate));
            Assert.That(item.CompletedDate, Is.EqualTo(completionDate));
        }

        [Test]
        public void NameIsNullByDefault()
        {
            var item = new ActivityHistoryItem();
            Assert.That(item.Name, Is.Null);
        }

        [Test]
        public void NameIsReadWrite()
        {
            string newName = "New Name";
            var item = new ActivityHistoryItem();
            item.Name = newName;
            Assert.That(item.Name, Is.EqualTo(newName));
        }

        [Test]
        public void DueDateIsMinByDefault()
        {
            var item = new ActivityHistoryItem();
            Assert.That(item.DueDate, Is.Null);
        }

        [Test]
        public void DueDateIsReadWrite()
        {
            DateTime newDate = new DateTime(2017, 2, 28);
            var item = new ActivityHistoryItem();
            item.DueDate = newDate;
            Assert.That(item.DueDate, Is.EqualTo(newDate));
        }

        [Test]
        public void CompletedDateIsMinByDefault()
        {
            var item = new ActivityHistoryItem();
            Assert.That(item.CompletedDate, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void CompletedDateIsReadWrite()
        {
            DateTime newDate = new DateTime(2017, 2, 28);
            var item = new ActivityHistoryItem();
            item.CompletedDate = newDate;
            Assert.That(item.CompletedDate, Is.EqualTo(newDate));
        }
    }
}
