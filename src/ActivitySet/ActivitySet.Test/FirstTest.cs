using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActivitySet.Test
{
    [TestFixture]
    public class FirstTest
    {
        [Test]
        public void CanRunNUnitTest()
        {
            bool value = true;
            Assert.That(value, Is.EqualTo(true));
        }
    }
}
