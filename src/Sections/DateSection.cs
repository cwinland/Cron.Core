// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
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
    /// Class DateSection.
    /// Implements the <see cref="Cron.Core.Sections.Section" />
    /// Implements the <see cref="Cron.Core.Interfaces.IDateSection" />
    /// </summary>
    /// <seealso cref="Cron.Core.Sections.Section" />
    /// <seealso cref="Cron.Core.Interfaces.IDateSection" />
    /// <inheritdoc cref="IDateSection" />
    public class DateSection : Section, IDateSection
    {
        /// <inheritdoc />
        protected internal DateSection(CronTimeSections time, string expression) : base(time, expression) { }

        /// <inheritdoc />
        public override bool Every => false;

        /// <inheritdoc />
        protected internal DateSection(CronTimeSections time) : base(time) { }
    }
}
