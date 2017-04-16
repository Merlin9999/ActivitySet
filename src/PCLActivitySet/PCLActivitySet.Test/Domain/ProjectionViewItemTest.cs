using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;
using PCLActivitySet.Domain.Views;

namespace PCLActivitySet.Test.Domain
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

            viewItem.Name.Should().Be(name);
        }

        [Test]
        public void DateSameAsActivityItemDueDate()
        {
            DateTime date = DateTime.Now;
            var item = new ActivityProjectionItem() { DueDate = date };
            var viewItem = new ProjectionViewItem(item, null);

            viewItem.Date.Should().Be(date);
        }

        [Test]
        public void IsActiveIsFalse()
        {
            var item = new ActivityProjectionItem();
            var viewItem = new ProjectionViewItem(item, null);

            viewItem.IsActive.Should().BeFalse();
        }
    }
}
