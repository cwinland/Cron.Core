using Cron.Core.Enums;

namespace Cron.Core
{
    /// <summary>
    ///     Class CronExpression.
    /// Implements the <see cref="Cron.Core.ICronExpression" />
    /// </summary>
    /// <seealso cref="Cron.Core.ICronExpression" />
    public class CronExpression : ICronExpression
    {
        #region Properties

        /// <summary>
        ///     Gets the minute.
        /// </summary>
        /// <value>The minute.</value>
        public string Minute { get; }
        /// <summary>
        ///     Gets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public string Hour { get; }
        /// <summary>
        ///     Gets the day of month.
        /// </summary>
        /// <value>The day of month.</value>
        public string DayOfMonth { get; }
        /// <summary>
        ///     Gets the month.
        /// </summary>
        /// <value>The month.</value>
        public string Month { get; }
        /// <summary>
        ///     Gets the weekday.
        /// </summary>
        /// <value>The weekday.</value>
        public string Weekday { get; }
        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description => ToString();

        /// <summary>
        ///     Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        public ICronBuilder Builder { get; internal set; }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="CronExpression" /> class.
        /// </summary>
        /// <param name="minute">The minute.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="dayOfMonth">The day of month.</param>
        /// <param name="month">The month.</param>
        /// <param name="weekday">The weekday.</param>
        public CronExpression(string minute, string hour, string dayOfMonth, string month, string weekday)
        {
            Minute = minute;
            Hour = hour;
            DayOfMonth = dayOfMonth;
            Month = month;
            Weekday = weekday;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CronExpression" /> class.
        /// </summary>
        /// <param name="minute">The minute.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="dayOfMonth">The day of month.</param>
        /// <param name="month">The month.</param>
        /// <param name="weekday">The weekday.</param>
        public CronExpression(string minute, string hour, string dayOfMonth, CronMonths month, CronDays weekday)
            : this(minute, hour, dayOfMonth, ((int) month).ToString(), ((int) weekday).ToString()) { }

        /// <inheritdoc />
        public override string ToString() => $"{Minute} {Hour} {DayOfMonth} {Month} {Weekday}";
    }
}