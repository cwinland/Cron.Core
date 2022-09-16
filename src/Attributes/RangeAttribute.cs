using System;
using System.ComponentModel;
using static Cron.Core.Attributes.ComparableExtensions;

namespace Cron.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class RangeAttribute<T> : RangeAttribute where T : struct
    {
        public new T MinValue => base.MinValue == default ? default : (T) base.MinValue;
        public new T MaxValue => base.MaxValue == default ? default : (T) base.MaxValue;

        public RangeAttribute(T minValue, T maxValue) : base(typeof(T), (IComparable)minValue, (IComparable)maxValue)
        {
        }

        public int CompareTo(RangeAttribute<T> other) => string.CompareOrdinal(ToString(), other?.ToString());
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class RangeAttribute : Attribute, IComparable<RangeAttribute>
    {
        public IComparable MinValue { get; }
        public IComparable MaxValue { get; }
        public Type RangeType { get; }

        public RangeAttribute(Type type, IComparable minValue, IComparable maxValue)
        {
            RangeType = type;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <inheritdoc />
        public override string ToString() => $"{MinValue}-{MaxValue}";

        private object GetDefault() => RangeType.IsValueType ? Activator.CreateInstance(RangeType) : null;

        public virtual bool IsValidRange(IComparable value)
        {
            return value.IsObjectGreaterThanOrEqualTo(MinValue) && value.IsObjectLessThanOrEqualTo(MaxValue);
        }
        public int CompareTo(RangeAttribute other) => string.CompareOrdinal(ToString(), other?.ToString());
    }
}
