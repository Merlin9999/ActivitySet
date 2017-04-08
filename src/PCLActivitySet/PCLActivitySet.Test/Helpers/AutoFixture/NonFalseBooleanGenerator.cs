using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Ploeh.AutoFixture.Kernel;

namespace PCLActivitySet.Test.Helpers.AutoFixture
{
    public class NonFalseBooleanGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return IsNotBoolRequest(request)
#pragma warning disable 618
                       ? new NoSpecimen(request)
#pragma warning restore 618
                       : (object)true;
        }

        private static bool IsNotBoolRequest(object request)
        {
            return !typeof(Boolean).IsAssignableFrom(request as Type);
        }
    }
}
