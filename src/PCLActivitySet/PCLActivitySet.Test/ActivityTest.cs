using System;
using System.Linq;
using NUnit.Framework;
using PCLActivitySet.Recurrence;

namespace PCLActivitySet.Test
{
    [TestFixture]
    public class ActivityTest
    {
        [Test]
        public void NamePropertyDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.Name, Is.Null);
        }

        [Test]
        public void NamePropertyIsReadWrite()
        {
            var activity = new Activity();
            string testName = "Test Name";
            activity.Name = testName;
            Assert.That(activity.Name, Is.EqualTo(testName));
        }

        [Test]
        public void ActiveDueDateDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.ActiveDueDate, Is.Null);
        }

        [Test]
        public void ActiveDueDatePropertyIsReadWrite()
        {
            var activity = new Activity();
            activity.ActiveDueDate = DateTime.MaxValue;
            Assert.That(activity.ActiveDueDate, Is.EqualTo(DateTime.MaxValue));
        }

        [Test]
        public void LeadTimePropertyDefaultsToNull()
        {
            var activity = new Activity();
            Assert.That(activity.LeadTime, Is.Null);
        }

        [Test]
        public void LeadTimePropertyIsReadWrite()
        {
            var activity = new Activity();
            IDateProjection projection = new WeeklyProjection();
            activity.LeadTime = projection;
            Assert.That(activity.LeadTime, Is.SameAs(projection));
        }

        [Test]
        public void LeadTimeDateIsNullWhenActiveDueDateIsNull()
        {
            var activity = new Activity();
            IDateProjection leadTimeProjection = new DailyProjection() { DayCount = 7 };
            activity.ActiveDueDate = null;
            activity.LeadTime = leadTimeProjection;
            Assert.That(activity.LeadTimeDate, Is.Null);
        }
        [Test]
        public void LeadTimeDateIsNullWhenLeadTimeIsNull()
        {
            var activity = new Activity();
            activity.ActiveDueDate = new DateTime(2017, 2, 8);
            activity.LeadTime = null;
            Assert.That(activity.LeadTimeDate, Is.Null);
        }
        [Test]
        public void LeadTimeDateVerified()
        {
            var activity = new Activity();
            IDateProjection leadTimeProjection = new DailyProjection() { DayCount = 7 };
            activity.ActiveDueDate = new DateTime(2017, 2, 8);
            activity.LeadTime = leadTimeProjection;
            Assert.That(activity.LeadTime, Is.SameAs(leadTimeProjection));
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 1)));
        }

        [Test]
        public void FluentlyCreateAndImplicitlyConvertToActivity()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName);
            Assert.That(activity.Name, Is.EqualTo(activityName));
        }

        [Test]
        public void ConvertActivityToFluentSyntax()
        {
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.DailyLeadTime(3);
            Assert.That(activity.LeadTime.GetTranslator().PeriodCount, Is.EqualTo(3));
        }

        [Test]
        public void FluentlyChangeName()
        {
            string newName = "New Name";
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.Name(newName);
            Assert.That(activity.Name, Is.EqualTo(newName));
        }

        [Test]
        public void FluentlyChangeActiveDueDate()
        {
            DateTime newDueDate = new DateTime(2017, 3, 21);
            var activity = new Activity() { Name = "New Activity", ActiveDueDate = new DateTime(2017, 2, 28) };
            activity.Fluently.ActiveDueDate(newDueDate);
            Assert.That(activity.ActiveDueDate, Is.EqualTo(newDueDate));
        }
        [Test]
        public void FluentlyCreateAndSetName()
        {
            string activityName = "New Activity";
            Activity activity = Activity.FluentNew(activityName).ToActivity;
            Assert.That(activity.Name, Is.EqualTo(activityName));
        }

        [Test]
        public void FluentlyCreateAndSetNameAndActiveDueDate()
        {
            string activityName = "New Activity";
            DateTime activityActiveDueDate = new DateTime(2017, 2, 28);
            Activity activity = Activity.FluentNew(activityName, activityActiveDueDate);
            Assert.That(activity.Name, Is.EqualTo(activityName));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(activityActiveDueDate));
        }

        [Test]
        public void FluentlyAddWithLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .LeadTime(new WeeklyProjection() { DaysOfWeek = EDaysOfWeekFlags.Monday, WeekCount = 1 });
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyAddWithDailyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .DailyLeadTime(7);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 21)));
        }

        [Test]
        public void FluentlyAddWithWeeklyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .WeeklyLeadTime(1, EDaysOfWeekFlags.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 27)));
        }

        [Test]
        public void FluentlyAddWithMonthlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .MonthlyLeadTime(3, 8);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 12, 8)));
        }

        [Test]
        public void FluentlyAddWithMonthlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .MonthlyLeadTime(1, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2017, 2, 13)));
        }

        [Test]
        public void FluentlyAddWithYearlyLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.March, 8);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 3, 8)));
        }

        [Test]
        public void FluentlyAddWithYearlyRelativeLeadTime()
        {
            Activity activity = Activity.FluentNew("New Activity", new DateTime(2017, 2, 28))
                .YearlyLeadTime(EMonth.April, EWeeksInMonth.Second, EDaysOfWeekExt.Monday);
            Assert.That(activity.LeadTimeDate, Is.EqualTo(new DateTime(2016, 4, 11)));
        }

        [Test]
        public void FluentlyCreateAndAddToActivitySet()
        {
            var activitySet = new ActivitySet();
            string activityName = "Activity Name";
            DateTime? activityActiveDueDate = new DateTime(2017,2,28);
            Activity.FluentNew(activityName, activityActiveDueDate)
                .AddTo(activitySet);
            Assert.That(activitySet.Count, Is.EqualTo(1));
            var activity = activitySet.First();
            Assert.That(activity.Name, Is.EqualTo(activityName));
            Assert.That(activity.ActiveDueDate, Is.EqualTo(activityActiveDueDate));
        }
    }
}
