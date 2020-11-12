using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Cron.Core.Enums;

namespace Cron.Core.Interfaces
{
    /// <summary>
    ///   Cron Interface Object.
    /// </summary>
    public interface ICron : IEnumerable
    {
        /// <summary>
        ///   Human readable description.
        /// </summary>
        /// <example>
        ///   Every 22 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 12 of the month, only on
        ///   Wednesday, only in March and May, every 5 years
        /// </example>
        string Description { get; }

        /// <summary>
        ///   Cron Expression.
        /// </summary>
        /// <example>*/22 */3,4 3-5,7-11 12 3,5 3 */5</example>
        string Value { get; }

        /// <summary>
        ///   Day of the Month Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection DayMonth { get; }

        /// <summary>
        ///   Day of the Week Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection DayWeek { get; }

        /// <summary>
        ///   Hours Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection Hours { get; }

        /// <summary>
        ///   Minutes Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection Minutes { get; }

        /// <summary>
        ///   Months Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection Months { get; }

        /// <summary>
        ///   Seconds Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection Seconds { get; }

        /// <summary>
        ///   Year Information
        /// </summary>
        /// <returns><see cref="ISection" /></returns>
        ISection Years { get; }

        /// <summary>
        ///   Add time value for the specified time section.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// <param name="repeatEvery">Indicates if the value is a repeating time or specific time.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronTimeSections time, [Range(0, 9999)] int value, bool repeatEvery = false);

        /// <summary>
        ///   Add time value for the specified time section.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="minValue">Starting value for the specified time section.</param>
        /// <param name="maxValue">Ending value for the specified time section.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronTimeSections time, [Range(0, 9999)] int minValue, [Range(0, 9999)] int maxValue);

        /// <summary>
        ///   Add day of the week.
        /// </summary>
        /// <param name="value"><see cref="CronDays" /> representing the day of the week.</param>
        /// <param name="repeatEvery">Indicates if the value is a repeating day or specific day.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronDays value, bool repeatEvery = false);

        /// <summary>
        ///   Add month.
        /// </summary>
        /// <param name="value"><see cref="CronMonths" /> representing the month.</param>
        /// <param name="repeatEvery">Indicates if the value is a repeating month or specific month.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronMonths value, bool repeatEvery = false);

        /// <summary>
        ///   Add range of days in the week.
        /// </summary>
        /// <param name="minValue">Starting <see cref="CronDays" /> representing the day of the week.</param>
        /// <param name="maxValue">Ending <see cref="CronDays" /> representing the day of the week.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronDays minValue, CronDays maxValue);

        /// <summary>
        ///   Add range of months.
        /// </summary>
        /// <param name="minValue">Starting <see cref="CronMonths" /> representing the day of the month.</param>
        /// <param name="maxValue">Ending <see cref="CronMonths" /> representing the day of the month.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronMonths minValue, CronMonths maxValue);

        /// <summary>
        ///   Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        void Remove(CronTimeSections time, int value);

        /// <summary>
        ///   Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="minValue">Starting value for the specified time section.</param>
        /// <param name="maxValue">Ending value for the specified time section.</param>
        void Remove(CronTimeSections time, int minValue, int maxValue);

        /// <summary>
        ///   Clear the specific time element of the Cron object to defaults without any numerical cron representations.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Reset(CronTimeSections time);

        /// <summary>
        ///   Clear the entire Cron object to defaults without any numerical cron representations.
        /// </summary>
        /// <returns><see cref="ICron" /></returns>
        ICron ResetAll();

        /// <summary>
        ///   Set time with <see cref="ISection" /> value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Set(CronTimeSections time, ISection value);
    }
}
