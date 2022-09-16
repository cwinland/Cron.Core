using System;

namespace Cron.Core.Attributes
{
    /// <summary>
    ///     Class RangeAttribute.
    /// Implements the <see cref="Cron.Core.Attributes.RangeAttribute" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Cron.Core.Attributes.RangeAttribute" />
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RangeAttribute<T> : RangeAttribute where T : struct
    {
        /// <summary>
        ///     Gets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public new T MinValue => base.MinValue == default ? default : (T) base.MinValue;
        /// <summary>
        ///     Gets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public new T MaxValue => base.MaxValue == default ? default : (T) base.MaxValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeAttribute{T}"/> class.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public RangeAttribute(T minValue, T maxValue) : base(typeof(T), (IComparable)minValue, (IComparable)maxValue)
        {
        }

        /// <summary>
        ///     Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(RangeAttribute<T> other) => string.CompareOrdinal(ToString(), other?.ToString());
    }

    /// <summary>
    ///     Class RangeAttribute.
    /// Implements the <see cref="Cron.Core.Attributes.RangeAttribute" />
    /// </summary>
    /// <seealso cref="Cron.Core.Attributes.RangeAttribute" />
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RangeAttribute : Attribute, IComparable<RangeAttribute>
    {
        /// <summary>
        ///     Gets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public IComparable MinValue { get; }
        /// <summary>
        ///     Gets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public IComparable MaxValue { get; }
        /// <summary>
        ///     Gets the type of the range.
        /// </summary>
        /// <value>The type of the range.</value>
        public Type RangeType { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public RangeAttribute(Type type, IComparable minValue, IComparable maxValue)
        {
            RangeType = type;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <inheritdoc />
        public override string ToString() => $"{MinValue}-{MaxValue}";

        private object GetDefault() => RangeType.IsValueType ? Activator.CreateInstance(RangeType) : null;

        /// <summary>
        ///     Determines whether [is valid range] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is valid range] [the specified value]; otherwise, <c>false</c>.</returns>
        public virtual bool IsValidRange(IComparable value)
        {
            return value.IsObjectGreaterThanOrEqualTo(MinValue) && value.IsObjectLessThanOrEqualTo(MaxValue);
        }
        /// <summary>
        ///     Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(RangeAttribute other) => string.CompareOrdinal(ToString(), other?.ToString());
    }
}
