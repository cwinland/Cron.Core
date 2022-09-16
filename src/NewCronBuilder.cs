using Cron.Core.Attributes;
using Cron.Core.Enums;
using Cron.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

// ReSharper disable UnusedMethodReturnValue.Local

namespace Cron.Core
{
    /// <summary>
    ///     Class NewCronBuilder.
    ///     Implements the <see cref="Cron.Core.ICronBuilder" />
    /// </summary>
    /// <seealso cref="Cron.Core.ICronBuilder" />
    public class NewCronBuilder : ICronBuilder
    {
        #region Fields

        private Dictionary<CronType, NumberParts> numbers = new();

        #endregion

        #region Properties

        public bool IsDirty => numbers.Any(x => x.Value.IsDirty);

        internal CronType? LastCronType { get; set; }

        #endregion

        public NewCronBuilder() => Clear();

        public ICronBuilder Daily(IReadOnlyList<int> hours, int minute) => Every(CronType.Day, CronType.Hour, hours);
        public ICronBuilder Daily(IReadOnlyList<int> hours, int minMinute, int maxMinute) => Every(CronType.Day, CronType.Hour, hours);

        public ICronBuilder Every<T>(CronType cronType, CronType subCronType, T minSubCronTypeValue, T maxSubCronTypeValue) where T : struct
        {
            CheckRange(CronType.Day, minSubCronTypeValue);
            CheckRange(CronType.Day, maxSubCronTypeValue);
            Every(cronType);
            return Every(cronType, subCronType, GetRange(minSubCronTypeValue, maxSubCronTypeValue));
        }

        /// <inheritdoc />
        public override string ToString() => Build().ToString();

        /// <summary>
        ///     Checks the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="values">The values.</param>
        /// <returns>ICronBuilder.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        internal static bool CheckRange<T>(CronType cronType, IReadOnlyList<T> values)
        {
            foreach (var value in values?.Where(value => !CheckRange(cronType, value)) ?? new List<T>())
            {
                throw new InvalidOperationException($"{value} is out of range.");
            }

            return true;
        }

        internal static bool CheckRange<T>(CronType cronType, T value)
        {
            var t = typeof(T).IsEnum ? (int) (object) value : (IComparable) value;
            var memberData = typeof(CronType).GetMember(cronType.ToString()).FirstOrDefault();
            if (memberData == null)
            {
                return false;
            }

            var attrs = memberData.GetCustomAttributes(true).Cast<Attribute>().Where(attr => attr is RangeAttribute).ToArray();
            foreach (var attr in attrs)
            {
                if (attr is RangeAttribute rAttr && rAttr.IsValidRange(t))
                {
                    return true;
                }
            }

            return false;
        }

        internal string GetRange<T>(T val1, T val2) where T : struct
            => (int)(object)val1 <= (int)(object)val2 ? $"{val1}-{val2}" : $"{val2}-{val1}";

        internal NumberParts GetValues(CronType type)
        {
            if (!(numbers?.ContainsKey(type) ?? false))
            {
                numbers?.Add(type, new NumberParts());
            }

            return numbers?[type] ?? new NumberParts();
        }

        internal NewCronBuilder Set(CronType cronType, string value)
        {
            ClearValues(cronType);
            AddValue(cronType, value);
            return this;
        }

        private ICronBuilder AddValue(CronType cronType, object value)
        {
            LastCronType = cronType;
            var list = GetValues(cronType);
            if (list.IsDirty)
            {
                list.Add(value?.ToString());
            }
            else
            {
                list.Replace(value?.ToString());
            }
            //if (!IsDirty && cronType != CronType.Minute)
            //{
            //    Set(CronType.Minute, "0");
            //}

            return this;
        }

        private ICronBuilder ClearValues(CronType type)
        {
            GetValues(type)?.Clear();
            return this;
        }

