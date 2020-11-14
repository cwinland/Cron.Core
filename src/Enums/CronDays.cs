// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="CronDays.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cron.Core.Enums
{
    /// <summary>
    /// Day of the week values to build Cron expressions.
    /// </summary>
    public enum CronDays
    {
        /// <summary>
        /// Sunday.
        /// </summary>
        Sunday = 0,

        /// <summary>
        /// Monday.
        /// </summary>
        Monday = 1,

        /// <summary>
        /// Tuesday.
        /// </summary>
        Tuesday = 2,

        /// <summary>
        /// Wednesday.
        /// </summary>
        Wednesday = 3,

        /// <summary>
        /// Thursday.
        /// </summary>
        Thursday = 4,

        /// <summary>
        /// Friday.
        /// </summary>
        Friday = 5,

        /// <summary>
        /// Saturday.
        /// </summary>
        Saturday = 6
    }
}
