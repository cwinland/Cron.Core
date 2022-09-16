using System;

namespace Cron.Core.Attributes
{
    /// <summary>
    ///     Class ComparableExtensions.
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        ///     Determines whether [is equal to] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsEqualTo<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) == 0;
        }

        /// <summary>
        ///     Determines whether [is greater than] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsGreaterThan<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) > 0;
        }
        /// <summary>
        ///     Determines whether [is greater than or equal to] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsGreaterThanOrEqualTo<T>(this T leftHand, T value) where T : struct, IComparable<T>
        {
            return leftHand.CompareTo(value) >= 0;
        }

        /// <summary>
        ///     Determines whether [is object greater than or equal to] [the specified value].
        /// </summary>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is object greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsObjectGreaterThanOrEqualTo(this IComparable leftHand, IComparable value)
        {
            return leftHand.CompareTo(value) >= 0;
        }

        /// <summary>
        ///     Determines whether [is less than] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsLessThan<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) < 0;
        }
        /// <summary>
        ///     Determines whether [is less than or equal to] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsLessThanOrEqualTo<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) <= 0;
        }

        /// <summary>
        ///     Determines whether [is object less than or equal to] [the specified value].
        /// </summary>
        /// <param name="leftHand">The left hand.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is object less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsObjectLessThanOrEqualTo(this IComparable leftHand, IComparable value) => leftHand.CompareTo(value) <= 0;
    }
}