using Cron.Core.Attributes;
using Cron.Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cron.Core.Extensions
{
    /// <summary>
    ///     Class CronBuilderExtensions.
    /// </summary>
    public static class CronBuilderExtensions
    {
        /// <summary>
        ///     Overwrites the specified overwrite.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns>ICronBuilder.</returns>
        public static ICronBuilder Overwrite(this ICronBuilder obj, bool overwrite = true)
        {
            if (obj is NewCronBuilder c)
            {
                if (c.IsDirty && c.LastCronType.HasValue)
                {
                    c.GetValues(c.LastCronType.Value).Overwrite = overwrite;
                }
                else
                {
                    CronType.Day.ToNameList()?.ForEach(x => c.GetValues(x).Overwrite = overwrite);
                }
            }

            return obj;
        }

        /// <summary>
        ///     Determines whether [is i enumerable] [the specified list].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="list">The list.</param>
        /// <returns><c>true</c> if [is i enumerable] [the specified list]; otherwise, <c>false</c>.</returns>
        public static bool IsIEnumerable(this object obj, out List<object> list)
        {
            if (obj?.GetType().Name != "String" && obj is IEnumerable l)
            {
                list = l.Cast<object>().ToList();

                return true;
            }

            list = null;
            return false;
        }

        /// <summary>
        ///     Gets the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static int? GetInt(this object value) => value is Enum ? (int) value : int.TryParse(value?.ToString(), out var newIntValue) ? newIntValue : null;
        /// <summary>
        ///     Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetString(this object value) => value.GetInt()?.ToString() ?? value?.ToString() ?? string.Empty;
        /// <summary>
        ///     Gets the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val1">The val1.</param>
        /// <param name="val2">The val2.</param>
        /// <returns>System.String.</returns>
        public static string GetRange<T>(this T val1, T val2) where T : struct
        {
            var int1 = val1.GetInt();
            var int2 = val2.GetInt();
            return int1 != null && int2 != null
                ? int1 <= int2 ? $"{int1}-{int2}" : $"{int2}-{int1}"
                : $"{val1}-{val2}";
        }

        /// <summary>
        ///     Checks the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronType">Type of the cron.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        internal static bool CheckRange<T>(this CronType cronType, T value)
        {
            if (value.IsIEnumerable(out var list))
            {
                foreach (var v in list.Where(listVal => !CheckRange(cronType, listVal)))
                {
                    throw new InvalidOperationException($"{v} is out of range.");
                }

                return true;
            }

            var t = typeof(T).IsEnum ? value.GetInt() : (IComparable) value;
            var memberData = typeof(CronType).GetMember(cronType.ToString()).FirstOrDefault();
            if (memberData == null)
            {
                return false;
            }

            var attrs = memberData.GetCustomAttributes(true).OfType<RangeAttribute>().ToArray();
            foreach (var attr in attrs)
            {
                if (attr is { } rAttr && rAttr.IsValidRange(t))
                {
                    return true;
                }
            }

            return false;
        }

    }
}