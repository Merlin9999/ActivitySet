
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
    class ExcludeNonActiveViewTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            ExcludeNonActiveViewDto sourceDto = fixture.Create<ExcludeNonActiveViewDto>();
            ExcludeNonActiveView domain = ExcludeNonActiveView.FromDto(sourceDto);
            ExcludeNonActiveViewDto targetDto = ExcludeNonActiveView.ToDto(domain);

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<ExcludeNonActiveViewDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            ExcludeNonActiveViewDto sourceDto = fixture.Create<ExcludeNonActiveViewDto>();
            ExcludeNonActiveViewDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<ExcludeNonActiveViewDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<ExcludeNonActiveViewDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

    }
}
