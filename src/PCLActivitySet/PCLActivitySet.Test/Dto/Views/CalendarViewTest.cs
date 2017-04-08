using LiteDB;
using NUnit.Framework;
using PCLActivitySet.Domain.Views;
using PCLActivitySet.Dto.Views;
using PCLActivitySet.Test.Helpers;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;
using System.IO;

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

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<CalendarViewDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
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

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<CalendarViewDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

    }
}
