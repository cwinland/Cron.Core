// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-17-2020
// ***********************************************************************
// <copyright file="DateSection.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************

using Cron.Core.Enums;
using Cron.Core.Interfaces;

namespace Cron.Core.Sections
{
    /// <summary>
    ///     Class DateSection.
    /// Implements the <see cref="Section{T}" />
    /// Implements the <see cref="IDateSection" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Section{T}" />
    /// <seealso cref="IDateSection" />
    /// <inheritdoc cref="IDateSection" />
    public class DateSection<T> : Section<T>, IDateSection
    {
        /// <inheritdoc />
        protected internal DateSection(CronTimeSections time, string expression) : base(time, expression)
        {
            Every = false;
        }

        /// <inheritdoc />
        protected internal DateSection(CronTimeSections time) : base(time)
        {
            Every = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimeSection{T}" /> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        protected internal DateSection(CronTimeSections time, bool enabled) : this(time)
        {
            Enabled = enabled;
        }
    }
}
