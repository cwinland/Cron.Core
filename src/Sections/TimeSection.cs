// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-12-2020
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
    /// Class TimeSection.
    /// Implements the <see cref="Cron.Core.Sections.Section" />
    /// Implements the <see cref="Cron.Core.Interfaces.ITimeSection" />
    /// </summary>
    /// <seealso cref="Cron.Core.Sections.Section" />
    /// <seealso cref="Cron.Core.Interfaces.ITimeSection" />
    /// <inheritdoc cref="ITimeSection" />
    public class TimeSection : Section, ITimeSection
    {
        /// <inheritdoc />
        protected internal TimeSection(CronTimeSections time, string expression) : base(time, expression) { }

        /// <inheritdoc />
        protected internal TimeSection(CronTimeSections time) : base(time) { }

        /// <inheritdoc />
        public List<int> AllowedIncrements =>
            time == CronTimeSections.Hours
                ? new List<int>()
                {
                    2,
                    3,
                    4,
                    6,
                    8,
                    12,
                }
                : new List<int>()
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
