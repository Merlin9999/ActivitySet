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
    public class InBoxActivityListTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            InBoxActivityListDto sourceDto = fixture.Create<InBoxActivityListDto>();
            InBoxActivityList domain = InBoxActivityList.FromDto(sourceDto, owningBoard: null);
            InBoxActivityListDto targetDto = InBoxActivityList.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndDomainRoundTripNulls()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            InBoxActivityListDto sourceDto = fixture.Create<InBoxActivityListDto>();
            sourceDto.InternalCalendarView = null;
            sourceDto.InternalExcludeNonActiveView = null;
            sourceDto.InternalFocusDateView = null;
            InBoxActivityList domain = InBoxActivityList.FromDto(sourceDto, owningBoard: null);
            InBoxActivityListDto targetDto = InBoxActivityList.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            InBoxActivityListDto sourceDto = fixture.Create<InBoxActivityListDto>();
            InBoxActivityListDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<InBoxActivityListDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }
    }
}
