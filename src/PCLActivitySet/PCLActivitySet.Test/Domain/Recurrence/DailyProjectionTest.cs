using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class DailyProjectionTest
    {
        [Test]
        public void GetNext()
        {
            int dayCount = 2;
            DateProjection dateProjection = DateProjection(dayCount);
            dateProjection.GetNext(new DateTime(2017, 2, 28)).Should().Be(new DateTime(2017, 3, 2));
        }

        [Test]
        public void GetNextDayCountLessThan1Exception()
        {
            int dayCount = 0;
            DateProjection dateProjection = DateProjection(dayCount);
            Action action = () => dateProjection.GetNext(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetPrevious()
        {
            int dayCount = 2;
            DateProjection dateProjection = DateProjection(dayCount);
            dateProjection.GetPrevious(new DateTime(2017, 2, 28)).Should().Be(new DateTime(2017, 2, 26));
        }

        [Test]
        public void GetPreviousDayCountLessThan1Exception()
        {
            int dayCount = 0;
            DateProjection dateProjection = DateProjection(dayCount);
            Action action = () => dateProjection.GetPrevious(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
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
