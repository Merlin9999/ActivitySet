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
    public class ActivityTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ActivityDto sourceDto = fixture.Create<ActivityDto>();
            Activity domain = Activity.FromDto(sourceDto);
            ActivityDto targetDto = Activity.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndDomainRoundTripNulls()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ActivityDto sourceDto = fixture.Create<ActivityDto>();
            sourceDto.LeadTime = null;
            sourceDto.Recurrence = null;
            sourceDto.CompletionHistory = null;

            Activity domain = Activity.FromDto(sourceDto);
            ActivityDto targetDto = Activity.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            ActivityDto sourceDto = fixture.Create<ActivityDto>();
            ActivityDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<ActivityDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);

                Assert.That(targetDto.Id, Is.EqualTo(id));
            }

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }
    }
}
