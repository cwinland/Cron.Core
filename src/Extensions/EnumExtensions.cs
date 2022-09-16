using System;
using System.Collections.Generic;
using System.Linq;

namespace Cron.Core.Extensions
{
    public static class EnumExtensions
    {
        public static List<T> ToNameList<T>(this T _) where T : struct, Enum => Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}