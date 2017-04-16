using System;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class MonthTest
    {
        [Test]
        public void GetMonthNumber()
        {
            Month.GetMonthNumber(EMonth.May).Should().Be(5);
            Action action = () => Month.GetMonthNumber((EMonth)13);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void GetMonth()
        {
            Month.GetMonth(6).Should().Be(EMonth.June);
            Action action = () => Month.GetMonth(0);
            action.ShouldThrow<ArgumentException>();
        }
    }
}
