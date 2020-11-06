using Cron.Interfaces;
using System;
using System.Collections.Generic;

namespace Cron
{
    public class SectionValues : ISectionValues
    {
        private readonly int? maxValue;

        internal SectionValues(int val) => MinValue = val;

        internal SectionValues(int minVal, int maxVal) : this(minVal) => maxValue = maxVal;

        public int MaxValue => maxValue ?? MinValue;

        public int MinValue { get; }

        public override string ToString()
        {
            return MaxValue == MinValue
                ? MinValue.ToString()
                : $"{MinValue}-{MaxValue}";
        }

        public static explicit operator SectionValues(List<ISectionValues> v)
        {
            throw new NotImplementedException();
        }
    }
}
