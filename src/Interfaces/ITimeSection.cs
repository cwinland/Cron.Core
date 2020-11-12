using System.Collections.Generic;

namespace Cron.Core.Interfaces
{
    /// <summary>
    /// Section specifically for Time (seconds, minutes, hours).
    /// </summary>
    public interface ITimeSection : ISection
    {
        /// <summary>
        /// List of allowed increments when the Every option is used.
        /// </summary>
        List<int> AllowedIncrements { get; }
    }
}
