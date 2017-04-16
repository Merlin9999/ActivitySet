using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;

namespace PCLActivitySet.Test.Domain
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
            item.Name.Should().Be(itemName);
            item.DueDate.Should().Be(dueDate);
            item.CompletedDate.Should().Be(completionDate);
        }

        [Test]
        public void NameIsNullByDefault()
        {
            var item = new ActivityHistoryItem();
            item.Name.Should().BeNull();
        }

        [Test]
        public void NameIsReadWrite()
        {
            string newName = "New Name";
            var item = new ActivityHistoryItem();
            item.Name = newName;
            item.Name.Should().Be(newName);
        }

        [Test]
        public void DueDateIsMinByDefault()
        {
            var item = new ActivityHistoryItem();
            item.DueDate.Should().BeNull();
        }

        [Test]
        public void DueDateIsReadWrite()
        {
            DateTime newDate = new DateTime(2017, 2, 28);
            var item = new ActivityHistoryItem();
            item.DueDate = newDate;
            item.DueDate.Should().Be(newDate);
        }

        [Test]
        public void CompletedDateIsMinByDefault()
        {
            var item = new ActivityHistoryItem();
            item.CompletedDate.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void CompletedDateIsReadWrite()
        {
            DateTime newDate = new DateTime(2017, 2, 28);
            var item = new ActivityHistoryItem();
            item.CompletedDate = newDate;
            item.CompletedDate.Should().Be(newDate);
        }
    }
}
