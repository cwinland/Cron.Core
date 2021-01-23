// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 11-17-2020
// ***********************************************************************
// <copyright file="CronDays.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
// ReSharper disable UnusedMember.Global
using EnhancedEnum;
using EnhancedEnum.Attributes;

namespace Cron.Core.Enums
{
    /// <summary>
    /// Day of the week values to build Cron expressions.
    /// </summary>
    public sealed class CronDays : EnhancedEnum<int, CronDays>
    {
        /// <summary>
        /// The sunday
        /// </summary>
        [Value(0)]
        public static readonly CronDays Sunday = new CronDays();

        /// <summary>
        /// The monday
        /// </summary>
        [Value(1)]
        public static readonly CronDays Monday = new CronDays();

        /// <summary>
        /// The tuesday
        /// </summary>
        [Value(2)]
        public static readonly CronDays Tuesday = new CronDays();

        /// <summary>
        /// The wednesday
        /// </summary>
        [Value(3)]
        public static readonly CronDays Wednesday = new CronDays();

        /// <summary>
        /// The thursday
        /// </summary>
        [Value(4)]
        public static readonly CronDays Thursday = new CronDays();

        /// <summary>
        /// The friday
        /// </summary>
        [Value(5)]
        public static readonly CronDays Friday = new CronDays();

        /// <summary>
        /// The saturday
        /// </summary>
        [Value(6)]
        public static readonly CronDays Saturday = new CronDays();

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="CronDays"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CronDays(string value) => Convert(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="CronDays"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CronDays(int value) => Convert(value);
    }
}
