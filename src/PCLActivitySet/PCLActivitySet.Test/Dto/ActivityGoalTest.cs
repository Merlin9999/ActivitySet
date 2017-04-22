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
    public class ActivityGoalTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ActivityGoalDto sourceDto = fixture.Create<ActivityGoalDto>();
            ActivityGoal domain = ActivityGoal.FromDto(sourceDto);
            ActivityGoalDto targetDto = ActivityGoal.ToDto(domain);

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            ActivityGoalDto sourceDto = fixture.Create<ActivityGoalDto>();
            ActivityGoalDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<ActivityGoalDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            sourceDto.ShouldBeEquivalentTo(targetDto);
        }
    }
}
