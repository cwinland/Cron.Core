// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 11-17-2020
// ***********************************************************************
// <copyright file="ISection.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using Cron.Core.Enums;

// ReSharper disable UnusedMemberInSuper.Global

namespace Cron.Core.Interfaces
{
    /// <summary>
    /// Section of time - represents the object for a specific given time element of the Cron expression.
    /// </summary>
    public interface ISection : IReadOnlyList<ISectionValues>
    {
        /// <summary>
        /// Add time value to this <see cref="ISection" />.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        ISection Add(int value);

        /// <summary>
        /// Add a time value range to this <see cref="ISection" />.
        /// </summary>
        /// <param name="minVal">Starting value for this <see cref="ISection" />.</param>
        /// <param name="maxVal">Ending value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        ISection Add(int minVal, int maxVal);

        /// <summary>
        /// Clear the values in the <see cref="ISection" />.
        /// </summary>
        /// <returns>ISection.</returns>
        ISection Clear();

        /// <summary>
        /// Indicates if this be represented as an integer.
        /// </summary>
        /// <returns><c>true</c> if this instance is int; otherwise, <c>false</c>.</returns>
        bool IsInt();

        /// <summary>
        /// Checks if the given value is valid for the current <see cref="ISection" />'s <see cref="CronTimeSections" /> value.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns><c>true</c> if [is valid range] [the specified value]; otherwise, <c>false</c>.</returns>
        bool IsValidRange(int value);

        /// <summary>
        /// Remove the specified value from the <see cref="ISection" />.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        ISection Remove(int value);

        /// <summary>
        /// Remove the specified range of values from the <see cref="ISection" />.
        /// </summary>
        /// <param name="minVal">Starting value for this <see cref="ISection" />.</param>
        /// <param name="maxVal">Ending value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        ISection Remove(int minVal, int maxVal);

        /// <summary>
        /// Convert to Integer.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int ToInt();

        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <param name="translateEnum">Indicates if there is an enumerable, that it should be represented as a string instead of integer.</param>
        /// <param name="enumType">Type of Enumerable.</param>
        /// <param name="showEvery">Show every indicator.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString(bool translateEnum, Type enumType, bool showEvery);

        /// <summary>
        /// Indicates that the value should be translated using the ? any indicator.
        /// </summary>
        /// <value><c>true</c> if any; otherwise, <c>false</c>.</value>
        /// <remarks>Experimental.</remarks>
        bool Any { get; set; }

        /// <summary>
        /// Indicates if any values contain a range.
        /// </summary>
        /// <value><c>true</c> if [contains range]; otherwise, <c>false</c>.</value>
        bool ContainsRange { get; }

        /// <summary>
        /// Get readable description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// Indicates that the value is enabled or used. Equivalent to using ? in a cron expression.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; set; }

        /// <summary>
        /// Indicates that the value should be translated using the */ every indicator.
        /// </summary>
        /// <value><c>true</c> if every; otherwise, <c>false</c>.</value>
        bool Every { get; set; }

        /// <summary>
        /// Gets the type of the section.
        /// </summary>
        /// <value>The type of the section.</value>
        CronSectionType SectionType { get; }

        /// <summary>
        /// Gets the <see cref="CronTimeSections" />.
        /// </summary>
        /// <value>The time.</value>
        CronTimeSections Time { get; }

        /// <summary>
        /// List of Cron value expression specific to the <see cref="ISection" />.
        /// </summary>
        /// <value>The values.</value>
        IEnumerable<string> Values { get; }
    }
}
