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
    public class DaysOfWeekExtTest
    {
        [Test]
        public void DateMatches()
        {
            DateTime date = new DateTime(2017, 2, 19);
            date.DateMatches(EDaysOfWeekExt.Sunday).Should().BeTrue();
            date.DateMatches(EDaysOfWeekExt.WeekDay).Should().BeFalse();
            date.DateMatches(EDaysOfWeekExt.WeekendDay).Should().BeTrue();
            date.DateMatches(EDaysOfWeekExt.EveryDay).Should().BeTrue();
        }

        [Test]
        public void IsWeekendDayTrue()
        {
            EDaysOfWeekExt.Sunday.IsWeekendDay().Should().BeTrue();
        }

        [Test]
        public void IsWeekendDayFalse()
        {
            EDaysOfWeekExt.Monday.IsWeekendDay().Should().BeFalse();
        }

        [Test]
        public void IsWeekDayTrue()
        {
            EDaysOfWeekExt.Tuesday.IsWeekDay().Should().BeTrue();
        }

        [Test]
        public void IsWeekDayFalse()
        {
            EDaysOfWeekExt.Sunday.IsWeekDay().Should().BeFalse();
        }

        [Test]
        public void IsDayGroupClassifierTrue()
        {
            EDaysOfWeekExt.WeekDay.IsDayGroupClassifier().Should().BeTrue();
        }

        [Test]
        public void IsDayGroupClassifierFalse()
        {
            EDaysOfWeekExt.Sunday.IsDayGroupClassifier().Should().BeFalse();
        }

        [Test]
        public void HasWeekDaysTrue()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Friday, EDaysOfWeekExt.Saturday }.HasWeekDays().Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysFalse()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.Saturday }.HasWeekDays().Should().BeFalse();
        }

        [Test]
        public void HasWeekDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeekExt>().HasWeekDays(true).Should().BeTrue();
        }

        [Test]
        public void HasWeekDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeekExt>().HasWeekDays(false).Should().BeFalse();
        }

        [Test]
        public void HasWeeendkDaysTrue()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Friday, EDaysOfWeekExt.Saturday }.HasWeekendDays().Should().BeTrue();
        }

        [Test]
        public void HasWeekendDaysFalse()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Monday, EDaysOfWeekExt.Thursday }.HasWeekendDays().Should().BeFalse();
        }

        [Test]
        public void HasWeekendDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeekExt>().HasWeekendDays(true).Should().BeTrue();
        }

        [Test]
        public void HasWeekendDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeekExt>().HasWeekendDays(false).Should().BeFalse();
        }

        [Test]
        public void HasDayGroupClassifierTrue()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Friday, EDaysOfWeekExt.WeekDay }.HasDayGroupClassifier().Should().BeTrue();
        }

        [Test]
        public void HasDayGroupClassifierFalse()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Monday, EDaysOfWeekExt.Saturday }.HasDayGroupClassifier().Should().BeFalse();
        }

        [Test]
        public void HasDayGroupClassifierAllowEmptyTrue()
        {
            new List<EDaysOfWeekExt>().HasDayGroupClassifier(true).Should().BeTrue();
        }

        [Test]
        public void HasDayGroupClassifierAllowEmptyFalse()
        {
            new List<EDaysOfWeekExt>().HasDayGroupClassifier(false).Should().BeFalse();
        }
        
        [Test]
        public void HasOnlyWeekDaysTrue()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Tuesday, EDaysOfWeekExt.Wednesday }.HasOnlyWeekDays().Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekDaysFalse()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Thursday, EDaysOfWeekExt.Saturday }.HasOnlyWeekDays().Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeekExt>().HasOnlyWeekDays(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeekExt>().HasOnlyWeekDays(false).Should().BeFalse();
        }

        [Test]
        public void HasOnlyDayGroupClassifiersTrue()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.WeekDay, EDaysOfWeekExt.WeekendDay }.HasOnlyDayGroupClassifiers().Should().BeTrue();
        }

        [Test]
        public void HasOnlyDayGroupClassifiersFalse()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.EveryDay }.HasOnlyDayGroupClassifiers().Should().BeFalse();
        }

        [Test]
        public void HasOnlyDayGroupClassifiersAllowEmptyTrue()
        {
            new List<EDaysOfWeekExt>().HasOnlyDayGroupClassifiers(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyDayGroupClassifiersAllowEmptyFalse()
        {
            new List<EDaysOfWeekExt>().HasOnlyDayGroupClassifiers(false).Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeeendkDaysTrue()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.Saturday }.HasOnlyWeekendDays().Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekendDaysFalse()
        {
            new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.Monday }.HasOnlyWeekendDays().Should().BeFalse();
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyTrue()
        {
            new List<EDaysOfWeekExt>().HasOnlyWeekendDays(true).Should().BeTrue();
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyFalse()
        {
            new List<EDaysOfWeekExt>().HasOnlyWeekendDays(false).Should().BeFalse();
        }

        [Test]
        public void ConvertFromDaysOfWeek()
        {
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Monday).Should().Be(EDaysOfWeekExt.Monday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Tuesday).Should().Be(EDaysOfWeekExt.Tuesday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Wednesday).Should().Be(EDaysOfWeekExt.Wednesday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Thursday).Should().Be(EDaysOfWeekExt.Thursday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Friday).Should().Be(EDaysOfWeekExt.Friday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Saturday).Should().Be(EDaysOfWeekExt.Saturday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Sunday).Should().Be(EDaysOfWeekExt.Sunday);
            Action action = () => DaysOfWeekExt.ConvertFrom((EDaysOfWeek) int.MaxValue);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ConvertFromEDaysOfWeekFlags()
        {
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Monday).Single().Should().Be(EDaysOfWeekExt.Monday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Tuesday).Single().Should().Be(EDaysOfWeekExt.Tuesday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Wednesday).Single().Should().Be(EDaysOfWeekExt.Wednesday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Thursday).Single().Should().Be(EDaysOfWeekExt.Thursday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Friday).Single().Should().Be(EDaysOfWeekExt.Friday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Saturday).Single().Should().Be(EDaysOfWeekExt.Saturday);
            DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Sunday).Single().Should().Be(EDaysOfWeekExt.Sunday);
        }

        [Test]
        public void ConvertFromEDaysOfWeekFlagsWithMultipleDays()
        {
            HashSet<EDaysOfWeekExt> weekendDays = DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Saturday ^ EDaysOfWeekFlags.Sunday);
            weekendDays.Should().NotBeEmpty();
            weekendDays.Count.Should().Be(2);
            weekendDays.Contains(EDaysOfWeekExt.Saturday).Should().BeTrue();
            weekendDays.Contains(EDaysOfWeekExt.Sunday).Should().BeTrue();
        }

        [Test]
        public void ConvertFromDayOfWeek()
        {
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Monday).Should().Be(EDaysOfWeekExt.Monday);
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Tuesday).Should().Be(EDaysOfWeekExt.Tuesday);
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Wednesday).Should().Be(EDaysOfWeekExt.Wednesday);
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Thursday).Should().Be(EDaysOfWeekExt.Thursday);
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Friday).Should().Be(EDaysOfWeekExt.Friday);
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Saturday).Should().Be(EDaysOfWeekExt.Saturday);
            DaysOfWeekExt.ConvertFrom(DayOfWeek.Sunday).Should().Be(EDaysOfWeekExt.Sunday);
            Action action = () => DaysOfWeekExt.ConvertFrom((DayOfWeek)int.MaxValue);
            action.ShouldThrow<ArgumentException>();
        }
    }
}
