using Cron.Core.Attributes;

namespace Cron.Core.Enums
{
    public enum CronType
    {
        [Range<int>(0, 59)]
        Minute,
        [Range<int>(0, 23)]
        Hour,
        [Range<int>(1, 31)]
        Day,
        [Range<int>(1,12)]
        [Range<CronMonths>(CronMonths.January, CronMonths.December)]
        Month,
        [Range<CronDays>(CronDays.Sunday, CronDays.Saturday)]
        [Range<int>(0,6)]
        Weekday
    }
}
