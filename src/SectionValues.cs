using System;
using System.Collections.Generic;
using Cron.Core.Enums;
using Cron.Core.Interfaces;

namespace Cron.Core
{
    /// <inheritdoc cref="ISectionValues" />
    public class SectionValues : ISectionValues
    {
        private readonly int? maxValue;
        private readonly CronTimeSections time;

        internal SectionValues(CronTimeSections time, int val)
        {
            this.time = time;
            MinValue = val;
        }

        internal SectionValues(CronTimeSections time, int minVal, int maxVal) : this(time, minVal)
        {
            maxValue = maxVal;
        }

        /// <inheritdoc cref="ISectionValues" />
        public int MaxValue => maxValue ?? MinValue;

        /// <inheritdoc cref="ISectionValues" />
        public int MinValue { get; }

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

        /// <inheritdoc cref="ISectionValues" />
        public static explicit operator SectionValues(List<ISectionValues> v)
        {
            throw new NotImplementedException();
        }
    }
}
