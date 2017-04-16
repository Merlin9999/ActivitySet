using System;
using FluentAssertions;
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
            entity.Equals(entity).Should().BeTrue();
        }

        [Test]
        public void EqualsReturnsTrueWhenGuidsMatch()
        {
            var guid = Guid.NewGuid();
            var entity1 = new TestEntity() {Guid = guid};
            var entity2 = new TestEntity() { Guid = guid };
            entity1.Equals(entity2).Should().BeTrue();
        }

        [Test]
        public void EqualsReturnsFalseWhenGuidsDontMatch()
        {
            var entity1 = new TestEntity();
            var entity2 = new TestEntity();
            entity1.Equals(entity2).Should().BeFalse();
        }

        [Test]
        public void EqualsReturnsFalseWhenComparingWithNull()
        {
            new TestEntity().Equals(null).Should().BeFalse();
        }

        [Test]
        public void EqualsTakingObjectParam()
        {
            var entity = new TestEntity();
            entity.Equals((object)entity).Should().BeTrue();
        }

        [Test]
        public void HashCodeMatchesWhenGuidsAreTheSame()
        {
            var guid = Guid.NewGuid();
            var entity1 = new TestEntity() { Guid = guid };
            var entity2 = new TestEntity() { Guid = guid };
            entity1.GetHashCode().Should().Be(entity2.GetHashCode());
        }
    }

    public class TestEntity : AbstractDomainEntity<TestEntity>
    {
    }
}
