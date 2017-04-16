using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class WeeklyProjectionTest
    {
        [Test]
        public void GetNext()
        {
            int weekCount = 2;
            EDaysOfWeekFlags daysOfWeek = EDaysOfWeekFlags.Monday;
            DateProjection dateProjection = DateProjection(weekCount, daysOfWeek);
            dateProjection.GetNext(new DateTime(2017, 2, 28)).Should().Be(new DateTime(2017, 3, 13));
        }

        [Test]
        public void GetNextWeekCountLessThan1Exception()
        {
            int weekCount = 0;
            EDaysOfWeekFlags daysOfWeek = EDaysOfWeekFlags.Monday;
            DateProjection dateProjection = DateProjection(weekCount, daysOfWeek);
            Action action = () => dateProjection.GetNext(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetNextDayOfWeekIsNoneException()
        {
            int weekCount = 2;
            EDaysOfWeekFlags daysOfWeek = EDaysOfWeekFlags.None;
            DateProjection dateProjection = DateProjection(weekCount, daysOfWeek);
            Action action = () => dateProjection.GetNext(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetPrevious()
        {
            int weekCount = 2;
            EDaysOfWeekFlags daysOfWeek = EDaysOfWeekFlags.Monday;
            DateProjection dateProjection = DateProjection(weekCount, daysOfWeek);
            dateProjection.GetPrevious(new DateTime(2017, 2, 28)).Should().Be(new DateTime(2017, 2, 20));
        }

        [Test]
        public void GetPreviousWeekCountLessThan1Exception()
        {
            int weekCount = 0;
            EDaysOfWeekFlags daysOfWeek = EDaysOfWeekFlags.Monday;
            DateProjection dateProjection = DateProjection(weekCount, daysOfWeek);
            Action action = () => dateProjection.GetPrevious(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetPreviousDayOfWeekIsNoneException()
        {
            int weekCount = 2;
            EDaysOfWeekFlags daysOfWeek = EDaysOfWeekFlags.None;
            DateProjection dateProjection = DateProjection(weekCount, daysOfWeek);
            Action action = () => dateProjection.GetPrevious(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        private static DateProjection DateProjection(int weekCount, EDaysOfWeekFlags daysOfWeek)
        {
            var projection = new WeeklyProjection()
            {
                WeekCount = weekCount,
                DaysOfWeek = daysOfWeek,
            };
            var dateProjection = new DateProjection(projection);
            return dateProjection;
        }
    }
}
