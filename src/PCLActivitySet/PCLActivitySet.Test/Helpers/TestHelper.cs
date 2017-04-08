using System;
using PCLActivitySet.Test.Helpers.AutoFixture;
using Ploeh.AutoFixture;

namespace PCLActivitySet.Test.Helpers
{
    public class TestHelper
    {
        public static Fixture CreateSerializationAutoFixture(bool useLiteDBCompatibleDateTime = false)
        {
            Fixture fixture = new Fixture();
            fixture.Customizations.Insert(0, new NonZeroEnumGenerator());
            fixture.Customizations.Insert(0, new NullObjectIdGenerator());
            fixture.Customizations.Insert(0, new NonFalseBooleanGenerator());
            if (useLiteDBCompatibleDateTime)
                fixture.Customizations.Insert(0, new CustomRandomDateTimeSequenceGenerator());
            return fixture;
        }
    }
}
