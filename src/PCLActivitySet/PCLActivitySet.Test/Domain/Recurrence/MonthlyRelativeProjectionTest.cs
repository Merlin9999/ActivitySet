using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class MonthlyRelativeProjectionTest
    {
        [Test]
        public void GetNext()
        {
            int monthCount = 2;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            dateProjection.GetNext(new DateTime(2017, 2, 28)).Should().Be(new DateTime(2017, 4, 10));
        }

        [Test]
        public void GetNextMonthCountLessThan1Exception()
        {
            int monthCount = 0;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            Action action = () => dateProjection.GetNext(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetPrevious()
        {
            int monthCount = 3;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            dateProjection.GetPrevious(new DateTime(2017, 2, 28)).Should().Be(new DateTime(2016, 12, 12));
        }

        [Test]
        public void GetPreviousMonthCountLessThan1Exception()
        {
            int monthCount = 0;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            Action action = () => dateProjection.GetPrevious(new DateTime(2017, 2, 28));
            action.ShouldThrow<InvalidOperationException>();
        }

        private static DateProjection DateProjection(int monthCount, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            var projection = new MonthlyRelativeProjection()
            {
                MonthCount = monthCount,
                WeeksInMonth = weeksInMonth,
                DaysOfWeekExt = daysOfWeek,
            };
            var dateProjection = new DateProjection(projection);
            return dateProjection;
        }
    }
}