        private string GetValue(CronType type)
        {
            var values = GetValues(type) ?? new NumberParts();
            return values.Count switch
            {
                0 => type == CronType.Minute ? "*" : "0",
                //1 => values[0] == null ? type == CronType.Minute ? "*" : "0" : values[0].ToString(),
                //2 => ParseValues(type, values[0], values[1]),
                //_ => ParseValues(type, values)
                _ => string.Join(",", values.Select(x => x.Number).ToList())
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

        #region ICronBuilder

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentOutOfRangeException">value</exception>
        public ICronBuilder Add<T>(CronType cronType, T value)
        {
            if (!CheckRange(cronType, value))
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
            GetValue(CronType.Weekday));

        /// <inheritdoc />
        public ICronBuilder Clear()
        {
            numbers ??= new Dictionary<CronType, NumberParts>();
            numbers.Clear();
            CronType.Day.ToNameList()?.ForEach(x => numbers.Add(x, new NumberParts("0")));
            Set(CronType.Minute, "*");
            SetClean();
            return this;
        }

        /// <inheritdoc />
        public ICronBuilder Clone() => new NewCronBuilder { numbers = numbers?.ToImmutableDictionary().ToDictionary(x => x.Key, y => y.Value) };

        /// <inheritdoc />
        public ICronBuilder Daily() => Every(CronType.Day);

        /// <inheritdoc />
        public ICronBuilder Daily(int hour) => Every(CronType.Day, CronType.Hour, hour);

        public ICronBuilder Daily(int hour, int minute) => Every(CronType.Day, CronType.Hour, hour)?.Every(CronType.Day, CronType.Minute, minute);

        /// <inheritdoc />
        public ICronBuilder Daily(params int[] hours) => Every(CronType.Day, CronType.Hour, hours);

        public ICronBuilder Daily(int minHour, int maxHour, int minute) => this;

        public ICronBuilder Daily(int minHour, int maxHour, int minMinute, int maxMinute) => this;

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
            Every(cronType);
            return Set(subCronType, subCronTypeValue);
        }

        /// <inheritdoc />
        public ICronBuilder Every<T>(CronType cronType, CronType subCronType, params T[] subCronTypeValues)
        {
            CheckRange(subCronType, subCronTypeValues);
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
            CheckRange(cronType, start);
            CheckRange(cronType, every);
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

            CheckRange(cronType, offset);
            return Set(cronType, $"L-{offset}");
        }

        /// <inheritdoc />
        public ICronBuilder List<T>(CronType cronType, params T[] values)
        {
            if (GetValues(cronType).Overwrite)
            {
                ClearValues(cronType);
            }

            return AddValue(cronType, string.Join(",", values));
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

        public ICronBuilder Overwrite(bool overwrite = true) => CronBuilderExtensions.Overwrite(this, overwrite);

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
            if (!CheckRange(cronType, minValue))
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            if (!CheckRange(cronType, maxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }

            AddValue(cronType, GetRange(minValue, maxValue));
            return this;
        }

        /// <inheritdoc />
        public ICronBuilder Weekday(int dayOfMonth)
        {
            CheckRange(CronType.Day, dayOfMonth);
            return Set(CronType.Day, $"{dayOfMonth}W");
        }

        /// <inheritdoc />
        public ICronBuilder Weekly(CronDays value) => Every(CronType.Month, CronType.Day, value);

        /// <inheritdoc />
        public ICronBuilder Weekly(CronDays minValue, CronDays maxValue)
        {
            CheckRange(CronType.Day, minValue);
            CheckRange(CronType.Day, maxValue);
            return Every(CronType.Month, CronType.Day, GetRange(minValue, maxValue));
        }

        /// <inheritdoc />
        public ICronBuilder Weekly(params CronDays[] days)
        {
            foreach (var cronDays in days)
            {
                CheckRange(CronType.Day, cronDays);
            }

            return Every(CronType.Month, CronType.Day, days);
        }

        /// <inheritdoc />
        public ICronBuilder Yearly(int month)
        {
            CheckRange(CronType.Month, month);
            return Set(CronType.Month, month);
        }

        /// <inheritdoc />
        public ICronBuilder Yearly(params int[] months) => Set(CronType.Month, months?.Select(x => x.ToString()).ToList());

        #endregion
    }
}