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
    public class DailyProjectionTest
    {
        [Test]
        public void GetNext()
        {
            int dayCount = 2;
            DateProjection dateProjection = DateProjection(dayCount);
            Assert.That(dateProjection.GetNext(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2017, 3, 2)));
        }

        [Test]
        public void GetNextDayCountLessThan1Exception()
        {
            int dayCount = 0;
            DateProjection dateProjection = DateProjection(dayCount);
            Assert.That(() => dateProjection.GetNext(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetPrevious()
        {
            int dayCount = 2;
            DateProjection dateProjection = DateProjection(dayCount);
            Assert.That(dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2017, 2, 26)));
        }

        [Test]
        public void GetPreviousDayCountLessThan1Exception()
        {
            int dayCount = 0;
            DateProjection dateProjection = DateProjection(dayCount);
            Assert.That(() => dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        private static DateProjection DateProjection(int dayCount)
        {
            var projection = new DailyProjection()
            {
                DayCount = dayCount,
            };
            var dateProjection = new DateProjection(projection);
            return dateProjection;
        }
    }
}
