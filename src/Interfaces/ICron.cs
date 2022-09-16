// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 06-11-2021
// ***********************************************************************
// <copyright file="ICron.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************

using Cron.Core.Converters;
using Cron.Core.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Cron.Core.Interfaces
{
    /// <summary>
    ///     Cron Interface Object.
    /// </summary>
    [JsonInterfaceConverter(typeof(InterfaceConverter<CronBuilder, ICron>))]
    public interface ICron : IEnumerable<CronTimeSections>
    {
        /// <summary>
        ///     Add time value for the specified time section.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// <param name="repeatEvery">Indicates if the value is a repeating time or specific time.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronTimeSections time, int value, bool repeatEvery = false);

        /// <summary>
        ///     Add time value for the specified time section.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="minValue">Starting value for the specified time section.</param>
        /// <param name="maxValue">Ending value for the specified time section.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronTimeSections time, int minValue, int maxValue);

        /// <summary>
        ///     Add day of the week.
        /// </summary>
        /// <param name="value"><see cref="CronDays" /> representing the day of the week.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronDays value);

        /// <summary>
        ///     Add month.
        /// </summary>
        /// <param name="value"><see cref="CronMonths" /> representing the month.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronMonths value);

        /// <summary>
        ///     Add range of days in the week.
        /// </summary>
        /// <param name="minValue">Starting <see cref="CronDays" /> representing the day of the week.</param>
        /// <param name="maxValue">Ending <see cref="CronDays" /> representing the day of the week.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronDays minValue, CronDays maxValue);

        /// <summary>
        ///     Add range of months.
        /// </summary>
        /// <param name="minValue">Starting <see cref="CronMonths" /> representing the day of the month.</param>
        /// <param name="maxValue">Ending <see cref="CronMonths" /> representing the day of the month.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Add(CronMonths minValue, CronMonths maxValue);

        /// <summary>
        ///     Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// <returns>ICron.</returns>
        ICron Remove(CronTimeSections time, int value);

        /// <summary>
        ///     Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="minValue">Starting value for the specified time section.</param>
        /// <param name="maxValue">Ending value for the specified time section.</param>
        /// <returns>ICron.</returns>
        ICron Remove(CronTimeSections time, int minValue, int maxValue);

        /// <summary>
        ///     Clear the specific time element of the Cron object to defaults without any numerical cron representations.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Reset(CronTimeSections time);

        /// <summary>
        ///     Clear the entire Cron object to defaults without any numerical cron representations.
        /// </summary>
        /// <returns><see cref="ICron" /></returns>
        ICron Reset();

        /// <summary>
        ///     Set time with <see cref="ISection" /> value.
        /// </summary>
        /// <param name="value">Value for the specified time section.</param>
        /// <returns><see cref="ICron" /></returns>
        ICron Set(ISection value);

        /// <summary>
        ///     String value of the object.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString();

        /// <summary>
        ///     Gets or Sets to allow Seconds in the cron expression output.
        /// </summary>
        /// <value><c>true</c> if [allow seconds]; otherwise, <c>false</c>.</value>
        bool AllowSeconds { get; set; }

        /// <summary>
        ///     Day of the Month Information
        /// </summary>
        /// <value>The day month.</value>
        ISection DayMonth { get; }

        /// <summary>
        ///     Day of the Week Information
        /// </summary>
        /// <value>The day week.</value>
        ISection DayWeek { get; }

        /// <summary>
        ///     Human readable description.
        /// </summary>
        /// <value>The description.</value>
        /// <example>
        /// Every 22 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 12 of the month, only on
        /// Wednesday, only in March and May, every 5 years
        /// </example>
        string Description { get; }

        /// <summary>
        ///     Hours Information
        /// </summary>
        /// <value>The hours.</value>
        ISection Hours { get; }

        /// <summary>
        ///     Minutes Information
        /// </summary>
        /// <value>The minutes.</value>
        ISection Minutes { get; }

        /// <summary>
        ///     Months Information
        /// </summary>
        /// <value>The months.</value>
        ISection Months { get; }

        /// <summary>
        ///     Seconds Information
        /// </summary>
        /// <value>The seconds.</value>
        ISection Seconds { get; }

        /// <summary>
        ///     Gets the section list.
        /// </summary>
        /// <value>The section list.</value>
        List<Tuple<PropertyInfo, ISection>> SectionList { get; }

        /// <summary>
        ///     Cron Expression.
        /// </summary>
        /// <value>The value.</value>
        /// <example>*/22 */3,4 3-5,7-11 12 3,5 3 */5</example>
        string Value { get; set; }
    }
}
