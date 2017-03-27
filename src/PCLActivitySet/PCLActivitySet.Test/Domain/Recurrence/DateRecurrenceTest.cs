using System;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;

namespace PCLActivitySet.Test.Domain.Recurrence
{
    [TestFixture]
    public class DateRecurrenceTest
    {
        [Test]
        public void DefaultConstructor()
        {
            var recur = new DateRecurrence();
            recur.DateProjectionImpl.GetTranslator().PeriodCount = 1;
            DateTime nextDate = recur.GetNext(new DateTime(2017, 2, 28));
            Assert.That(nextDate, Is.EqualTo(new DateTime(2017, 3, 1)));
        }

        [Test]
        public void ProjectionTypeAndRecurFromTypeConstructor()
        {
            var recur = new DateRecurrence(EDateProjectionType.Monthly, ERecurFromType.FromCompletedDate);
            recur.DateProjectionImpl.GetTranslator().PeriodCount = 1;
            recur.DateProjectionImpl.GetTranslator().DayOfMonth = 29;
            DateTime? nextDate = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0);
            Assert.That(nextDate, Is.EqualTo(new DateTime(2017, 3, 29)));
        }

        [Test]
        public void ProjectionTypeAndRecurFromTypeAndMaxOccurenceConstructor()
        {
            var recur = new DateRecurrence(EDateProjectionType.Monthly, ERecurFromType.FromCompletedDate, 1);
            recur.DateProjectionImpl.GetTranslator().PeriodCount = 1;
            recur.DateProjectionImpl.GetTranslator().DayOfMonth = 29;

            DateTime? nextDate1 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0);
            Assert.That(nextDate1, Is.EqualTo(new DateTime(2017, 3, 29)));

            DateTime? nextDate2 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 1);
            Assert.That(nextDate2, Is.Null);
        }

        [Test]
        public void ProjectionTypeAndRecurFromTypeAndDateRangeConstructor()
        {
            var startDate = new DateTime(2017, 2, 1);
            var endDate = new DateTime(2017, 4, 1);
            var recur = new DateRecurrence(EDateProjectionType.Monthly, ERecurFromType.FromCompletedDate, startDate, endDate);
            recur.DateProjectionImpl.GetTranslator().PeriodCount = 1;
            recur.DateProjectionImpl.GetTranslator().DayOfMonth = 29;

            DateTime? nextDate1 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0);
            Assert.That(nextDate1, Is.EqualTo(new DateTime(2017, 3, 29)));

            DateTime? nextDate2 = recur.GetNext(new DateTime(), new DateTime(2017, 3, 29), 1);
            Assert.That(nextDate2, Is.Null);
        }

        [Test]
        public void ProjectionTypeAndRecurFromTypeAndBadDateRangeConstructor()
        {
            var startDate = new DateTime(2017, 4, 1);
            var endDate = new DateTime(2017, 2, 1);
            Assert.That(() => new DateRecurrence(EDateProjectionType.Monthly, ERecurFromType.FromCompletedDate, startDate, endDate), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void DateProjectionAndRecurFromTypeConstructor()
        {
            var recur = new DateRecurrence(new DailyProjection() { DayCount = 1 }, ERecurFromType.FromCompletedDate);

            DateTime? nextDate1 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0);
            Assert.That(nextDate1, Is.EqualTo(new DateTime(2017, 3, 1)));
        }

        [Test]
        public void DateProjectionAndRecurFromTypeAndMaxOccurenceConstructor()
        {
            var recur = new DateRecurrence(new DailyProjection() { DayCount = 1 }, ERecurFromType.FromCompletedDate, 1);

            DateTime? nextDate1 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0);
            Assert.That(nextDate1, Is.EqualTo(new DateTime(2017, 3, 1)));

            DateTime? nextDate2 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 1);
            Assert.That(nextDate2, Is.Null);
        }

        [Test]
        public void DateProjectionAndRecurFromTypeAndDateRangeConstructor()
        {
            var startDate = new DateTime(2017, 2, 1);
            var endDate = new DateTime(2017, 3, 1);
            var recur = new DateRecurrence(new DailyProjection() { DayCount = 1 }, ERecurFromType.FromCompletedDate, startDate, endDate);

            DateTime? nextDate1 = recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0);
            Assert.That(nextDate1, Is.EqualTo(new DateTime(2017, 3, 1)));

            DateTime? nextDate2 = recur.GetNext(new DateTime(), new DateTime(2017, 3, 1), 0);
            Assert.That(nextDate2, Is.Null);
        }

        [Test]
        public void DateProjectionAndRecurFromTypeAndBadDateRangeConstructor()
        {
            var startDate = new DateTime(2017, 3, 1);
            var endDate = new DateTime(2017, 2, 1);
            Assert.That(() => new DateRecurrence(new DailyProjection() { DayCount = 1 }, ERecurFromType.FromCompletedDate, startDate, endDate), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void BadRecurFromTypeError()
        {
            var recur = new DateRecurrence(new DailyProjection() { DayCount = 1 }, (ERecurFromType)int.MaxValue);
            Assert.That(() => recur.GetNext(new DateTime(), new DateTime(2017, 2, 28), 0), Throws.TypeOf<InvalidOperationException>());
        }
    }
}
