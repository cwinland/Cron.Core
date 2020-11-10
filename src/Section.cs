using Cron.Core.Enums;
using Cron.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cron.Core
{
    /// <inheritdoc cref="ISection" />
    public class Section : ISection
    {
        #region Fields

        private const string DEFAULT_CHAR = "*";
        private const string ANY_CHAR = "?";

        private readonly CronTimeSections time;
        private readonly List<ISectionValues> values = new List<ISectionValues>();

        #endregion Fields

        #region Constructors

        /// <summary>
        ///   Create an <see cref="ISection" /> based on the specified expression.
        /// </summary>
        /// <remarks>Experimental.</remarks>
        /// <param name="time">Specific <see cref="CronTimeSections" />.</param>
        /// <param name="expression">Cron Expression</param>
        protected internal Section(CronTimeSections time, string expression) : this(time)
        {
            Any = expression == ANY_CHAR;
            Every = expression.Length > 1 && (expression.StartsWith("/") || expression.StartsWith("*"));
            expression = expression.StartsWith("*") ? expression.Substring(1) : expression;
            expression = expression.StartsWith("/") ? expression.Substring(1) : expression;

            if (expression.Length > 0)
            {
                expression.Split(',')
                    .ToList()
                    .ForEach(
                        x =>
                        {
                            var vals = x.Split('-').ToList();
                            var val1 = int.Parse(vals[0]);
                            ISectionValues sectionValues = new SectionValues(
                                val1,
                                (vals.Count == 1)
                                    ? val1
                                    : int.Parse(vals[1])
                            );

                            values.Add(sectionValues);
                        }
                    );
            }
        }

        /// <summary>
        ///   Create <see cref="ISection" /> for a specific <see cref="CronTimeSections" />.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        protected internal Section(CronTimeSections time)
        {
            this.time = time;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc cref="ISection" />
        public int Count => values.Count;

        /// <inheritdoc cref="ISection" />
        public bool Enabled { get; set; } = true;

        /// <inheritdoc cref="ISection" />
        public bool Every { get; set; }

        /// <inheritdoc cref="ISection" />
        public bool Any { get; set; }

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
                else
                {
                    if (Every)
                    {
                        valueString.Add($"{DEFAULT_CHAR}/");
                    }

                    valueString.AddRange(values.Select(s => s.ToString()));

                    return valueString;
                }
            }
        }

        #endregion Properties

        #region Indexers

        /// <inheritdoc cref="ISection" />
        public ISectionValues this[int index] => ((IReadOnlyList<ISectionValues>)values)[index];

        #endregion Indexers

        #region Methods

        /// <inheritdoc cref="ISection" />
        public ISection Add([Range(0, 9999)] int value)
        {
            if (!IsValidRange(value))
            {
                throw new ArgumentOutOfRangeException();
            }
            values.Add(new SectionValues(value));
            return this;
        }

        /// <inheritdoc cref="ISection" />
        public ISection Add([Range(0, 9999)] int minVal, [Range(0, 9999)] int maxVal)
        {
            if (!IsValidRange(minVal) || !IsValidRange(maxVal))
            {
                throw new ArgumentOutOfRangeException();
            }
            values.Add(new SectionValues(minVal, maxVal));
            return this;
        }

        /// <inheritdoc cref="ISection" />
        public ISection Clear()
        {
            values.Clear();
            return this;
        }

        /// <inheritdoc cref="ISection" />
        public IEnumerator<ISectionValues> GetEnumerator()
        {
            return ((IEnumerable<ISectionValues>)values).GetEnumerator();
        }

        /// <inheritdoc cref="ISection" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)values).GetEnumerator();
        }

        /// <inheritdoc cref="ISection" />
        public ISection Remove([Range(0, 9999)] int value)
        {
            var val = values.Find(x => x.MinValue == value);
            values.Remove(val);
            return this;
        }

        /// <inheritdoc cref="ISection" />
        public ISection Remove([Range(0, 9999)] int minVal, [Range(0, 9999)] int maxVal)
        {
            var val = values.Find(x => x.MinValue == minVal && x.MaxValue == maxVal);
            values.Remove(val);
            return this;
        }

        /// <inheritdoc cref="ISection" />
        public override string ToString()
        {
            if (!Enabled)
            {
                return string.Empty;
            }

            if (values.Count == 0)
            {
                return DEFAULT_CHAR;
            }
            else
            {
                SortValues();

                var valueString = string.Join(",", values);
                if (Every)
                {
                    valueString = $"{DEFAULT_CHAR}/" + valueString;
                }

                return valueString;
            }
        }

        /// <inheritdoc cref="ISection" />
        public bool IsValidRange(ISectionValues values)
        {
            return IsValidRange(values.MinValue) && IsValidRange(values.MaxValue);
        }

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
                    result = (value > 0 && value < 20) || (value > 2000);
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        private void SortValues()
        {
            values.Sort(delegate (ISectionValues x, ISectionValues y)
            {
                var compare1 = x.MinValue.CompareTo(y.MinValue);
                return (compare1 != 0) ? compare1 : x.MaxValue.CompareTo(y.MaxValue);
            });
        }

        #endregion Methods
    }
}
