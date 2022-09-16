using System;

namespace Cron.Core.Attributes
{
    public static class ComparableExtensions
    {
        public static bool IsEqualTo<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) == 0;
        }

        public static bool IsGreaterThan<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) > 0;
        }
        public static bool IsGreaterThanOrEqualTo<T>(this T leftHand, T value) where T : struct, IComparable<T>
        {
            return leftHand.CompareTo(value) >= 0;
        }

        public static bool IsObjectGreaterThanOrEqualTo(this IComparable leftHand, IComparable value)
        {
            return leftHand.CompareTo(value) >= 0;
        }

        public static bool IsLessThan<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) < 0;
        }
        public static bool IsLessThanOrEqualTo<T>(this T leftHand, T value) where T : IComparable<T>
        {
            return leftHand.CompareTo(value) <= 0;
        }

        public static bool IsObjectLessThanOrEqualTo(this IComparable leftHand, IComparable value) => leftHand.CompareTo(value) <= 0;
    }
}