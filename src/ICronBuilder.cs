using Cron.Core.Enums;
using System;

namespace Cron.Core
{
    /// <summary>
    ///     Interface ICronBuilder
    /// </summary>
    public interface ICronBuilder
    {
        /// <summary>
        ///     Adds the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="value">The value.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Add<T>(CronType cronType, T value);

        /// <summary>
        ///     Alls the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder All<T>(CronType cronType);

        /// <summary>
        ///     Anies the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Any<T>(CronType cronType);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>ICronExpression.</returns>
        ICronExpression Build();

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Clear();

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Clone();

        /// <summary>
        ///     Dailies this instance.
        /// </summary>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Daily();

        /// <summary>
        ///     Dailies the specified hour.
        /// </summary>
        /// <param name="hour">The hour.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Daily(int hour);

        /// <summary>
        ///     Dailies the specified minimum hour.
        /// </summary>
        /// <param name="minHour">The minimum hour.</param>
        /// <param name="maxHour">The maximum hour.</param>
        /// <param name="minute">The minute.</param>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Daily(int minHour, int maxHour, int minute);

        /// <summary>
        ///     Dailies the specified minimum hour.
        /// </summary>
        /// <param name="minHour">The minimum hour.</param>
        /// <param name="maxHour">The maximum hour.</param>
        /// <param name="minMinute">The minimum minute.</param>
        /// <param name="maxMinute">The maximum minute.</param>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Daily(int minHour, int maxHour, int minMinute, int maxMinute);

        /// <summary>
        ///     Dailies the specified hour.
        /// </summary>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Daily(int hour, int minute);

        /// <summary>
        ///     Dailies the specified hours.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Daily(params int[] hours);

        /// <summary>
        ///     Everies this instance.
        /// </summary>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Every();

        /// <summary>
        ///     Everies the specified cron type.
        /// </summary>
        /// <param name="cronType">Type of the cron.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Every(CronType cronType);

        /// <summary>
        ///     Everies the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="subCronType">Type of the sub cron.</param>
        /// <param name="subCronTypeValue">The sub cron type value.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Every<T>(CronType cronType, CronType subCronType, T subCronTypeValue);

        /// <summary>
        ///     Everies the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="subCronType">Type of the sub cron.</param>
        /// <param name="subCronTypeValues">The sub cron type values.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Every<T>(CronType cronType, CronType subCronType, params T[] subCronTypeValues);

        /// <summary>
        ///     Hourlies this instance.
        /// </summary>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Hourly();

        /// <summary>
        ///     Hourlies the specified minute.
        /// </summary>
        /// <param name="minute">The minute.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Hourly(int minute);

        /// <summary>
        ///     Hourlies the specified minutes.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Hourly(params int[] minutes);

        /// <summary>
        ///     Specifies incremental values. For example, a "10/20" in the minute field means at "10, 30, and 50 minutes of an
        /// hour".
        /// </summary>
        /// <typeparam name="T">Cron field value type</typeparam>
        /// <param name="cronType">Cron field Type.</param>
        /// <param name="start">The start value.</param>
        /// <param name="every">The increment value.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        /// <example>Increment(CronType.Minute, 10, 20);</example>
        ICronBuilder Increment<T>(CronType cronType, T start, T every);

        /// <summary>
        ///     Lasts the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Last<T>(CronType cronType);

        /// <summary>
        ///     Lasts the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="offset">The offset.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Last<T>(CronType cronType, T offset);

        /// <summary>
        ///     Lists the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="values">The values.</param>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder List<T>(CronType cronType, params T[] values);

        /// <summary>
        ///     Monthlies this instance.
        /// </summary>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Monthly();

        /// <summary>
        ///     Monthlies the specified day.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Monthly(int day);

        /// <summary>
        ///     Monthlies the specified minimum day.
        /// </summary>
        /// <param name="minDay">The minimum day.</param>
        /// <param name="maxDay">The maximum day.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Monthly(int minDay, int maxDay);

        /// <summary>
        ///     Monthlies the specified days.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Monthly(params int[] days);

        /// <summary>
        ///     Monthlies the specified cron type.
        /// </summary>
        /// <param name="nthValue">The NTH value.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Monthly(int nthValue, DayOfWeek dayOfWeek);

        /// <summary>
        ///     Overwrites the specified overwrite.
        /// </summary>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Overwrite(bool overwrite = true);

        /// <summary>
        ///     Overwrites the specified cron type.
        /// </summary>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Overwrite(CronType cronType, bool overwrite = true);

        /// <summary>
        ///     Pers the minute.
        /// </summary>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder PerMinute();

        /// <summary>
        ///     Ranges the specified cron type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Range<T>(CronType cronType, T minValue, T maxValue) where T : struct;

        /// <summary>
        ///     Determines the weekday nearest to a given day of the month.
        /// </summary>
        /// <param name="dayOfMonth">The day of the month.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Weekday(int dayOfMonth);

        /// <summary>
        ///     Weeklies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Weekly(CronDays value);

        /// <summary>
        ///     Weeklies the specified minimum value.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Weekly(CronDays minValue, CronDays maxValue);

        /// <summary>
        ///     Weeklies the specified days.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Weekly(params CronDays[] days);

        /// <summary>
        ///     Yearlies the specified month.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns><see cref="T:Cron.Core.ICronBuilder" /></returns>
        ICronBuilder Yearly(int month);

        /// <summary>
        ///     Yearlies the specified months.
        /// </summary>
        /// <param name="months">The months.</param>
        /// <returns>ICronBuilder.</returns>
        ICronBuilder Yearly(params int[] months);
    }
}