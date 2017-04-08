using System;
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
    public class FocusDateViewTest
    {
        [Test]
        public void DtoAndDomainRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture();

            FocusDateViewDto sourceDto = fixture.Create<FocusDateViewDto>();
            FocusDateView domain = FocusDateView.FromDto(sourceDto);
            FocusDateViewDto targetDto = FocusDateView.ToDto(domain);

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<FocusDateViewDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

        [Test]
        public void DtoAndLiteDbRoundTrip()
        {
            Fixture fixture = TestHelper.CreateSerializationAutoFixture(useLiteDBCompatibleDateTime: true);

            FocusDateViewDto sourceDto = fixture.Create<FocusDateViewDto>();
            FocusDateViewDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<FocusDateViewDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<FocusDateViewDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

    }
}
