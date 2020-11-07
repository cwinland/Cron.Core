namespace Cron.Interfaces
{
    /// <summary>
    ///   Stores the values for a section list.
    /// </summary>
    public interface ISectionValues
    {
        /// <summary>
        ///   Maximum value in a value range.
        /// </summary>
        int MaxValue { get; }

        /// <summary>
        ///   Minimum value in a value range. Also represents the only value, if the section is not a range.
        /// </summary>
        int MinValue { get; }
    }
}
