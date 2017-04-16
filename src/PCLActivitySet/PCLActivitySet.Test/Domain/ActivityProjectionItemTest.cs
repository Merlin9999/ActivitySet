using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain;

namespace PCLActivitySet.Test.Domain
{
    [TestFixture]
    public class ActivityProjectionItemTest
    {
        [Test]
        public void DefaultConstructorInitializesToDefaultValues()
        {
            var item = new ActivityProjectionItem();
            item.Name.Should().BeNull();
            item.DueDate.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void MainConstructorInitializesToPassedValues()
        {
            string name = "The Name";
            DateTime dueDate = new DateTime(2017, 5, 13);
            var item = new ActivityProjectionItem(name, dueDate);
            item.Name.Should().Be(name);
            item.DueDate.Should().Be(dueDate);
        }
    }
}
