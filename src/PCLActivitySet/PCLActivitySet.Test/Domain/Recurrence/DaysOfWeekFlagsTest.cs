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
    public class DaysOfWeekFlagsTest
    {
        [Test]
        public void DateMatches()
        {
            DateTime date = new DateTime(2017, 2, 19);
            date.DateMatches(EDaysOfWeekFlags.Sunday).Should().BeTrue();
            date.DateMatches(DaysOfWeekFlags.WeekDays).Should().BeFalse();
            date.DateMatches(DaysOfWeekFlags.WeekendDays).Should().BeTrue();
            date.DateMatches(DaysOfWeekFlags.EveryDay).Should().BeTrue();
        }

        [Test]
        public void AsSeperateValues()
        {
            HashSet<EDaysOfWeekFlags> everyDaySet = DaysOfWeekFlags.EveryDay.AsSeperateValues();
            everyDaySet.Count.Should().Be(7);
            everyDaySet.Contains(EDaysOfWeekFlags.Monday).Should().BeTrue();
            everyDaySet.Contains(EDaysOfWeekFlags.Tuesday).Should().BeTrue();
            everyDaySet.Contains(EDaysOfWeekFlags.Wednesday).Should().BeTrue();
            everyDaySet.Contains(EDaysOfWeekFlags.Thursday).Should().BeTrue();
            everyDaySet.Contains(EDaysOfWeekFlags.Friday).Should().BeTrue();
            everyDaySet.Contains(EDaysOfWeekFlags.Saturday).Should().BeTrue();
            everyDaySet.Contains(EDaysOfWeekFlags.Sunday).Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysTrue()
        {
            (EDaysOfWeekFlags.Friday | EDaysOfWeekFlags.Saturday).HasWeekDays().Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysFalse()
        {
            (EDaysOfWeekFlags.Sunday | EDaysOfWeekFlags.Saturday).HasWeekDays().Should().BeFalse();
        }

        [Test]
        public void HasWeekDaysAllowNoneTrue()
        {
            EDaysOfWeekFlags.None.HasWeekDays(true).Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysAllowNoneFalse()
        {
            EDaysOfWeekFlags.None.HasWeekDays(false).Should().BeFalse();
        }

        [Test]
        public void HasWeeendkDaysTrue()
        {
            (EDaysOfWeekFlags.Friday | EDaysOfWeekFlags.Saturday).HasWeekendDays().Should().BeTrue();
        }

        [Test]
        public void HasWeekendDaysFalse()
        {
            (EDaysOfWeekFlags.Monday | EDaysOfWeekFlags.Thursday).HasWeekendDays().Should().BeFalse();
        }

        [Test]
        public void HasWeekendDaysAllowNoneTrue()
        {
            EDaysOfWeekFlags.None.HasWeekendDays(true).Should().BeTrue();
        }

        [Test]
        public void HasWeekendDaysAllowNoneFalse()
        {
            EDaysOfWeekFlags.None.HasWeekendDays(false).Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekDaysTrue()
        {
            (EDaysOfWeekFlags.Tuesday | EDaysOfWeekFlags.Wednesday).HasOnlyWeekDays().Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekDaysFalse()
        {
            (EDaysOfWeekFlags.Thursday | EDaysOfWeekFlags.Saturday).HasOnlyWeekDays().Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekDaysAllowNoneTrue()
        {
            EDaysOfWeekFlags.None.HasOnlyWeekDays(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekDaysAllowNoneFalse()
        {
            EDaysOfWeekFlags.None.HasOnlyWeekDays(false).Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeeendkDaysTrue()
        {
            (EDaysOfWeekFlags.Sunday | EDaysOfWeekFlags.Saturday).HasOnlyWeekendDays().Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekendDaysFalse()
        {
            (EDaysOfWeekFlags.Sunday | EDaysOfWeekFlags.Monday).HasOnlyWeekendDays().Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekendDaysAllowNoneTrue()
        {
            EDaysOfWeekFlags.None.HasOnlyWeekendDays(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekendDaysAllowNoneFalse()
        {
            EDaysOfWeekFlags.None.HasOnlyWeekendDays(false).Should().BeFalse();
        }

        [Test]
        public void ConvertFromDaysOfWeek()
        {
            List<EDaysOfWeek> everyDay = DaysOfWeek.WeekDays.Concat(DaysOfWeek.WeekendDays).ToList();

            EDaysOfWeekFlags everyDayFlags = DaysOfWeekFlags.ConvertFrom(everyDay);
            ((everyDayFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None).Should().BeTrue();
        }

        [Test]
        public void ConvertFromInvalidDaysOfWeek()
        {
            Action action = () => DaysOfWeekFlags.ConvertFrom((EDaysOfWeek) int.MaxValue);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ConvertFromDaysOfWeekExt()
        {
            List<EDaysOfWeekExt> everyDay = DaysOfWeekExt.WeekDays.Concat(DaysOfWeekExt.WeekendDays).ToList();

            EDaysOfWeekFlags everyDayFlags = DaysOfWeekFlags.ConvertFrom(everyDay);
            ((everyDayFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((everyDayFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None).Should().BeTrue();
        }

        [Test]
        public void ConvertFromWeekDaysOfWeekExt()
        {
            EDaysOfWeekFlags daysOfWeekFlags = DaysOfWeekFlags.ConvertFrom(EDaysOfWeekExt.WeekDay);
            ((daysOfWeekFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None).Should().BeFalse();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None).Should().BeFalse();
        }

        [Test]
        public void ConvertFromWeekendDaysOfWeekExt()
        {
            EDaysOfWeekFlags daysOfWeekFlags = DaysOfWeekFlags.ConvertFrom(EDaysOfWeekExt.WeekendDay);
            ((daysOfWeekFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None).Should().BeFalse();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None).Should().BeFalse();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None).Should().BeFalse();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None).Should().BeFalse();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None).Should().BeFalse();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None).Should().BeTrue();
        }

        [Test]
        public void ConvertFromEveryDaysOfWeekExt()
        {
            EDaysOfWeekFlags daysOfWeekFlags = DaysOfWeekFlags.ConvertFrom(EDaysOfWeekExt.EveryDay);
            ((daysOfWeekFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None).Should().BeTrue();
            ((daysOfWeekFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None).Should().BeTrue();
        }

        [Test]
        public void ConvertFromInvalidDaysOfWeekExt()
        {
            Action action = () => DaysOfWeekFlags.ConvertFrom((EDaysOfWeekExt)int.MaxValue);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ConvertFromDayOfWeek()
        {
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Monday).Should().Be(EDaysOfWeekFlags.Monday);
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Tuesday).Should().Be(EDaysOfWeekFlags.Tuesday);
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Wednesday).Should().Be(EDaysOfWeekFlags.Wednesday);
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Thursday).Should().Be(EDaysOfWeekFlags.Thursday);
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Friday).Should().Be(EDaysOfWeekFlags.Friday);
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Saturday).Should().Be(EDaysOfWeekFlags.Saturday);
            DaysOfWeekFlags.ConvertFrom(DayOfWeek.Sunday).Should().Be(EDaysOfWeekFlags.Sunday);
            Action action = () => DaysOfWeekFlags.ConvertFrom((DayOfWeek)int.MaxValue);
            action.ShouldThrow<ArgumentException>();
        }
    }
}
