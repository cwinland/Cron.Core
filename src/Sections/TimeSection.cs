using Cron.Core.Enums;
using Cron.Core.Interfaces;
using System.Collections.Generic;

namespace Cron.Core.Sections
{
    /// <inheritdoc cref="ITimeSection"/>
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
