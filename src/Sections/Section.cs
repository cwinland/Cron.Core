// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-12-2020
// ***********************************************************************
// <copyright file="Section.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cron.Core.Enums;
using Cron.Core.Interfaces;

namespace Cron.Core.Sections
{
    /// <summary>
    /// Class Section.
    /// Implements the <see cref="Cron.Core.Interfaces.ISection" />
    /// </summary>
    /// <seealso cref="Cron.Core.Interfaces.ISection" />
    /// <inheritdoc cref="ISection" />
    public abstract class Section : ISection
    {
        #region Indexers

        /// <summary>
        /// Gets the <see cref="ISectionValues"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ISectionValues.</returns>
        /// <inheritdoc cref="ISection" />
        public ISectionValues this[int index] => ((IReadOnlyList<ISectionValues>) values)[index];

        #endregion Indexers

        #region Fields

        private const string ANY_CHAR = "?";
        private const string DEFAULT_CHAR = "*";

        /// <summary>
        /// Time Section Type.
        /// </summary>
        protected readonly CronTimeSections time;

        private readonly List<ISectionValues> values = new List<ISectionValues>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create an <see cref="ISection" /> based on the specified expression.
        /// </summary>
        /// <param name="time">Specific <see cref="CronTimeSections" />.</param>
        /// <param name="expression">Cron Expression</param>
        /// <remarks>Experimental.</remarks>
        protected Section(CronTimeSections time, string expression) : this(time)
        {
            Any = expression == ANY_CHAR;

            // Should only allow every on times
            Every = expression.Length > 1 && (expression.StartsWith("/") || expression.StartsWith("*"));
            expression = expression.StartsWith("*") ? expression.Substring(1) : expression;
            expression = expression.StartsWith("/") ? expression.Substring(1) : expression;

            if (expression.Length <= 0)
            {
                return;
            }

            Enabled = true;
            expression.Split(',')
                      .ToList()
                      .ForEach(
                          x =>
                          {
                              var vals = x.Split('-')
                                          .ToList();
                              var val1 = int.Parse(vals[0]);
                              ISectionValues sectionValues = new SectionValues(
                                  time,
                                  val1,
                                  vals.Count == 1
                                      ? val1
                                      : int.Parse(vals[1])
                              );

                              values.Add(sectionValues);
                          }
                      );
        }

