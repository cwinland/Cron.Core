using System;
using System.Collections.Generic;
using System.Text;

namespace Cron.Enums
{
    /// <summary>
    ///   Sections of the Cron indicating the type of time.
    /// </summary>
    public enum CronTimeSections
    {
        Seconds = 1,
        Minutes = 2,
        Hours = 3,
        DayMonth = 4,
        DayWeek = 5,
        Months = 6,
        Years = 7,
    }
}
