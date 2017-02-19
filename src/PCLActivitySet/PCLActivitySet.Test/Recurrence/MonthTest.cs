using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test.Recurrence
{
    [TestFixture]
    public class MonthTest
    {
        [Test]
        public void GetMonthNumber()
        {
            Assert.That(Month.GetMonthNumber(EMonth.May), Is.EqualTo(5));
            Assert.That(() => Month.GetMonthNumber((EMonth)13), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetMonth()
        {
            Assert.That(Month.GetMonth(6), Is.EqualTo(EMonth.June));
            Assert.That(() => Month.GetMonth(0), Throws.TypeOf<ArgumentException>());
        }
    }
}
