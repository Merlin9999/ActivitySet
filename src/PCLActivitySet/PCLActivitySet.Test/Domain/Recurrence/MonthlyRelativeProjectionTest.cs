using System;
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
            Assert.That(dateProjection.GetNext(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2017, 4, 10)));
        }

        [Test]
        public void GetNextMonthCountLessThan1Exception()
        {
            int monthCount = 0;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            Assert.That(() => dateProjection.GetNext(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetPrevious()
        {
            int monthCount = 3;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            Assert.That(dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2016, 12, 12)));
        }

        [Test]
        public void GetPreviousMonthCountLessThan1Exception()
        {
            int monthCount = 0;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(monthCount, weeksInMonth, daysOfWeek);
            Assert.That(() => dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Throws.TypeOf<InvalidOperationException>());
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
