
using System;
using LiteDB;
using NUnit.Framework;
using PCLActivitySet.Domain.Views;
using PCLActivitySet.Dto.Views;
using PCLActivitySet.Test.Helpers;
using Ploeh.AutoFixture;
using System.IO;
using FluentAssertions;

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

            const string exceptionMessage =
                "No members were found for comparison. Please specify some members to include in the comparison or choose a more meaningful assertion.";
            Action action = () => sourceDto.ShouldBeEquivalentTo(targetDto);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage(exceptionMessage);
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

            const string exceptionMessage =
                "No members were found for comparison. Please specify some members to include in the comparison or choose a more meaningful assertion.";
            Action action = () => sourceDto.ShouldBeEquivalentTo(targetDto);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

    }
}
