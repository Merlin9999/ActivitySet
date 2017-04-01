using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PCLActivitySet.Test.Helpers.Test
{
    [TestFixture]
    public class CollectionComparerTest
    {
        [Test]
        public void FirstListIsNull()
        {
            List<TestListItem> list1 = null;
            List<TestListItem> list2 = new List<TestListItem>();

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void SecondListIsNull()
        {
            List<TestListItem> list1 = new List<TestListItem>();
            List<TestListItem> list2 = null;

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void BothListsAreNull()
        {
            List<TestListItem> list1 = null;
            List<TestListItem> list2 = null;

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.True);
        }

        [Test]
        public void EmptyLists()
        {
            var list1 = new List<TestListItem>();
            var list2 = new List<TestListItem>();

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.True);
        }

        [Test]
        public void EqualListsWith1Item()
        {
            var list1 = new List<TestListItem>() { new TestListItem() { Name = "Thingy" } };
            var list2 = new List<TestListItem>() { new TestListItem() { Name = "Thingy" } };

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.True);
        }

        [Test]
        public void EqualListsWith2Items()
        {
            var list1 = new List<TestListItem>()
            {
                new TestListItem() {Name = "Thingy1"},
                new TestListItem() {Name = "Thingy2"}
            };
            var list2 = new List<TestListItem>()
            {
                new TestListItem() {Name = "Thingy1"},
                new TestListItem() {Name = "Thingy2"}
            };

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.True);
        }

        [Test]
        public void NonEqualListsWith1Item()
        {
            var list1 = new List<TestListItem>() { new TestListItem() { Name = "Thingy1" } };
            var list2 = new List<TestListItem>() { new TestListItem() { Name = "Thingy2" } };

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void EmptyListAndNonEmptyList()
        {
            var list1 = new List<TestListItem>();
            var list2 = new List<TestListItem>() { new TestListItem() { Name = "Thingy2" } };

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void NonEmptyListAndEmptyList()
        {
            var list1 = new List<TestListItem>() { new TestListItem() { Name = "Thingy1" } };
            var list2 = new List<TestListItem>();

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void FirstListEqualButShorter()
        {
            var list1 = new List<TestListItem>()
            {
                new TestListItem() {Name = "Thingy1"},
            };
            var list2 = new List<TestListItem>()
            {
                new TestListItem() {Name = "Thingy1"},
                new TestListItem() {Name = "Thingy2"}
            };

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void FirstListEqualButLonger()
        {
            var list1 = new List<TestListItem>()
            {
                new TestListItem() {Name = "Thingy1"},
                new TestListItem() {Name = "Thingy2"}
            };
            var list2 = new List<TestListItem>()
            {
                new TestListItem() {Name = "Thingy1"},
            };

            Assert.That(new CollectionComparer<TestListItem>().Equals(list1, list2), Is.False);
        }

        [Test]
        public void EqualListsCustomComparer()
        {
            var list1 = new List<TestListItem>() { new TestListItem() { Name = "Thingy" } };
            var list2 = new List<TestListItem>() { new TestListItem() { Name = "Thingy" } };

            Assert.That(new CollectionComparer<TestListItem>(CustomItemComparer).Equals(list1, list2), Is.True);
        }

        [Test]
        public void NonEqualListsCustomComparer()
        {
            var list1 = new List<TestListItem>() { new TestListItem() { Name = "Thingy1" } };
            var list2 = new List<TestListItem>() { new TestListItem() { Name = "Thingy2" } };

            Assert.That(new CollectionComparer<TestListItem>(CustomItemComparer).Equals(list1, list2), Is.False);
        }

        private bool CustomItemComparer(TestListItem x, TestListItem y)
        {
            return x.Name == y.Name;
        }

        public class TestListItem
        {
            public string Name { get; set; }
        }
    }
}
