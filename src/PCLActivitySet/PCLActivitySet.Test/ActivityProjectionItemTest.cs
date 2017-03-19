using NUnit.Framework;
using System;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ActivityProjectionItemTest
    {
        [Test]
        public void DefaultConstructorInitializesToDefaultValues()
        {
            var item = new ActivityProjectionItem();
            Assert.That(item.Name, Is.Null);
            Assert.That(item.DueDate, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void MainConstructorInitializesToPassedValues()
        {
            string name = "The Name";
            DateTime dueDate = new DateTime(2017, 5, 13);
            var item = new ActivityProjectionItem(name, dueDate);
            Assert.That(item.Name, Is.EqualTo(name));
            Assert.That(item.DueDate, Is.EqualTo(dueDate));
        }
    }
}