        /// <summary>
        /// Create <see cref="ISection" /> for a specific <see cref="CronTimeSections" />.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        protected internal Section(CronTimeSections time)
        {
            this.time = time;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates that the value should be translated using the ? any indicator.
        /// </summary>
        /// <value><c>true</c> if any; otherwise, <c>false</c>.</value>
        /// <inheritdoc cref="ISection" />
        /// <remarks>Experimental.</remarks>
        public bool Any { get; set; }

        /// <summary>
        /// Indicates if any values contain a range.
        /// </summary>
        /// <value><c>true</c> if [contains range]; otherwise, <c>false</c>.</value>
        /// <inheritdoc cref="ISectionValues" />
        public bool ContainsRange => values.Any(x => x.MinValue != x.MaxValue);

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>The count.</value>
        /// <inheritdoc cref="ISection" />
        public int Count => values.Count;

        /// <inheritdoc />
        public string Description
        {
            get
            {
                var result = string.Empty;

                if (!Enabled ||
                    values.Count == 0)
                {
                    return time == CronTimeSections.Seconds && Every ? "Every second" : string.Empty;
                }

                switch (time)
                {
                    case CronTimeSections.Seconds:
                        result += Every
                            ? $"Every {ToString(false, null, false)} seconds"
                            : $"Only at {ToString(false, null, false)} seconds past the minute";

                        break;

                    case CronTimeSections.Minutes:
                        result += Every
                            ? $"every {ToString(false, null, false)} minutes"
                            : $"only at {ToString(false, null, false)} minutes past the hour";

                        break;

                    case CronTimeSections.Hours:
                        result += ContainsRange
                            ? $"{ToString(true, null, true)}"
                            : Every
                                ? $"every {ToString(false, null, false)} hours"
                                : $"only at {ToString(false, null, false)} hours";

                        break;

                    case CronTimeSections.DayMonth:
                        result += Every
                            ? $"every {ToString(false, null, false)} days"
                            : $"only on day {ToString(false, null, false)} of the month";

                        break;

                    case CronTimeSections.DayWeek:
                        result += Every
                            ? $"every {ToString(false, null, false)} day of the week"
                            : $"only on {ToString(true, typeof(CronDays), false)}";

                        break;

                    case CronTimeSections.Months:
                        var everyMonthValue = ToString(false, null, false);
                        result += Every
                            ? everyMonthValue == "1"
                                ? string.Empty
                                : $"every {everyMonthValue} months"
                            : $"only in {ToString(true, typeof(CronMonths), false)}";

                        break;

                    case CronTimeSections.Years:
                        var everyYearValue = ToString(false, null, false);
                        result += Every
                            ? everyYearValue == "1"
                                ? string.Empty
                                : $"every {everyYearValue} years"
                            : $"only in year {ToString(false, null, false)}";

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return result.Trim();
            }
        }

        /// <summary>
        /// Indicates that the value is enabled or used. Equivalent to using ? in a cron expression.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <inheritdoc cref="ISection" />
        public bool Enabled { get; set; }

        /// <summary>
        /// Indicates that the value should be translated using the */ every indicator.
        /// </summary>
        /// <value><c>true</c> if every; otherwise, <c>false</c>.</value>
        /// <inheritdoc cref="ISection" />
        public virtual bool Every { get; set; } = true;

        /// <summary>
        /// List of Cron value expression specific to the <see cref="ISection" />.
        /// </summary>
        /// <value>The values.</value>
        /// <inheritdoc cref="ISection" />
        public IEnumerable<string> Values
        {
            get
            {
                var valueString = new List<string>();

                if (values.Count == 0)
                {
                    valueString.Add(DEFAULT_CHAR);

                    return valueString;
                }

                if (Every)
                {
                    valueString.Add($"{DEFAULT_CHAR}/");
                }

                valueString.AddRange(values.Select(s => s.ToString()));

                return valueString;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add time value to this <see cref="ISection" />.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <inheritdoc cref="ISection" />
        public ISection Add([Range(0, 9999)] int value)
        {
            if (!IsValidRange(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            values.Add(new SectionValues(time, value));

            if (values.Count == 1)
            {
                Every = false;
            }

            Enabled = true;

            return this;
        }

        /// <summary>
        /// Add a time value range to this <see cref="ISection" />.
        /// </summary>
        /// <param name="minVal">Starting value for this <see cref="ISection" />.</param>
        /// <param name="maxVal">Ending value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <inheritdoc cref="ISection" />
        public ISection Add([Range(0, 9999)] int minVal, [Range(0, 9999)] int maxVal)
        {
            if (!IsValidRange(minVal) ||
                !IsValidRange(maxVal))
            {
                throw new ArgumentOutOfRangeException();
            }

            values.Add(new SectionValues(time, minVal, maxVal));
            Enabled = true;

            return this;
        }

        /// <summary>
        /// Clear the values in the <see cref="ISection" />.
        /// </summary>
        /// <returns>ISection.</returns>
        /// <inheritdoc cref="ISection" />
        public ISection Clear()
        {
            values.Clear();
            Enabled = false;

            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        /// <inheritdoc cref="ISection" />
        public IEnumerator<ISectionValues> GetEnumerator()
        {
            return ((IEnumerable<ISectionValues>) values).GetEnumerator();
        }

        /// <inheritdoc cref="ISection" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) values).GetEnumerator();
        }

        /// <summary>
        /// Indicates if this be represented as an integer.
        /// </summary>
        /// <returns><c>true</c> if this instance is int; otherwise, <c>false</c>.</returns>
        /// <inheritdoc cref="ISectionValues" />
        public bool IsInt()
        {
            return !ContainsRange && int.TryParse(ToString(false, null, true), out _);
        }

        /// <summary>
        /// Determines whether [is valid range] [the specified check values].
        /// </summary>
        /// <param name="checkValues">The check values.</param>
        /// <returns><c>true</c> if [is valid range] [the specified check values]; otherwise, <c>false</c>.</returns>
        /// <inheritdoc cref="ISection" />
        public bool IsValidRange(ISectionValues checkValues)
        {
            return IsValidRange(checkValues.MinValue) && IsValidRange(checkValues.MaxValue);
        }

        /// <summary>
        /// Checks if the given value is valid for the current <see cref="ISection" />'s <see cref="CronTimeSections" /> value.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns><c>true</c> if [is valid range] [the specified value]; otherwise, <c>false</c>.</returns>
        /// <inheritdoc cref="ISection" />
        public bool IsValidRange([Range(0, 9999)] int value)
        {
            bool result;

            switch (time)
            {
                case CronTimeSections.Seconds:
                case CronTimeSections.Minutes:
                    result = value >= 0 && value <= 59;

                    break;

                case CronTimeSections.Hours:
                    result = value >= 0 && value <= 23;

                    break;

                case CronTimeSections.DayWeek:
                    result = Enum.IsDefined(typeof(CronDays), value);

                    break;

                case CronTimeSections.DayMonth:
                    result = value >= 1 && value <= 31;

                    break;

                case CronTimeSections.Months:
                    result = Enum.IsDefined(typeof(CronMonths), value);

                    break;

                case CronTimeSections.Years:
                    result = value > 0 && value < 20 || value > 2000;

                    break;

                default:
                    result = false;

                    break;
            }

            return result;
        }

        /// <summary>
        /// Remove the specified value from the <see cref="ISection" />.
        /// </summary>
        /// <param name="value">Value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        /// <inheritdoc cref="ISection" />
        public ISection Remove([Range(0, 9999)] int value)
        {
            var val = values.Find(x => x.MinValue == value);
            values.Remove(val);

            return this;
        }

        /// <summary>
        /// Remove the specified range of values from the <see cref="ISection" />.
        /// </summary>
        /// <param name="minVal">Starting value for this <see cref="ISection" />.</param>
        /// <param name="maxVal">Ending value for this <see cref="ISection" />.</param>
        /// <returns>ISection.</returns>
        /// <inheritdoc cref="ISection" />
        public ISection Remove([Range(0, 9999)] int minVal, [Range(0, 9999)] int maxVal)
        {
            var val = values.Find(x => x.MinValue == minVal && x.MaxValue == maxVal);
            values.Remove(val);

            return this;
        }

        /// <summary>
        /// Convert to Integer.
        /// </summary>
        /// <returns>System.Int32.</returns>
        /// <inheritdoc cref="ISectionValues" />
        public int ToInt()
        {
            if (!Enabled)
            {
                return 0;
            }

            SortValues();

            var myValues = values.Select(x => x.ToString(false, null));
            var valueString = string.Join(",", myValues)
                                    .Trim();

            return int.TryParse(valueString, out var num) ? num : 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="translateEnum">Indicates if there is an enumerable, that it should be represented as a string instead of integer.</param>
        /// <param name="enumType">Type of Enumerable.</param>
        /// <param name="showEvery">Show every indicator.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <inheritdoc cref="ISection" />
        public string ToString(bool translateEnum, Type enumType, bool showEvery)
        {
            if (!Enabled &&
                !showEvery)
            {
                return string.Empty;
            }

            if (values.Count == 0)
            {
                return DEFAULT_CHAR;
            }

            SortValues();

            var myValues = values.Select(x => x.ToString(translateEnum, enumType));
            var valueString = string.Join(",", myValues);

            if (Every && showEvery)
            {
                valueString = $"{DEFAULT_CHAR}/" + valueString;
            }

            return valueString.Trim();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <inheritdoc cref="ISection" />
        public override string ToString() => ToString(false, null, true);

        private void SortValues()
        {
            values.Sort(
                delegate(ISectionValues x, ISectionValues y)
                {
                    var compare1 = x.MinValue.CompareTo(y.MinValue);

                    return compare1 != 0 ? compare1 : x.MaxValue.CompareTo(y.MaxValue);
                }
            );
        }

        #endregion Methods
    }
}
