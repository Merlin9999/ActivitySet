﻿using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

// Tweaked implementation of Ploeh's code from GitHub:
// https://github.com/AutoFixture/AutoFixture

namespace PCLActivitySet.Test.Helpers.AutoFixture
{
    /// <summary>
    /// Creates random <see cref="DateTime"/> specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="DateTime"/> values will be within
    /// a range of ± two years from today's date,
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class CustomRandomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly bool _forceZeroMilliseconds;
        private readonly DateTimeKind? _kindDefault;
        private readonly RandomNumericSequenceGenerator randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRandomDateTimeSequenceGenerator"/> class.
        /// </summary>
        public CustomRandomDateTimeSequenceGenerator(bool forceZeroMilliseconds = false, DateTimeKind? kindDefault = null)
            : this(DateTime.Today.AddYears(-2), DateTime.Today.AddYears(2))
        {
            this._forceZeroMilliseconds = forceZeroMilliseconds;
            this._kindDefault = kindDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRandomDateTimeSequenceGenerator"/> class
        /// for a specific range of dates.
        /// </summary>
        /// <param name="minDate">The lower bound of the date range.</param>
        /// <param name="maxDate">The upper bound of the date range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
        /// </exception>
        public CustomRandomDateTimeSequenceGenerator(DateTime minDate, DateTime maxDate, 
            bool forceZeroMilliseconds = false, DateTimeKind? kindDefault = null)
        {
            this._forceZeroMilliseconds = forceZeroMilliseconds;
            this._kindDefault = kindDefault;

            if (minDate >= maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");
            }

            this.randomizer = new RandomNumericSequenceGenerator(minDate.Ticks, maxDate.Ticks);
        }

        /// <summary>
        /// Creates a new <see cref="DateTime"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="DateTime"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return IsNotDateTimeRequest(request)
#pragma warning disable 618
                       ? new NoSpecimen(request)
#pragma warning restore 618
                       : this.CreateRandomDate(context);
        }

        private static bool IsNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate(ISpecimenContext context)
        {
            DateTime val = new DateTime(this.GetRandomNumberOfTicks(context));
            int milliseconds = this._forceZeroMilliseconds ? 0 : val.Millisecond;
            DateTimeKind kind = this._kindDefault == null ? DateTimeKind.Unspecified : this._kindDefault.Value;
            val = new DateTime(val.Year, val.Month, val.Day, val.Hour, val.Minute, val.Second, milliseconds, kind);
            return val;
        }

        private long GetRandomNumberOfTicks(ISpecimenContext context)
        {
            return (long)this.randomizer.Create(typeof(long), context);
        }
    }
} 
