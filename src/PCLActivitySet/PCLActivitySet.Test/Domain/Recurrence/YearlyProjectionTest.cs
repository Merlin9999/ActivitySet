using System;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class YearlyProjectionTest
    {
        [Test]
        public void GetNext()
        {
            EMonth month = EMonth.February;
            int dayOfMonth = 31;
            DateProjection dateProjection = DateProjection(month, dayOfMonth);
            Assert.That(dateProjection.GetNext(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2018, 2, 28)));
        }

        [Test]
        public void GetNextDayOfMonthLessThan1Exception()
        {
            EMonth month = EMonth.February;
            int dayOfMonth = 0;
            DateProjection dateProjection = DateProjection(month, dayOfMonth);
            Assert.That(() => dateProjection.GetNext(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetPrevious()
        {
            EMonth month = EMonth.February;
            int dayOfMonth = 31;
            DateProjection dateProjection = DateProjection(month, dayOfMonth);
            Assert.That(dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2016, 2, 29)));
        }

        [Test]
        public void GetPreviousDayOfMonthLessThan1Exception()
        {
            EMonth month = EMonth.February;
            int dayOfMonth = 0;
            DateProjection dateProjection = DateProjection(month, dayOfMonth);
            Assert.That(() => dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        private static DateProjection DateProjection(EMonth month, int dayOfMonth)
        {
            var projection = new YearlyProjection()
            {
                Month = month,
                DayOfMonth = dayOfMonth,
            };
            var dateProjection = new DateProjection(projection);
            return dateProjection;
        }
    }
}
