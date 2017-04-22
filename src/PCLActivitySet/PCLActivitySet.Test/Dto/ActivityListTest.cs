using LiteDB;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;
using Ploeh.AutoFixture;
using System.IO;
using FluentAssertions;
using PCLActivitySet.Domain;
using PCLActivitySet.Dto;
using PCLActivitySet.Test.Helpers;

namespace PCLActivitySet.Test.Dto
{
    [TestFixture]
    public class ActivityListTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ActivityListDto sourceDto = fixture.Create<ActivityListDto>();
            ActivityList domain = ActivityList.FromDto(sourceDto, owningBoard: null);
            ActivityListDto targetDto = ActivityList.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndDomainRoundTripNulls()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ActivityListDto sourceDto = fixture.Create<ActivityListDto>();
            sourceDto.InternalCalendarView = null;
            sourceDto.InternalExcludeNonActiveView = null;
            sourceDto.InternalFocusDateView = null;
            ActivityList domain = ActivityList.FromDto(sourceDto, owningBoard: null);
            ActivityListDto targetDto = ActivityList.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            ActivityListDto sourceDto = fixture.Create<ActivityListDto>();
            ActivityListDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<ActivityListDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }
    }
}
