// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="SectionValues.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using Cron.Core.Enums;
using Cron.Core.Interfaces;

namespace Cron.Core.Sections
{
    /// <summary>
    /// Class SectionValues.
    /// Implements the <see cref="Cron.Core.Interfaces.ISectionValues" />
    /// </summary>
    /// <seealso cref="Cron.Core.Interfaces.ISectionValues" />
    /// <inheritdoc cref="ISectionValues" />
    public class SectionValues : ISectionValues
    {
        private readonly int? maxValue;
        private readonly CronTimeSections time;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionValues" /> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="val">The value.</param>
        internal SectionValues(CronTimeSections time, int val)
        {
            this.time = time;
            MinValue = val;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionValues" /> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="minVal">The minimum value.</param>
        /// <param name="maxVal">The maximum value.</param>
        internal SectionValues(CronTimeSections time, int minVal, int maxVal) : this(time, minVal)
        {
            maxValue = maxVal;
        }

        /// <summary>
        /// Maximum value in a value range.
        /// </summary>
        /// <value>The maximum value.</value>
        /// <inheritdoc cref="ISectionValues" />
        public int MaxValue => maxValue ?? MinValue;

        /// <summary>
        /// Minimum value in a value range. Also represents the only value, if the section is not a range.
        /// </summary>
        /// <value>The minimum value.</value>
        /// <inheritdoc cref="ISectionValues" />
        public int MinValue { get; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="translate">Translate values.</param>
        /// <param name="enumType">Associated Enum Type.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <inheritdoc cref="ISectionValues" />
        public string ToString(bool translate, Type enumType)
        {
            var translateEnum = translate && enumType != null;
            var minVal = translateEnum
                ? Enum.ToObject(enumType, MinValue)
                    .ToString()
                : MinValue.ToString();

            var maxVal = translateEnum
                ? Enum.ToObject(enumType, MaxValue)
                    .ToString()
                : MaxValue.ToString();

            if (time == CronTimeSections.Hours && translate)
            {
                minVal = new DateTime().AddHours(int.Parse(minVal))
                    .ToString("hh:mm tt");
                maxVal = new DateTime().AddHours(int.Parse(maxVal))
                    .AddMinutes(59)
                    .AddSeconds(59)
                    .ToString("hh:mm tt");
            }

            return (minVal == maxVal
                ? minVal
                : $"{minVal}-{maxVal}").Trim();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(false, null);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="List{ISectionValues}" /> to <see cref="SectionValues" />.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        /// <inheritdoc cref="ISectionValues" />
        public static explicit operator SectionValues(List<ISectionValues> v)
        {
            throw new NotImplementedException();
        }
    }
}
