using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCLActivitySet.Data.Recurrence;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class DaysOfWeekFlagsTest
    {
        [Test]
        public void DateMatches()
        {
            DateTime date = new DateTime(2017, 2, 19);
            Assert.That(date.DateMatches(EDaysOfWeekFlags.Sunday), Is.True);
            Assert.That(date.DateMatches(DaysOfWeekFlags.WeekDays), Is.False);
            Assert.That(date.DateMatches(DaysOfWeekFlags.WeekendDays), Is.True);
            Assert.That(date.DateMatches(DaysOfWeekFlags.EveryDay), Is.True);
        }

        [Test]
        public void AsSeperateValues()
        {
            HashSet<EDaysOfWeekFlags> everyDaySet = DaysOfWeekFlags.EveryDay.AsSeperateValues();
            Assert.That(everyDaySet.Count, Is.EqualTo(7));
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Monday), Is.True);
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Tuesday), Is.True);
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Wednesday), Is.True);
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Thursday), Is.True);
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Friday), Is.True);
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Saturday), Is.True);
            Assert.That(everyDaySet.Contains(EDaysOfWeekFlags.Sunday), Is.True);
        }

        [Test]
        public void HasWeekDaysTrue()
        {
            Assert.That((EDaysOfWeekFlags.Friday | EDaysOfWeekFlags.Saturday).HasWeekDays(), Is.True);
        }

        [Test]
        public void HasWeekDaysFalse()
        {
            Assert.That((EDaysOfWeekFlags.Sunday | EDaysOfWeekFlags.Saturday).HasWeekDays(), Is.False);
        }

        [Test]
        public void HasWeekDaysAllowNoneTrue()
        {
            Assert.That(EDaysOfWeekFlags.None.HasWeekDays(true), Is.True);
        }

        [Test]
        public void HasWeekDaysAllowNoneFalse()
        {
            Assert.That(EDaysOfWeekFlags.None.HasWeekDays(false), Is.False);
        }

        [Test]
        public void HasWeeendkDaysTrue()
        {
            Assert.That((EDaysOfWeekFlags.Friday | EDaysOfWeekFlags.Saturday).HasWeekendDays(), Is.True);
        }

        [Test]
        public void HasWeekendDaysFalse()
        {
            Assert.That((EDaysOfWeekFlags.Monday | EDaysOfWeekFlags.Thursday).HasWeekendDays(), Is.False);
        }

        [Test]
        public void HasWeekendDaysAllowNoneTrue()
        {
            Assert.That(EDaysOfWeekFlags.None.HasWeekendDays(true), Is.True);
        }

        [Test]
        public void HasWeekendDaysAllowNoneFalse()
        {
            Assert.That(EDaysOfWeekFlags.None.HasWeekendDays(false), Is.False);
        }

        [Test]
        public void HasOnlyWeekDaysTrue()
        {
            Assert.That((EDaysOfWeekFlags.Tuesday | EDaysOfWeekFlags.Wednesday).HasOnlyWeekDays(), Is.True);
        }

        [Test]
        public void HasOnlyWeekDaysFalse()
        {
            Assert.That((EDaysOfWeekFlags.Thursday | EDaysOfWeekFlags.Saturday).HasOnlyWeekDays(), Is.False);
        }

        [Test]
        public void HasOnlyWeekDaysAllowNoneTrue()
        {
            Assert.That(EDaysOfWeekFlags.None.HasOnlyWeekDays(true), Is.True);
        }

        [Test]
        public void HasOnlyWeekDaysAllowNoneFalse()
        {
            Assert.That(EDaysOfWeekFlags.None.HasOnlyWeekDays(false), Is.False);
        }

        [Test]
        public void HasOnlyWeeendkDaysTrue()
        {
            Assert.That((EDaysOfWeekFlags.Sunday | EDaysOfWeekFlags.Saturday).HasOnlyWeekendDays(), Is.True);
        }

        [Test]
        public void HasOnlyWeekendDaysFalse()
        {
            Assert.That((EDaysOfWeekFlags.Sunday | EDaysOfWeekFlags.Monday).HasOnlyWeekendDays(), Is.False);
        }

        [Test]
        public void HasOnlyWeekendDaysAllowNoneTrue()
        {
            Assert.That(EDaysOfWeekFlags.None.HasOnlyWeekendDays(true), Is.True);
        }

        [Test]
        public void HasOnlyWeekendDaysAllowNoneFalse()
        {
            Assert.That(EDaysOfWeekFlags.None.HasOnlyWeekendDays(false), Is.False);
        }

        [Test]
        public void ConvertFromDaysOfWeek()
        {
            List<EDaysOfWeek> everyDay = DaysOfWeek.WeekDays.Concat(DaysOfWeek.WeekendDays).ToList();

            EDaysOfWeekFlags everyDayFlags = DaysOfWeekFlags.ConvertFrom(everyDay);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None, Is.True);
        }

        [Test]
        public void ConvertFromInvalidDaysOfWeek()
        {
            Assert.That(() => DaysOfWeekFlags.ConvertFrom((EDaysOfWeek) int.MaxValue), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConvertFromDaysOfWeekExt()
        {
            List<EDaysOfWeekExt> everyDay = DaysOfWeekExt.WeekDays.Concat(DaysOfWeekExt.WeekendDays).ToList();

            EDaysOfWeekFlags everyDayFlags = DaysOfWeekFlags.ConvertFrom(everyDay);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((everyDayFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None, Is.True);
        }

        [Test]
        public void ConvertFromWeekDaysOfWeekExt()
        {
            EDaysOfWeekFlags daysOfWeekFlags = DaysOfWeekFlags.ConvertFrom(EDaysOfWeekExt.WeekDay);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None, Is.False);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None, Is.False);
        }

        [Test]
        public void ConvertFromWeekendDaysOfWeekExt()
        {
            EDaysOfWeekFlags daysOfWeekFlags = DaysOfWeekFlags.ConvertFrom(EDaysOfWeekExt.WeekendDay);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None, Is.False);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None, Is.False);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None, Is.False);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None, Is.False);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None, Is.False);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None, Is.True);
        }

        [Test]
        public void ConvertFromEveryDaysOfWeekExt()
        {
            EDaysOfWeekFlags daysOfWeekFlags = DaysOfWeekFlags.ConvertFrom(EDaysOfWeekExt.EveryDay);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Monday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Tuesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Wednesday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Thursday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Friday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Saturday) != EDaysOfWeekFlags.None, Is.True);
            Assert.That((daysOfWeekFlags & EDaysOfWeekFlags.Sunday) != EDaysOfWeekFlags.None, Is.True);
        }

        [Test]
        public void ConvertFromInvalidDaysOfWeekExt()
        {
            Assert.That(() => DaysOfWeekFlags.ConvertFrom((EDaysOfWeekExt)int.MaxValue), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConvertFromDayOfWeek()
        {
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Monday), Is.EqualTo(EDaysOfWeekFlags.Monday));
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Tuesday), Is.EqualTo(EDaysOfWeekFlags.Tuesday));
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Wednesday), Is.EqualTo(EDaysOfWeekFlags.Wednesday));
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Thursday), Is.EqualTo(EDaysOfWeekFlags.Thursday));
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Friday), Is.EqualTo(EDaysOfWeekFlags.Friday));
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Saturday), Is.EqualTo(EDaysOfWeekFlags.Saturday));
            Assert.That(DaysOfWeekFlags.ConvertFrom(DayOfWeek.Sunday), Is.EqualTo(EDaysOfWeekFlags.Sunday));
            Assert.That(() => DaysOfWeekFlags.ConvertFrom((DayOfWeek)int.MaxValue), Throws.TypeOf<ArgumentException>());
        }
    }
}
