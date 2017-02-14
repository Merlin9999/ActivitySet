using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PCLActivitySet.Test
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
        public void HashCodeMatchesWhenGuidsAreTheSame()
        {
            var guid = Guid.NewGuid();
            var entity1 = new TestEntity() { Guid = guid };
            var entity2 = new TestEntity() { Guid = guid };
            Assert.That(entity1.GetHashCode(), Is.EqualTo(entity2.GetHashCode()));
        }
    }

    public class TestEntity : AbstractEntity<TestEntity>
    {
    }
}
