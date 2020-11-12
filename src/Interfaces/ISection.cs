using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cron.Core.Enums;

namespace Cron.Core.Interfaces
{
    /// <summary>
    ///   Section of time - represents the object for a specific given time element of the Cron expression.
    /// </summary>
    public interface ISection : IReadOnlyList<ISectionValues>
    {
        /// <summary>
        ///   List of Cron value expression specific to the <see cref="ISection" />.
        /// </summary>
        IEnumerable<string> Values { get; }

        /// <summary>
        ///   Indicates that the value should be translated using the */ every indicator.
        /// </summary>
        bool Every { get; set; }

        /// <summary>
        ///   Indicates if any values contain a range.
        /// </summary>
        bool ContainsRange { get; }

        /// <summary>
        ///   Indicates that the value should be translated using the ? any indicator.
        /// </summary>
        /// <remarks>Experimental.</remarks>
        bool Any { get; set; }

        /// <summary>
        ///   Indicates that the value is enabled or used. Equivalent to using ? in a cron expression.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///   Get readable description.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///   Checks if the given values are valid for the current <see cref="ISection" />'s <see cref="CronTimeSections" /> value.
        /// </summary>
        /// <param name="values"><see cref="ISectionValues" /> values.</param>
        /// <returns></returns>
        bool IsValidRange(ISectionValues values);

        /// <summary>
        ///   Checks if the given value is valid for the current <see cref="ISection" />'s <see cref="CronTimeSections" /> value.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns></returns>
        bool IsValidRange([Range(0, 9999)] int value);

        /// <summary>
        ///   Add time value to this <see cref="ISection" />.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns></returns>
        ISection Add([Range(0, 9999)] int value);

        /// <summary>
        ///   Add a time value range to this <see cref="ISection" />.
        /// </summary>
        /// <param name="minVal">Starting value for this <see cref="ISection" />.</param>
        /// <param name="maxVal">Ending value for this <see cref="ISection" />.</param>
        /// <returns></returns>
        ISection Add([Range(0, 9999)] int minVal, [Range(0, 9999)] int maxVal);

        /// <summary>
        ///   Clear the values in the <see cref="ISection" />.
        /// </summary>
        /// <returns></returns>
        ISection Clear();

        /// <summary>
        ///   Remove the specified value from the <see cref="ISection" />.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns></returns>
        ISection Remove(int value);

        /// <summary>
        ///   Remove the specified range of values from the <see cref="ISection" />.
        /// </summary>
        /// <param name="minVal">Starting value for this <see cref="ISection" />.</param>
        /// <param name="maxVal">Ending value for this <see cref="ISection" />.</param>
        /// <returns></returns>
        ISection Remove(int minVal, int maxVal);

        /// <summary>
        ///   Convert to string.
        /// </summary>
        /// <param name="translateEnum">Indicates if there is an enumerable, that it should be represented as a string instead of integer.</param>
        /// <param name="enumType">Type of Enumerable.</param>
        /// <param name="showEvery">Show every indicator.</param>
        /// <returns></returns>
        string ToString(bool translateEnum, Type enumType, bool showEvery);

        /// <summary>
        ///   Indicates if this be represented as an integer.
        /// </summary>
        /// <returns></returns>
        bool IsInt();

        /// <summary>
        ///   Convert to Integer.
        /// </summary>
        int ToInt();

        /// <summary>
        ///   Next value for this section.
        /// </summary>
        /// <returns></returns>
        TimeSpan Next(DateTime dateTime);
    }
}
