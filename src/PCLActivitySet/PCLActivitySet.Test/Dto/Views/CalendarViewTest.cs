using LiteDB;
using NUnit.Framework;
using PCLActivitySet.Domain.Views;
using PCLActivitySet.Dto.Views;
using PCLActivitySet.Test.Helpers;
using Ploeh.AutoFixture;
using System.IO;
using FluentAssertions;

namespace PCLActivitySet.Test.Dto.Views
{
    [TestFixture]
    public class CalendarViewTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            CalendarViewDto sourceDto = fixture.Create<CalendarViewDto>();
            CalendarView domain = CalendarView.FromDto(sourceDto);
            CalendarViewDto targetDto = CalendarView.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            CalendarViewDto sourceDto = fixture.Create<CalendarViewDto>();
            CalendarViewDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<CalendarViewDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }
    }
}
