using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;
using PCLActivitySet.Domain.Views;

namespace PCLActivitySet.Test.Domain
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

            viewItem.Name.Should().Be(name);
        }

        [Test]
        public void DateSameAsItemCompletedDate()
        {
            DateTime date = DateTime.Now;
            var item = new ActivityHistoryItem() { CompletedDate = date };
            var viewItem = new HistoryViewItem(item, null);

            viewItem.Date.Should().Be(date);
        }

        [Test]
        public void IsActiveIsFalse()
        {
            var item = new ActivityHistoryItem();
            var viewItem = new HistoryViewItem(item, null);

            viewItem.IsActive.Should().BeFalse();
        }
    }
}
