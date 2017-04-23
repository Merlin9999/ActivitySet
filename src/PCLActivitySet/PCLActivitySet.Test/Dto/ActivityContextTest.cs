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
    public class ActivityContextTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ActivityContextDto sourceDto = fixture.Create<ActivityContextDto>();
            ActivityContext domain = ActivityContext.FromDto(sourceDto);
            ActivityContextDto targetDto = ActivityContext.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            ActivityContextDto sourceDto = fixture.Create<ActivityContextDto>();
            ActivityContextDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<ActivityContextDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }
    }
}
