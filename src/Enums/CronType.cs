using Cron.Core.Attributes;

namespace Cron.Core.Enums
{
    /// <summary>
    ///     Enum CronType
    /// </summary>
    public enum CronType
    {
        /// <summary>
        ///     The minute
        /// </summary>
        [Range<int>(0, 59)]
        Minute,
        /// <summary>
        ///     The hour
        /// </summary>
        [Range<int>(0, 23)]
        Hour,
        /// <summary>
        ///     The day
        /// </summary>
        [Range<int>(1, 31)]
        Day,
        /// <summary>
        ///     The month
        /// </summary>
        [Range<int>(1,12)]
        [Range<CronMonths>(CronMonths.January, CronMonths.December)]
        Month,
        /// <summary>
        ///     The weekday
        /// </summary>
        [Range<CronDays>(CronDays.Sunday, CronDays.Saturday)]
        [Range<int>(0,6)]
        Weekday
    }
}
