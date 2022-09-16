using Cron.Core.Enums;
using System;

namespace Cron.Core
{
    /// <summary>
    ///     Class NumberPart.
    /// </summary>
    public class NumberPart
    {
        #region Fields

        private string number;
        private bool isDirty;

        #endregion

        #region Properties

        public CronType CronType { get; }

        /// <summary>
        ///     Gets the number.
        /// </summary>
        /// <value>The number.</value>
        public string Number
        {
            get => number;
            private set
            {
                number = value;
                SetIsDirty();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public bool IsDirty
        {
            get => isDirty && ((CronType == CronType.Minute && number != "?" && number != "*") || (number != "?"));
            internal set => isDirty = value;
        }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="NumberPart"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public NumberPart(object number, CronType cronType)
        {
            CronType = cronType;
            Number = number.ToString();
            IsDirty = false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is NumberPart part ? part.Number.Equals(Number, StringComparison.OrdinalIgnoreCase) : false;

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => Number.GetHashCode();

        private void SetIsDirty() => IsDirty = Number is not "*" and not "?";
    }
}