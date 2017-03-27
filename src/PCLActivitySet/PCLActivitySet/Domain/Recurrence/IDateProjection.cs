using System;

namespace PCLActivitySet.Domain.Recurrence
{
    public interface IDateProjection
    {
        EDateProjectionType ProjectionType { get; }

        DateTime GetNext(DateTime fromDate);
        DateTime GetPrevious(DateTime fromDate);

        IDateProjectionTranslator GetTranslator();
    }
}
