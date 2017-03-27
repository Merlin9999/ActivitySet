using System;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class MonthlyProjectionTest
    {
        [Test]
        public void GetNext()
        {
            int monthCount = 2;
            int dayOfMonth = 31;
            DateProjection dateProjection = DateProjection(monthCount, dayOfMonth);
            Assert.That(dateProjection.GetNext(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2017, 4, 30)));
        }

        [Test]
        public void GetNextMonthCountLessThan1Exception()
        {
            int monthCount = 0;
            int dayOfMonth = 31;
            DateProjection dateProjection = DateProjection(monthCount, dayOfMonth);
            Assert.That(() => dateProjection.GetNext(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetNextDayOfMonthLessThan1Exception()
        {
            int monthCount = 2;
            int dayOfMonth = 0;
            DateProjection dateProjection = DateProjection(monthCount, dayOfMonth);
            Assert.That(() => dateProjection.GetNext(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetPrevious()
        {
            int monthCount = 3;
            int dayOfMonth = 31;
            DateProjection dateProjection = DateProjection(monthCount, dayOfMonth);
            Assert.That(dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2016, 11, 30)));
        }

        [Test]
        public void GetPreviousMonthCountLessThan1Exception()
        {
            int monthCount = 0;
            int dayOfMonth = 31;
            DateProjection dateProjection = DateProjection(monthCount, dayOfMonth);
            Assert.That(() => dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetPreviousDayOfMonthLessThan1Exception()
        {
            int monthCount = 2;
            int dayOfMonth = 0;
            DateProjection dateProjection = DateProjection(monthCount, dayOfMonth);
            Assert.That(() => dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        private static DateProjection DateProjection(int monthCount, int dayOfMonth)
        {
            var projection = new MonthlyProjection()
            {
                MonthCount = monthCount,
                DayOfMonth = dayOfMonth,
            };
            var dateProjection = new DateProjection(projection);
            return dateProjection;
        }
    }
}
