using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class RelativeDateTest
    {
        [Test]
        public void GetDateInFirstWeek()
        {
            RelativeDate.GetDate(2017, 2, EWeeksInMonth.First, EDaysOfWeek.Monday).Should().Be(new DateTime(2017, 2, 6));
        }

        [Test]
        public void GetDateInSecondWeek()
        {
            RelativeDate.GetDate(2017, EMonth.February, EWeeksInMonth.Second, EDaysOfWeek.Monday).Should().Be(new DateTime(2017, 2, 13));
        }

        [Test]
        public void GetDateInThirdWeek()
        {
            RelativeDate.GetDate(2017, 2, EWeeksInMonth.Third, EDaysOfWeekExt.Monday).Should().Be(new DateTime(2017, 2, 20));
        }

        [Test]
        public void GetDateInFourthWeek()
        {
            RelativeDate.GetDate(2017, EMonth.February, EWeeksInMonth.Fourth, EDaysOfWeekExt.Monday).Should().Be(new DateTime(2017, 2, 27));
        }

        [Test]
        public void GetDateInLastWeek()
        {
            RelativeDate.GetDate(2017, EMonth.February, EWeeksInMonth.Last, EDaysOfWeekExt.Monday).Should().Be(new DateTime(2017, 2, 27));
        }

        [Test]
        public void GetDateInAnInvalidWeek()
        {
            Action action = () => RelativeDate.GetDate(2017, EMonth.February, (EWeeksInMonth)int.MaxValue, EDaysOfWeekExt.Monday);
            action.ShouldThrow<ArgumentException>();
        }
    }
}
