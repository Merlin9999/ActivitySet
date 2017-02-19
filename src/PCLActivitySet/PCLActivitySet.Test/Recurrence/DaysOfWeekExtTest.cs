using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test.Recurrence
{
    [TestFixture]
    public class DaysOfWeekExtTest
    {
        [Test]
        public void DateMatches()
        {
            DateTime date = new DateTime(2017, 2, 19);
            Assert.That(date.DateMatches(EDaysOfWeekExt.Sunday), Is.True);
            Assert.That(date.DateMatches(EDaysOfWeekExt.WeekDay), Is.False);
            Assert.That(date.DateMatches(EDaysOfWeekExt.WeekendDay), Is.True);
            Assert.That(date.DateMatches(EDaysOfWeekExt.EveryDay), Is.True);
        }

        [Test]
        public void IsWeekendDayTrue()
        {
            Assert.That(EDaysOfWeekExt.Sunday.IsWeekendDay(), Is.True);
        }

        [Test]
        public void IsWeekendDayFalse()
        {
            Assert.That(EDaysOfWeekExt.Monday.IsWeekendDay(), Is.False);
        }

        [Test]
        public void IsWeekDayTrue()
        {
            Assert.That(EDaysOfWeekExt.Tuesday.IsWeekDay(), Is.True);
        }

        [Test]
        public void IsWeekDayFalse()
        {
            Assert.That(EDaysOfWeekExt.Sunday.IsWeekDay(), Is.False);
        }

        [Test]
        public void IsDayGroupClassifierTrue()
        {
            Assert.That(EDaysOfWeekExt.WeekDay.IsDayGroupClassifier(), Is.True);
        }

        [Test]
        public void IsDayGroupClassifierFalse()
        {
            Assert.That(EDaysOfWeekExt.Sunday.IsDayGroupClassifier(), Is.False);
        }

        [Test]
        public void HasWeekDaysTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Friday, EDaysOfWeekExt.Saturday }.HasWeekDays(), Is.True);
        }

        [Test]
        public void HasWeekDaysFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.Saturday }.HasWeekDays(), Is.False);
        }

        [Test]
        public void HasWeekDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasWeekDays(true), Is.True);
        }

        [Test]
        public void HasWeekDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasWeekDays(false), Is.False);
        }

        [Test]
        public void HasWeeendkDaysTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Friday, EDaysOfWeekExt.Saturday }.HasWeekendDays(), Is.True);
        }

        [Test]
        public void HasWeekendDaysFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Monday, EDaysOfWeekExt.Thursday }.HasWeekendDays(), Is.False);
        }

        [Test]
        public void HasWeekendDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasWeekendDays(true), Is.True);
        }

        [Test]
        public void HasWeekendDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasWeekendDays(false), Is.False);
        }

        [Test]
        public void HasDayGroupClassifierTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Friday, EDaysOfWeekExt.WeekDay }.HasDayGroupClassifier(), Is.True);
        }

        [Test]
        public void HasDayGroupClassifierFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Monday, EDaysOfWeekExt.Saturday }.HasDayGroupClassifier(), Is.False);
        }

        [Test]
        public void HasDayGroupClassifierAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasDayGroupClassifier(true), Is.True);
        }

        [Test]
        public void HasDayGroupClassifierAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasDayGroupClassifier(false), Is.False);
        }
        
        [Test]
        public void HasOnlyWeekDaysTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Tuesday, EDaysOfWeekExt.Wednesday }.HasOnlyWeekDays(), Is.True);
        }

        [Test]
        public void HasOnlyWeekDaysFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Thursday, EDaysOfWeekExt.Saturday }.HasOnlyWeekDays(), Is.False);
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasOnlyWeekDays(true), Is.True);
        }

        [Test]
        public void HasOnlyWeekDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasOnlyWeekDays(false), Is.False);
        }

        [Test]
        public void HasOnlyDayGroupClassifiersTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.WeekDay, EDaysOfWeekExt.WeekendDay }.HasOnlyDayGroupClassifiers, Is.True);
        }

        [Test]
        public void HasOnlyDayGroupClassifiersFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.EveryDay }.HasOnlyDayGroupClassifiers(), Is.False);
        }

        [Test]
        public void HasOnlyDayGroupClassifiersAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasOnlyDayGroupClassifiers(true), Is.True);
        }

        [Test]
        public void HasOnlyDayGroupClassifiersAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasOnlyDayGroupClassifiers(false), Is.False);
        }

        [Test]
        public void HasOnlyWeeendkDaysTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.Saturday }.HasOnlyWeekendDays(), Is.True);
        }

        [Test]
        public void HasOnlyWeekendDaysFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>() { EDaysOfWeekExt.Sunday, EDaysOfWeekExt.Monday }.HasOnlyWeekendDays(), Is.False);
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyTrue()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasOnlyWeekendDays(true), Is.True);
        }

        [Test]
        public void HasOnlyWeekendDaysAllowEmptyFalse()
        {
            Assert.That(new List<EDaysOfWeekExt>().HasOnlyWeekendDays(false), Is.False);
        }

        [Test]
        public void ConvertFromDaysOfWeek()
        {
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Monday), Is.EqualTo(EDaysOfWeekExt.Monday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Tuesday), Is.EqualTo(EDaysOfWeekExt.Tuesday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Wednesday), Is.EqualTo(EDaysOfWeekExt.Wednesday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Thursday), Is.EqualTo(EDaysOfWeekExt.Thursday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Friday), Is.EqualTo(EDaysOfWeekExt.Friday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Saturday), Is.EqualTo(EDaysOfWeekExt.Saturday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeek.Sunday), Is.EqualTo(EDaysOfWeekExt.Sunday));
            Assert.That(() => DaysOfWeekExt.ConvertFrom((EDaysOfWeek)int.MaxValue), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConvertFromEDaysOfWeekFlags()
        {
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Monday).Single(), Is.EqualTo(EDaysOfWeekExt.Monday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Tuesday).Single(), Is.EqualTo(EDaysOfWeekExt.Tuesday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Wednesday).Single(), Is.EqualTo(EDaysOfWeekExt.Wednesday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Thursday).Single(), Is.EqualTo(EDaysOfWeekExt.Thursday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Friday).Single(), Is.EqualTo(EDaysOfWeekExt.Friday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Saturday).Single(), Is.EqualTo(EDaysOfWeekExt.Saturday));
            Assert.That(DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Sunday).Single(), Is.EqualTo(EDaysOfWeekExt.Sunday));
        }

        [Test]
        public void ConvertFromEDaysOfWeekFlagsWithMultipleDays()
        {
            HashSet<EDaysOfWeekExt> weekendDays = DaysOfWeekExt.ConvertFrom(EDaysOfWeekFlags.Saturday ^ EDaysOfWeekFlags.Sunday);
            Assert.That(weekendDays, Is.Not.Empty);
            Assert.That(weekendDays.Count, Is.EqualTo(2));
            Assert.That(weekendDays.Contains(EDaysOfWeekExt.Saturday), Is.True);
            Assert.That(weekendDays.Contains(EDaysOfWeekExt.Sunday), Is.True);
        }

        [Test]
        public void ConvertFromDayOfWeek()
        {
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Monday), Is.EqualTo(EDaysOfWeekExt.Monday));
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Tuesday), Is.EqualTo(EDaysOfWeekExt.Tuesday));
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Wednesday), Is.EqualTo(EDaysOfWeekExt.Wednesday));
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Thursday), Is.EqualTo(EDaysOfWeekExt.Thursday));
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Friday), Is.EqualTo(EDaysOfWeekExt.Friday));
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Saturday), Is.EqualTo(EDaysOfWeekExt.Saturday));
            Assert.That(DaysOfWeekExt.ConvertFrom(DayOfWeek.Sunday), Is.EqualTo(EDaysOfWeekExt.Sunday));
            Assert.That(() => DaysOfWeekExt.ConvertFrom((DayOfWeek)int.MaxValue), Throws.TypeOf<ArgumentException>());
        }
    }
}
