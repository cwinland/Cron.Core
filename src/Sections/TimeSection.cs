// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="TimeSection.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Cron.Core.Enums;
using Cron.Core.Interfaces;

namespace Cron.Core.Sections
{
    /// <summary>
    /// Class TimeSection.
    /// Implements the <see cref="Section" />
    /// Implements the <see cref="ITimeSection" />
    /// </summary>
    /// <seealso cref="Section" />
    /// <seealso cref="ITimeSection" />
    /// <inheritdoc cref="ITimeSection" />
    public class TimeSection : Section, ITimeSection
    {
        /// <inheritdoc />
        protected internal TimeSection(CronTimeSections time, string expression) : base(time, expression) { }

        /// <inheritdoc />
        protected internal TimeSection(CronTimeSections time) : base(time) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSection"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        protected internal TimeSection(CronTimeSections time, bool enabled) : this(time)
        {
            Enabled = enabled;
        }

        /// <inheritdoc />
        public override bool IsValidRange(int value) => base.IsValidRange(value) && (!Every || AllowedIncrements.Contains(value));

        /// <inheritdoc />
        public List<int> AllowedIncrements =>
            Time == CronTimeSections.Hours
                ? new List<int>
                {
                    2,
                    3,
                    4,
                    6,
                    8,
                    12,
                }
                : new List<int>
                {
                    2,
                    3,
                    4,
                    5,
                    6,
                    10,
                    12,
                    15,
                    20,
                    30,
                };
    }
}
