using Cron.Core.Enums;
using Cron.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

// ReSharper disable UnusedMethodReturnValue.Local

namespace Cron.Core
{
    /// <summary>
    ///     Class NewCronBuilder.
    ///     Implements the <see cref="Cron.Core.ICronBuilder" />
    /// </summary>
    /// <seealso cref="Cron.Core.ICronBuilder" />
    /// <inheritdoc />
    public class NewCronBuilder : ICronBuilder
    {
        #region Fields

        private readonly Dictionary<CronType, NumberParts> numbers = new();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether the builder has changed.
        /// </summary>
        /// <value><c>true</c> if the builder has changed; otherwise, <c>false</c>.</value>
        public bool IsDirty => numbers.Any(x => x.Value.IsDirty);

        /// <summary>
        ///     Gets or sets the last type of the cron.
        /// </summary>
        /// <value>The last type of the cron.</value>
        internal CronType? LastCronType { get; set; }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="NewCronBuilder" /> class.
        /// </summary>
        public NewCronBuilder() => Clear();
        internal NewCronBuilder(Dictionary<CronType, NumberParts> numbers, CronType? lastCronType)
        {
            this.numbers = numbers;
            LastCronType = lastCronType;
        }


        /// <summary>
        ///     Single point to add value to the number list.
        /// </summary>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="value">Single value or list of values.</param>
        /// <param name="maxValue">Optional maximum value. This value indicates a range.</param>
        /// <returns><see cref="ICronBuilder" />.</returns>
        internal ICronBuilder AddValue(CronType cronType, object value, object maxValue = null)
        {
            LastCronType = cronType;
            var list = GetValues(cronType);
            if (list.IsDirty)
            {
                if (maxValue == null)
                {
                    list.Add(value);
                }
                else
                {
                    list.Add(value, maxValue);
                }
            }
            else
            {
                if (maxValue == null)
                {
                    list.Replace(value);
                }
                else
                {
                    list.Replace(value, maxValue);
                }
            }

            SetUnset(cronType);
            return this;
        }

        /// <summary>
        ///     Gets the values.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>NumberParts.</returns>
        internal NumberParts GetValues(CronType type) => numbers?[type] ?? throw new ArgumentNullException(nameof(type));

        /// <summary>
        ///     Sets the specified cron type.
        /// </summary>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="value">The value.</param>
        /// <returns>NewCronBuilder.</returns>
        internal NewCronBuilder Set(CronType cronType, string value)
        {
            ClearValues(cronType);
            AddValue(cronType, value);
            return this;
        }

        private ICronBuilder ClearValues(CronType type)
        {
            GetValues(type)?.Clear();
            return this;
        }

        private string GetValue(CronType type)
        {
            var values = GetValues(type) ?? throw new KeyNotFoundException(nameof(type));
            return values.Count switch
            {
                0 => type == CronType.Minute ? "*" : "0",
                _ => string.Join(",", values.GetNumbers())
            };
        }

        private NewCronBuilder Set<T>(CronType cronType, T value)
        {
            ClearValues(cronType);
            AddValue(cronType, value);
            return this;
        }

        private void SetClean()
        {
            foreach (var value in numbers.Values)
            {
                value?.ForEach(x => x.IsDirty = false);
            }
        }

        private ICronBuilder SetUnset(CronType cronType)
        {
            for (var i = CronType.Minute; i < cronType; i++)
            {
                var values = GetValues(i);
                if (!values.IsDirty)
                {
                    values.Replace("0");
                    values.Numbers.First().IsDirty = false;
                }
            }

            for (var i = cronType + 1; i <= CronType.Weekday; i++)
            {
                var values = GetValues(i);
                if (!values.IsDirty)
                {
                    values.Replace("?");
                }
            }

            return this;
        }

        /// <inheritdoc />
        public override string ToString() => Build()?.ToString();

