using Cron.Interfaces;
using System;
using System.Collections.Generic;

namespace Cron
{
    /// <inheritdoc cref="ISectionValues" />
    public class SectionValues : ISectionValues
    {
        private readonly int? maxValue;

        internal SectionValues(int val) => MinValue = val;

        internal SectionValues(int minVal, int maxVal) : this(minVal) => maxValue = maxVal;

        /// <inheritdoc cref="ISectionValues" />
        public int MaxValue => maxValue ?? MinValue;

        /// <inheritdoc cref="ISectionValues" />
        public int MinValue { get; }

        /// <inheritdoc cref="ISectionValues" />
        public override string ToString()
        {
            return MaxValue == MinValue
                ? MinValue.ToString()
                : $"{MinValue}-{MaxValue}";
        }

        /// <inheritdoc cref="ISectionValues" />
        public static explicit operator SectionValues(List<ISectionValues> v)
        {
            throw new NotImplementedException();
        }
    }
}
