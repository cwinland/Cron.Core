using Cron.Core.Enums;
using Cron.Core.Interfaces;

namespace Cron.Core.Sections
{
    /// <inheritdoc cref="IDateSection"/>
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
