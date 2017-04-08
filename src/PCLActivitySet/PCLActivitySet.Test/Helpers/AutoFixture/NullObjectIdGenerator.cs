using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Ploeh.AutoFixture.Kernel;

namespace PCLActivitySet.Test.Helpers.AutoFixture
{
    public class NullObjectIdGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return IsNotObjectIdRequest(request)
#pragma warning disable 618
                       ? new NoSpecimen(request)
#pragma warning restore 618
                       : null;
        }

        private static bool IsNotObjectIdRequest(object request)
        {
            return !typeof(ObjectId).IsAssignableFrom(request as Type);
        }
    }
}
