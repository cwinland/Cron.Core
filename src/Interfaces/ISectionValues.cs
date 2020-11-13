// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 11-12-2020
// ***********************************************************************
// <copyright file="ISectionValues.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Cron.Core.Interfaces
{
    /// <summary>
    /// Stores the values for a section list.
    /// </summary>
    public interface ISectionValues
    {
        /// <summary>
        /// Maximum value in a value range.
        /// </summary>
        /// <value>The maximum value.</value>
        int MaxValue { get; }

        /// <summary>
        /// Minimum value in a value range. Also represents the only value, if the section is not a range.
        /// </summary>
        /// <value>The minimum value.</value>
        int MinValue { get; }

        /// <summary>
        /// Convert to a string and translate to Enum, if applicable.
        /// </summary>
        /// <param name="translate">Translate values.</param>
        /// <param name="enumType">Associated Enum Type.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString(bool translate, Type enumType);
    }
}
