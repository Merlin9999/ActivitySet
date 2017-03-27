using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCLActivitySet.Data.Recurrence;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class DaysOfWeekTest
    {
        [Test]
        public void DateMatches()
        {
            Assert.That(new DateTime(2017, 2, 19).DateMatches(EDaysOfWeek.Sunday), Is.True);
        }

        [Test]
        public void IsWeekendDayTrue()
        {
            Assert.That(EDaysOfWeek.Sunday.IsWeekendDay(), Is.True);
        }

        [Test]
        public void IsWeekendDayFalse()
        {
            Assert.That(EDaysOfWeek.Monday.IsWeekendDay(), Is.False);
        }

        [Test]
        public void IsWeekDayTrue()
        {
            Assert.That(EDaysOfWeek.Monday.IsWeekDay(), Is.True);
        }

        [Test]
        public void IsWeekDayFalse()
        {
            Assert.That(EDaysOfWeek.Sunday.IsWeekDay(), Is.False);
        }

        [Test]
        public void HasWeekDaysTrue()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Friday, EDaysOfWeek.Saturday }.HasWeekDays(), Is.True);
        }

        [Test]
        public void HasWeekDaysFalse()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Sunday, EDaysOfWeek.Saturday }.HasWeekDays(), Is.False);
        }

        [Test]
        public void HasWeekDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeek>().HasWeekDays(true), Is.True);
        }

        [Test]
        public void HasWeekDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeek>().HasWeekDays(false), Is.False);
        }
        
        [Test]
        public void HasWeeendkDaysTrue()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Friday, EDaysOfWeek.Saturday }.HasWeekendDays(), Is.True);
        }

        [Test]
        public void HasWeekendDaysFalse()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Monday, EDaysOfWeek.Thursday }.HasWeekendDays(), Is.False);
        }

        [Test]
        public void HasWeekendDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeek>().HasWeekendDays(true), Is.True);
        }

        [Test]
        public void HasWeekendDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeek>().HasWeekendDays(false), Is.False);
        }

        [Test]
        public void HasOnlyWeekDaysTrue()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Tuesday, EDaysOfWeek.Wednesday }.HasOnlyWeekDays(), Is.True);
        }

        [Test]
        public void HasOnlyWeekDaysFalse()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Thursday, EDaysOfWeek.Saturday }.HasOnlyWeekDays(), Is.False);
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeek>().HasOnlyWeekDays(true), Is.True);
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeek>().HasOnlyWeekDays(false), Is.False);
        }

        [Test]
        public void HasOnlyWeeendkDaysTrue()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Sunday, EDaysOfWeek.Saturday }.HasOnlyWeekendDays(), Is.True);
        }

        [Test]
        public void HasOnlyWeekendDaysFalse()
        {
            Assert.That(new List<EDaysOfWeek>() { EDaysOfWeek.Sunday, EDaysOfWeek.Monday }.HasOnlyWeekendDays(), Is.False);
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeek>().HasOnlyWeekendDays(true), Is.True);
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeek>().HasOnlyWeekendDays(false), Is.False);
        }

        [Test]
        public void ConvertFromDaysOfWeekExt()
        {
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Monday), Is.EqualTo(EDaysOfWeek.Monday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Tuesday), Is.EqualTo(EDaysOfWeek.Tuesday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Wednesday), Is.EqualTo(EDaysOfWeek.Wednesday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Thursday), Is.EqualTo(EDaysOfWeek.Thursday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Friday), Is.EqualTo(EDaysOfWeek.Friday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Saturday), Is.EqualTo(EDaysOfWeek.Saturday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekExt.Sunday), Is.EqualTo(EDaysOfWeek.Sunday));
            Assert.That(() => DaysOfWeek.ConvertFrom(EDaysOfWeekExt.WeekDay), Throws.TypeOf<ArgumentException>());
            Assert.That(() => DaysOfWeek.ConvertFrom((EDaysOfWeekExt)int.MaxValue), Throws.TypeOf<ArgumentException>());
        }



        [Test]
        public void ConvertFromEDaysOfWeekFlags()
        {
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Monday).Single(), Is.EqualTo(EDaysOfWeek.Monday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Tuesday).Single(), Is.EqualTo(EDaysOfWeek.Tuesday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Wednesday).Single(), Is.EqualTo(EDaysOfWeek.Wednesday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Thursday).Single(), Is.EqualTo(EDaysOfWeek.Thursday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Friday).Single(), Is.EqualTo(EDaysOfWeek.Friday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Saturday).Single(), Is.EqualTo(EDaysOfWeek.Saturday));
            Assert.That(DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Sunday).Single(), Is.EqualTo(EDaysOfWeek.Sunday));
        }

        [Test]
        public void ConvertFromEDaysOfWeekFlagsWithMultipleDays()
        {
            HashSet<EDaysOfWeek> weekendDays = DaysOfWeek.ConvertFrom(EDaysOfWeekFlags.Saturday ^ EDaysOfWeekFlags.Sunday);
            Assert.That(weekendDays, Is.Not.Empty);
            Assert.That(weekendDays.Count, Is.EqualTo(2));
            Assert.That(weekendDays.Contains(EDaysOfWeek.Saturday), Is.True);
            Assert.That(weekendDays.Contains(EDaysOfWeek.Sunday), Is.True);
        }

        [Test]
        public void ConvertFromDayOfWeek()
        {
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Monday), Is.EqualTo(EDaysOfWeek.Monday));
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Tuesday), Is.EqualTo(EDaysOfWeek.Tuesday));
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Wednesday), Is.EqualTo(EDaysOfWeek.Wednesday));
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Thursday), Is.EqualTo(EDaysOfWeek.Thursday));
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Friday), Is.EqualTo(EDaysOfWeek.Friday));
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Saturday), Is.EqualTo(EDaysOfWeek.Saturday));
            Assert.That(DaysOfWeek.ConvertFrom(DayOfWeek.Sunday), Is.EqualTo(EDaysOfWeek.Sunday));
            Assert.That(() => DaysOfWeek.ConvertFrom((DayOfWeek)int.MaxValue), Throws.TypeOf<ArgumentException>());
        }
    }
}
