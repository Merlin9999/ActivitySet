using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using NUnit.Framework;
using PCLActivitySet.Data.Recurrence;
using PCLActivitySet.Domain.Recurrence;
using PCLActivitySet.Test.AutoFixtureCustomizations;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;

namespace PCLActivitySet.Test.Data
{
    [TestFixture]
    public class DateProjectionTest
    {
        private ICustomization asdf;
        private IFixture qwer;

        [Test]
        public void DtoToDomainToDtoRoundTrip()
        {
            Fixture fixture = new Fixture();
            fixture.Customizations.Insert(0, new NonZeroEnumGenerator());

            DateProjectionDto sourceDto = fixture.Create<DateProjectionDto>();
            DateProjection domain = DateProjection.FromDto(sourceDto);
            DateProjectionDto targetDto = domain.ToDto();

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<DateProjectionDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }

        [Test]
        public void DtoToLiteDbToDtoRoundTrip()
        {
            Fixture fixture = new Fixture();
            fixture.Customizations.Insert(0, new NonZeroEnumGenerator());
            DateProjectionDto sourceDto = fixture.Create<DateProjectionDto>();
            DateProjectionDto targetDto;
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var col = db.GetCollection<DateProjectionDto>();
                var id = col.Insert(sourceDto);
                targetDto = col.FindById(id);
            }

            var sourceDtoLikeness = sourceDto.AsSource().OfLikeness<DateProjectionDto>();
            sourceDtoLikeness.ShouldEqual(targetDto);
        }
    }
}
