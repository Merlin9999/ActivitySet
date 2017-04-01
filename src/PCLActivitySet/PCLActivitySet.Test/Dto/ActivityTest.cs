using LiteDB;
using NUnit.Framework;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Dto.Recurrence;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;
using System.IO;
using PCLActivitySet.Domain;
using PCLActivitySet.Dto;
using PCLActivitySet.Test.Helpers;
using Ploeh.SemanticComparison;

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

            var sourceDtoLikeness = CreateLikeness(sourceDto);
            sourceDtoLikeness.ShouldEqual(targetDto);
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

            var sourceDtoLikeness = CreateLikeness(sourceDto);
            sourceDtoLikeness.ShouldEqual(targetDto);
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

            var sourceDtoLikeness = CreateLikeness(sourceDto);
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

        private static Likeness<ActivityDto, ActivityDto> CreateLikeness(ActivityDto sourceDto)
        {
            var historyComparer = new CollectionComparer<ActivityHistoryItemDto>();

            return sourceDto.AsSource().OfLikeness<ActivityDto>()
                .With(x => x.LeadTime)
                .EqualsWhen((x, y) => x.LeadTime.AsSource().OfLikeness<DateProjectionDto>().Equals(y.LeadTime))
                .With(x => x.Recurrence)
                .EqualsWhen((x, y) => x.Recurrence.AsSource().OfLikeness<DateRecurrenceDto>().Equals(y.Recurrence))
                .With(x => x.CompletionHistory)
                .EqualsWhen((x, y) => historyComparer.Equals(x.CompletionHistory, y.CompletionHistory));
        }
    }
}
