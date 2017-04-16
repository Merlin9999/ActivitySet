using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class DaysOfWeekTest
    {
        [Test]
        public void DateMatches()
        {
            new DateTime(2017, 2, 19).DateMatches(EDaysOfWeek.Sunday).Should().BeTrue();
        }

        [Test]
        public void IsWeekendDayTrue()
        {
            EDaysOfWeek.Sunday.IsWeekendDay().Should().BeTrue();
        }

        [Test]
        public void IsWeekendDayFalse()
        {
            EDaysOfWeek.Monday.IsWeekendDay().Should().BeFalse();
        }

        [Test]
        public void IsWeekDayTrue()
        {
            EDaysOfWeek.Monday.IsWeekDay().Should().BeTrue();
        }

        [Test]
        public void IsWeekDayFalse()
        {
            EDaysOfWeek.Sunday.IsWeekDay().Should().BeFalse();
        }

        [Test]
        public void HasWeekDaysTrue()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Friday, EDaysOfWeek.Saturday }.HasWeekDays().Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysFalse()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Sunday, EDaysOfWeek.Saturday }.HasWeekDays().Should().BeFalse();
        }

        [Test]
        public void HasWeekDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeek>().HasWeekDays(true).Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeek>().HasWeekDays(false).Should().BeFalse();
        }
        
        [Test]
        public void HasWeeendkDaysTrue()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Friday, EDaysOfWeek.Saturday }.HasWeekendDays().Should().BeTrue();
        }

        [Test]
        public void HasWeekendDaysFalse()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Monday, EDaysOfWeek.Thursday }.HasWeekendDays().Should().BeFalse();
        }

        [Test]
        public void HasWeekendDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeek>().HasWeekendDays(true).Should().BeTrue();
        }

        [Test]
        public void HasWeekendDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeek>().HasWeekendDays(false).Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekDaysTrue()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Tuesday, EDaysOfWeek.Wednesday }.HasOnlyWeekDays().Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekDaysFalse()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Thursday, EDaysOfWeek.Saturday }.HasOnlyWeekDays().Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeek>().HasOnlyWeekDays(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeek>().HasOnlyWeekDays(false).Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeeendkDaysTrue()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Sunday, EDaysOfWeek.Saturday }.HasOnlyWeekendDays().Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekendDaysFalse()
        {
            new List<EDaysOfWeek>() { EDaysOfWeek.Sunday, EDaysOfWeek.Monday }.HasOnlyWeekendDays().Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeek>().HasOnlyWeekendDays(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeek>().HasOnlyWeekendDays(false).Should().BeFalse();
        }

        [Test]
        public void ConvertFromDaysOfWeekExt()
        {
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Monday).Should().Be(EDaysOfWeek.Monday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Tuesday).Should().Be(EDaysOfWeek.Tuesday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Wednesday).Should().Be(EDaysOfWeek.Wednesday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Thursday).Should().Be(EDaysOfWeek.Thursday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Friday).Should().Be(EDaysOfWeek.Friday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Saturday).Should().Be(EDaysOfWeek.Saturday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Sunday).Should().Be(EDaysOfWeek.Sunday);
            Action action1 = () => DaysOfWeek.ConvertFrom(EDaysOfWeekExt.WeekDay);
            action1.ShouldThrow<ArgumentException>();
            Action action2 = () => DaysOfWeek.ConvertFrom((EDaysOfWeekExt)int.MaxValue);
            action2.ShouldThrow<ArgumentException>();
        }



        [Test]
        public void ConvertFromEDaysOfWeekFlags()
        {
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Monday).Single().Should().Be(EDaysOfWeek.Monday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Tuesday).Single().Should().Be(EDaysOfWeek.Tuesday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Wednesday).Single().Should().Be(EDaysOfWeek.Wednesday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Thursday).Single().Should().Be(EDaysOfWeek.Thursday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Friday).Single().Should().Be(EDaysOfWeek.Friday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Saturday).Single().Should().Be(EDaysOfWeek.Saturday);
            DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Sunday).Single().Should().Be(EDaysOfWeek.Sunday);
        }

        [Test]
        public void ConvertFromEDaysOfWeekFlagsWithMultipleDays()
        {
            HashSet<EDaysOfWeek> weekendDays = DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Saturday ^ EDaysOfWeekFlags.Sunday);
            weekendDays.Should().NotBeEmpty();
            weekendDays.Count.Should().Be(2);
            weekendDays.Contains(EDaysOfWeek.Saturday).Should().BeTrue();
            weekendDays.Contains(EDaysOfWeek.Sunday).Should().BeTrue();
        }

        [Test]
        public void ConvertFromDayOfWeek()
        {
            DaysOfWeek.ConvertFrom(DayOfWeek.Monday).Should().Be(EDaysOfWeek.Monday);
            DaysOfWeek.ConvertFrom(DayOfWeek.Tuesday).Should().Be(EDaysOfWeek.Tuesday);
            DaysOfWeek.ConvertFrom(DayOfWeek.Wednesday).Should().Be(EDaysOfWeek.Wednesday);
            DaysOfWeek.ConvertFrom(DayOfWeek.Thursday).Should().Be(EDaysOfWeek.Thursday);
            DaysOfWeek.ConvertFrom(DayOfWeek.Friday).Should().Be(EDaysOfWeek.Friday);
            DaysOfWeek.ConvertFrom(DayOfWeek.Saturday).Should().Be(EDaysOfWeek.Saturday);
            DaysOfWeek.ConvertFrom(DayOfWeek.Sunday).Should().Be(EDaysOfWeek.Sunday);
            Action action = () => DaysOfWeek.ConvertFrom((DayOfWeek)int.MaxValue);
            action.ShouldThrow<ArgumentException>();
        }
    }
}
