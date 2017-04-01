using System;
using NUnit.Framework;
using PCLActivitySet.Domain;

namespace PCLActivitySet.Test.Domain
{
    [TestFixture]
    public class AbstractEntityTest
    {
        [Test]
        public void EqualsReturnsTrueWhenSameInstance()
        {
            var entity = new TestEntity();
            Assert.That(entity.Equals(entity), Is.True);
        }

        [Test]
        public void EqualsReturnsTrueWhenGuidsMatch()
        {
            var guid = Guid.NewGuid();
            var entity1 = new TestEntity() {Guid = guid};
            var entity2 = new TestEntity() { Guid = guid };
            Assert.That(entity1.Equals(entity2), Is.True);
        }

        [Test]
        public void EqualsReturnsFalseWhenGuidsDontMatch()
        {
            var entity1 = new TestEntity();
            var entity2 = new TestEntity();
            Assert.That(entity1.Equals(entity2), Is.False);
        }

        [Test]
        public void EqualsReturnsFalseWhenComparingWithNull()
        {
            Assert.That(new TestEntity().Equals(null), Is.False);
        }

        [Test]
        public void EqualsTakingObjectParam()
        {
            var entity = new TestEntity();
            Assert.That(entity.Equals((object)entity), Is.True);
        }

        [Test]
        public void HashCodeMatchesWhenGuidsAreTheSame()
        {
            var guid = Guid.NewGuid();
            var entity1 = new TestEntity() { Guid = guid };
            var entity2 = new TestEntity() { Guid = guid };
            Assert.That(entity1.GetHashCode(), Is.EqualTo(entity2.GetHashCode()));
        }
    }

    public class TestEntity : AbstractDomainEntity<TestEntity>
    {
    }
}