        #region ICronBuilder

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentOutOfRangeException">value</exception>
        public ICronBuilder Add<T>(CronType cronType, T value)
        {
            if (!cronType.CheckRange(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return AddValue(cronType, value);
        }

        /// <inheritdoc />
        public ICronBuilder All<T>(CronType cronType) => Set(cronType, "*");

        /// <inheritdoc />
        public ICronBuilder Any<T>(CronType cronType) => Set(cronType, "?");

        /// <inheritdoc />
        public ICronExpression Build() => new CronExpression(GetValue(CronType.Minute),
            GetValue(CronType.Hour),
            GetValue(CronType.Day),
            GetValue(CronType.Month),
            GetValue(CronType.Weekday)) { Builder = this };

        /// <inheritdoc />
        public ICronBuilder Clear()
        {
            numbers.Clear();
            if (numbers.Count == 0)
            {
                CronType.Day.ToNameList()?.ForEach(x => numbers.Add(x, new NumberParts("0", x)));
            }

            Set(CronType.Minute, "*");
            SetClean();
            return this;
        }

        /// <inheritdoc />
        public ICronBuilder Clone() => new NewCronBuilder(numbers?.ToImmutableDictionary().ToDictionary(x => x.Key, y => y.Value), LastCronType);

        /// <inheritdoc />
        public ICronBuilder Daily() => Every(CronType.Day);

        /// <inheritdoc />
        public ICronBuilder Daily(int hour) => Every(CronType.Day, CronType.Hour, hour);

        /// <inheritdoc />
        public ICronBuilder Daily(int hour, int minute) => Every(CronType.Day, CronType.Hour, hour)?.Every(CronType.Day, CronType.Minute, minute);

        /// <inheritdoc />
        public ICronBuilder Daily(params int[] hours) => Every(CronType.Day, CronType.Hour, hours);

        /// <inheritdoc />
        public ICronBuilder Daily(int minHour, int maxHour, int minute) => this;

        /// <inheritdoc />
        public ICronBuilder Daily(int minHour, int maxHour, int minMinute, int maxMinute) => this;

        /// <inheritdoc />
        public ICronBuilder Every<T>(CronType cronType, CronType subCronType, T minSubCronTypeValue, T maxSubCronTypeValue) where T : struct
        {
            CronType.Day.CheckRange(minSubCronTypeValue);
            CronType.Day.CheckRange(maxSubCronTypeValue);
            Every(cronType);
            return Every(cronType, subCronType, minSubCronTypeValue.GetRange(maxSubCronTypeValue));
        }

        /// <inheritdoc />
        public ICronBuilder Every()
        {
            CronType.Day.ToNameList()?.ForEach(x => Every(x));
            return this;
        }

        /// <inheritdoc />
        public ICronBuilder Every(CronType cronType) => Set(cronType, "*");

        /// <inheritdoc />
        public ICronBuilder Every<T>(CronType cronType, CronType subCronType, T subCronTypeValue)
        {
            subCronType.CheckRange(subCronTypeValue);
            Every(cronType);
            return Set(subCronType, subCronTypeValue);
        }

        /// <inheritdoc />
        public ICronBuilder Every<T>(CronType cronType, CronType subCronType, params T[] subCronTypeValues)
        {
            subCronType.CheckRange(subCronTypeValues);
            Every(cronType);
            ClearValues(subCronType);
            List(cronType, subCronTypeValues);
            return this;
        }

        /// <inheritdoc />
        public ICronBuilder Hourly() => Every(CronType.Hour);

        /// <inheritdoc />
        public ICronBuilder Hourly(int minute) => Every(CronType.Hour, CronType.Minute, minute);

        /// <inheritdoc />
        public ICronBuilder Hourly(params int[] minutes) => Every(CronType.Hour, CronType.Minute, minutes);

        /// <inheritdoc />
        public ICronBuilder Increment<T>(CronType cronType, T start, T every)
        {
            cronType.CheckRange(start);
            cronType.CheckRange(every);
            return Set(cronType, $"{start}/{every}");
        }

        /// <inheritdoc />
        public ICronBuilder Last<T>(CronType cronType)
        {
            if (cronType is not (CronType.Day or CronType.Weekday))
            {
                throw new ArgumentException($"{nameof(cronType)} must be '{CronType.Day}' or '{CronType.Weekday}'.");
            }

            return Set(cronType, "L");
        }

        /// <inheritdoc />
        public ICronBuilder Last<T>(CronType cronType, T offset)
        {
            if (cronType is not (CronType.Day or CronType.Weekday))
            {
                throw new ArgumentException($"{nameof(cronType)} must be '{CronType.Day}' or '{CronType.Weekday}'.");
            }

            cronType.CheckRange(offset);
            return Set(cronType, $"L-{offset}");
        }

        /// <inheritdoc />
        public ICronBuilder List<T>(CronType cronType, params T[] values)
        {
            if (GetValues(cronType).Overwrite)
            {
                ClearValues(cronType);
            }

            return AddValue(cronType, values);
        }

        /// <inheritdoc />
        public ICronBuilder Monthly() => Every(CronType.Month);

        /// <inheritdoc />
        public ICronBuilder Monthly(int day) => Every(CronType.Month, CronType.Day, day);

        /// <inheritdoc />
        public ICronBuilder Monthly(int minDay, int maxDay) => Every(CronType.Month)?.Range(CronType.Day, minDay, maxDay);

        /// <inheritdoc />
        public ICronBuilder Monthly(params int[] days) => Every(CronType.Month, CronType.Day, days);

        /// <inheritdoc />
        public ICronBuilder Monthly(int nthValue, DayOfWeek dayOfWeek) => Every(CronType.Month, CronType.Weekday, $"{dayOfWeek}#{nthValue}");

        /// <inheritdoc />
        public ICronBuilder Overwrite(bool overwrite = true) => CronBuilderExtensions.Overwrite(this, overwrite);

        /// <inheritdoc />
        public ICronBuilder Overwrite(CronType cronType, bool overwrite = true)
        {
            LastCronType = cronType;
            return Overwrite(overwrite);
        }

        /// <inheritdoc />
        public ICronBuilder PerMinute() => Every(CronType.Minute);

        /// <inheritdoc />
        public ICronBuilder Range<T>(CronType cronType, T minValue, T maxValue) where T : struct
        {
            if (!cronType.CheckRange(minValue))
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            if (!cronType.CheckRange(maxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }

            AddValue(cronType, minValue, maxValue);
            return this;
        }

        /// <inheritdoc />
        public ICronBuilder Weekday(int dayOfMonth)
        {
            CronType.Day.CheckRange(dayOfMonth);
            return Set(CronType.Day, $"{dayOfMonth}W");
        }

        /// <inheritdoc />
        public ICronBuilder Weekly(CronDays value) => Every(CronType.Month, CronType.Day, value);

        /// <inheritdoc />
        public ICronBuilder Weekly(CronDays minValue, CronDays maxValue)
        {
            CronType.Day.CheckRange(minValue);
            CronType.Day.CheckRange(maxValue);
            return Every(CronType.Month, CronType.Day, minValue.GetRange(maxValue));
        }

        /// <inheritdoc />
        public ICronBuilder Weekly(params CronDays[] days)
        {
            foreach (var cronDays in days)
            {
                CronType.Day.CheckRange(cronDays);
            }

            return Every(CronType.Month, CronType.Day, days);
        }

        /// <inheritdoc />
        public ICronBuilder Yearly(int month)
        {
            CronType.Month.CheckRange(month);
            return Set(CronType.Month, month);
        }

        /// <inheritdoc />
        public ICronBuilder Yearly(params int[] months) => Set(CronType.Month, months?.Select(x => x.ToString()).ToList());

        #endregion
    }
}