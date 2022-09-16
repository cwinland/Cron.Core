using System;
using System.Collections.Generic;
using System.Linq;

namespace Cron.Core.Extensions
{
    /// <summary>
    ///     Class EnumExtensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Converts to namelist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_">The .</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> ToNameList<T>(this T _) where T : struct, Enum => Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}