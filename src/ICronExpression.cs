namespace Cron.Core
{
    /// <summary>
    ///     Interface ICronExpression
    /// </summary>
    public interface ICronExpression
    {
        #region Properties

        /// <summary>
        ///     Gets the minute.
        /// </summary>
        /// <value>The minute.</value>
        string Minute { get; }
        /// <summary>
        ///     Gets the hour.
        /// </summary>
        /// <value>The hour.</value>
        string Hour { get; }
        /// <summary>
        ///     Gets the day of month.
        /// </summary>
        /// <value>The day of month.</value>
        string DayOfMonth { get; }
        /// <summary>
        ///     Gets the month.
        /// </summary>
        /// <value>The month.</value>
        string Month { get; }
        /// <summary>
        ///     Gets the weekday.
        /// </summary>
        /// <value>The weekday.</value>
        string Weekday { get; }
        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }
        /// <summary>
        ///     Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        ICronBuilder Builder { get; }
        #endregion
    }
}