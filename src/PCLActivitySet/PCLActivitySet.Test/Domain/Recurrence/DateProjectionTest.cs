using System;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class DateProjectionTest
    {
        [Test]
        public void TranslateProjectionType()
        {
            const int periodCount = 1;
            const EMonth month = EMonth.February;
            const int dayOfMonth = 3;
            const EDaysOfWeekExt dayOfWeekExt = EDaysOfWeekExt.Thursday;
            const EDaysOfWeekFlags dayOfWeekFlags = EDaysOfWeekFlags.Friday;
            const EWeeksInMonth weekInMonth = EWeeksInMonth.Last;

            DateProjection prj = new DateProjection(EDateProjectionType.Daily)
            {
                PeriodCount = periodCount,
                Month = month,
                DayOfMonth = dayOfMonth,
                DaysOfWeekExt = dayOfWeekExt,
                DaysOfWeekFlags = dayOfWeekFlags,
                WeeksInMonth = weekInMonth,
            };

            prj.ProjectionType = EDateProjectionType.Weekly;
            Assert.That(DateProjection.ToShortDescription(prj), Is.EqualTo("Weekly"));
            prj.ProjectionType = EDateProjectionType.Monthly;
            Assert.That(DateProjection.ToShortDescription(prj), Is.EqualTo("Monthly"));
            prj.ProjectionType = EDateProjectionType.MonthlyRelative;
            Assert.That(DateProjection.ToShortDescription(prj), Is.EqualTo("Monthly Relative"));
            prj.ProjectionType = EDateProjectionType.Yearly;
            Assert.That(DateProjection.ToShortDescription(prj), Is.EqualTo("Yearly"));
            prj.ProjectionType = EDateProjectionType.YearlyRelative;
            Assert.That(DateProjection.ToShortDescription(prj), Is.EqualTo("Yearly Relative"));
            prj.ProjectionType = EDateProjectionType.Daily;
            Assert.That(DateProjection.ToShortDescription(prj), Is.EqualTo("Daily"));

            Assert.That(prj.PeriodCount, Is.EqualTo(periodCount));
            Assert.That(prj.Month, Is.EqualTo(month));
            Assert.That(prj.DayOfMonth, Is.EqualTo(dayOfMonth));
            Assert.That(prj.DaysOfWeekExt, Is.EqualTo(dayOfWeekExt));
            Assert.That(prj.DaysOfWeekFlags, Is.EqualTo(dayOfWeekFlags));
            Assert.That(prj.WeeksInMonth, Is.EqualTo(weekInMonth));

            Assert.That(DateProjection.ToShortDescription(null), Is.EqualTo("None"));
        }

        [Test]
        public void TranslateToInvalidProjectionType()
        {
            var prj = new DateProjection();
            Assert.That(() => prj.ProjectionType = (EDateProjectionType) int.MaxValue, Throws.TypeOf<InvalidOperationException>());

        }
    }
}
