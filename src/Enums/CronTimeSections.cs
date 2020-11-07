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
        /// <summary>
        ///   Seconds.
        /// </summary>
        Seconds = 1,

        /// <summary>
        ///   Minutes.
        /// </summary>
        Minutes = 2,

        /// <summary>
        ///   Hours.
        /// </summary>
        Hours = 3,

        /// <summary>
        ///   Day of the Month.
        /// </summary>
        DayMonth = 4,

        /// <summary>
        ///   Day of the Week.
        /// </summary>
        DayWeek = 5,

        /// <summary>
        ///   Month.
        /// </summary>
        Months = 6,

        /// <summary>
        ///   Year or interval of years.
        /// </summary>
        /// <example>2020, 2021.</example>
        /// <example>*/2 (Every two years).</example>
        Years = 7,
    }
}
