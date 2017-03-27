using System;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class YearlyRelativeProjectionTest
    {
        [Test]
        public void GetNext()
        {
            EMonth month = EMonth.March;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(month, weeksInMonth, daysOfWeek);
            Assert.That(dateProjection.GetNext(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2017, 3, 13)));
        }

        [Test]
        public void GetPrevious()
        {
            EMonth month = EMonth.March;
            EWeeksInMonth weeksInMonth = EWeeksInMonth.Second;
            EDaysOfWeekExt daysOfWeek = EDaysOfWeekExt.Monday;
            DateProjection dateProjection = DateProjection(month, weeksInMonth, daysOfWeek);
            Assert.That(dateProjection.GetPrevious(new DateTime(2017, 2, 28)), Is.EqualTo(new DateTime(2016, 3, 14)));
        }

        private static DateProjection DateProjection(EMonth month, EWeeksInMonth weeksInMonth, EDaysOfWeekExt daysOfWeek)
        {
            var projection = new YearlyRelativeProjection()
            {
                Month = month,
                WeeksInMonth = weeksInMonth,
                DaysOfWeekExt = daysOfWeek,
            };
            var dateProjection = new DateProjection(projection);
            return dateProjection;
        }
    }
}
