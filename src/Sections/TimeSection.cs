// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-17-2020
// ***********************************************************************
// <copyright file="TimeSection.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************

using Cron.Core.Enums;
using Cron.Core.Interfaces;
using System.Collections.Generic;

namespace Cron.Core.Sections
{
    /// <summary>
    ///     Class TimeSection.
    /// Implements the <see cref="Section{T}" />
    /// Implements the <see cref="ITimeSection" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Section{T}" />
    /// <seealso cref="ITimeSection" />
    /// <inheritdoc cref="ITimeSection" />
    public class TimeSection<T> : Section<T>, ITimeSection
    {
        /// <inheritdoc />
        protected internal TimeSection(CronTimeSections time, string expression) : base(time, expression) { }

        /// <inheritdoc />
        protected internal TimeSection(CronTimeSections time) : base(time) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimeSection{T}" /> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        protected internal TimeSection(CronTimeSections time, bool enabled) : this(time)
        {
            Enabled = enabled;
        }

        /// <inheritdoc />
        public override bool IsValidRange(int value) =>
            base.IsValidRange(value) && (!Every || AllowedIncrements.Contains(value));

        /// <inheritdoc />
        public List<int> AllowedIncrements => Time == CronTimeSections.Hours
            ? new List<int> { 2, 3, 4, 6, 8, 12 }
            : new List<int>
            {
                2, 3, 4, 5, 6, 10, 12, 15, 20, 30
            };
    }
}
