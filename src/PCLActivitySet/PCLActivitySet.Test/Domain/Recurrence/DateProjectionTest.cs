using System;
using FluentAssertions;
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
            DateProjection.ToShortDescription(prj).Should().Be("Weekly");
            prj.ProjectionType = EDateProjectionType.Monthly;
            DateProjection.ToShortDescription(prj).Should().Be("Monthly");
            prj.ProjectionType = EDateProjectionType.MonthlyRelative;
            DateProjection.ToShortDescription(prj).Should().Be("Monthly Relative");
            prj.ProjectionType = EDateProjectionType.Yearly;
            DateProjection.ToShortDescription(prj).Should().Be("Yearly");
            prj.ProjectionType = EDateProjectionType.YearlyRelative;
            DateProjection.ToShortDescription(prj).Should().Be("Yearly Relative");
            prj.ProjectionType = EDateProjectionType.Daily;
            DateProjection.ToShortDescription(prj).Should().Be("Daily");

            prj.PeriodCount.Should().Be(periodCount);
            prj.Month.Should().Be(month);
            prj.DayOfMonth.Should().Be(dayOfMonth);
            prj.DaysOfWeekExt.Should().Be(dayOfWeekExt);
            prj.DaysOfWeekFlags.Should().Be(dayOfWeekFlags);
            prj.WeeksInMonth.Should().Be(weekInMonth);

            DateProjection.ToShortDescription(null).Should().Be("None");
        }

        [Test]
        public void TranslateToInvalidProjectionType()
        {
            var prj = new DateProjection();
            Action action = () => prj.ProjectionType = (EDateProjectionType) int.MaxValue;
            action.ShouldThrow<InvalidOperationException>();
        }
    }
}
