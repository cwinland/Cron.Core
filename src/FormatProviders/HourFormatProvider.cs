using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Cron.Core.FormatProviders
{
    public class HourFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public string Format(string fmt, object arg, IFormatProvider formatProvider)
        {
            // Provide default formatting if arg is not an Int64.
            if (!int.TryParse(arg.ToString(), out var num))
                try
                {
                    return HandleOtherFormats(fmt, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException($"The format of '{fmt}' is invalid.", e);
                }

            // Provide default formatting for unsupported format strings.
            string ufmt = fmt.ToUpper(CultureInfo.InvariantCulture);

            if (!(ufmt == "H" || ufmt == "I"))
                try
                {
                    return HandleOtherFormats(fmt, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException($"The format of '{fmt}' is invalid.", e);
                }

            // Convert argument to a string.
            return new DateTime(0, 0, 0, num, 0, 0).ToString(fmt);
        }

        private string HandleOtherFormats(string format, object arg)
        {
            return (arg is IFormattable formattable)
                ? formattable.ToString(format, CultureInfo.CurrentCulture)
                : (arg == null)
                    ? string.Empty
                    : arg.ToString();
        }
    }
}
