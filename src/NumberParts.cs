using Cron.Core.Enums;
using Cron.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cron.Core
{
    /// <summary>
    ///     Class NumberParts.
    ///     Implements the <see cref="System.Collections.Generic.IEnumerable{Cron.Core.NumberPart}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Cron.Core.NumberPart}" />
    public class NumberParts : IEnumerable<NumberPart>
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public bool IsDirty => Numbers.Any(x => x.IsDirty);

        /// <summary>
        ///     Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => Numbers.Count;

        public CronType CronType { get; }

        /// <summary>
        ///     Gets the numbers.
        /// </summary>
        /// <value>The numbers.</value>
        internal List<NumberPart> Numbers { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="NumberParts" /> is overwrite.
        /// </summary>
        /// <value><c>true</c> if overwrite; otherwise, <c>false</c>.</value>
        internal bool Overwrite { get; set; }

        /// <summary>
        ///     Gets the source.
        /// </summary>
        /// <value>The source.</value>
        internal List<object> Source { get; }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="NumberParts" /> class.
        /// </summary>
        public NumberParts(CronType cronType)
        {
            CronType = cronType;
            Numbers = new();
            Source = new();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NumberParts" /> class.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public NumberParts(string initialValue, CronType cronType) : this(cronType)
        {
            Add(initialValue, true);
        }

        /// <summary>
        ///     Adds the specified part.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <returns>NumberPart.</returns>
        public NumberPart Add(NumberPart part)
        {
            part.IsDirty = true;
            Numbers.Add(part);
            return part;
        }

        /// <summary>
        ///     Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="preventDirty">if set to <c>true</c> [prevent dirty].</param>
        public void Add(object value, bool preventDirty = false)
        {
            if (value.IsIEnumerable(out var list))
            {
                foreach (var o in list.Cast<object>())
                {
                    object s = o.GetInt() == null ? o.GetString() : o.GetInt();
                    if (!Source.Contains(s))
                    {
                        var part = new NumberPart(s, CronType);

                        Add(part);
                        if (preventDirty)
                        {
                            part.IsDirty = false;
                        }

                        Source.Add(s);
                    }
                }
            }
            else if (!Source.Contains(value.GetString()))
            {
                object v = value.GetInt() == null ? value.GetString() : value.GetInt();
                var part = new NumberPart(v, CronType);

                Add(part);
                if (preventDirty)
                {
                    part.IsDirty = false;
                }

                Source.Add(v);
            }
        }

        /// <summary>
        ///     Adds the specified minimum value.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public void Add(object minValue, object maxValue)
        {
            var minInt = minValue.GetInt();
            var maxInt = maxValue.GetInt();
            if (minInt != null && maxInt != null)
            {
                AddIntByRange(minInt.Value, maxInt.Value);
            }
        }

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        /// <returns>NumberParts.</returns>
        public NumberParts Clear()
        {
            Numbers.Clear();
            Source.Clear();
            return this;
        }

        /// <summary>
        ///     Determines whether this instance contains the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool Contains(string value) => Numbers.Any(x => x.Number.Equals(value, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        ///     Determines whether this instance contains the object.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <returns><c>true</c> if [contains] [the specified part]; otherwise, <c>false</c>.</returns>
        public bool Contains(NumberPart part) => Numbers.Any(x => x.Number.Equals(part.Number, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        ///     Fors the each.
        /// </summary>
        /// <param name="action">The action.</param>
        public void ForEach(Action<NumberPart> action) => Numbers.ForEach(action);

        /// <summary>
        ///     Gets the numbers.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetNumbers() => Numbers?.Select(x => x.Number).OrderBy(x => x, new NumberComparer()).ToList();

        /// <summary>
        ///     Replaces the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Replace(object value)
        {
            Clear();
            Add(value);
        }

        /// <summary>
        ///     Replaces the specified minimum value.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public void Replace(object minValue, object maxValue)
        {
            Clear();
            Add(minValue, maxValue);
        }

        /// <summary>
        ///     Resets this instance.
        /// </summary>
        /// <returns>NumberParts.</returns>
        public NumberParts Reset()
        {
            Clear().Overwrite = false;
            return this;
        }

        /// <summary>
        ///     Adds the int by range.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        internal void AddIntByRange(int minValue, int maxValue)
        {
            var sb = new StringBuilder();

            int? lastValue = null;
            int? lastAdded = null;
            for (var v = minValue; v <= maxValue; v++)
            {
                if (!Source.Contains(v))
                {
                    Source.Add(v);
                    if (!lastAdded.HasValue)
                    {
                        sb.Append(v);
                        lastAdded = v;
                        lastValue = v;
                    }
                    else if (v == maxValue)
                    {
                        sb.Append("-");
                        sb.Append(v);
                        lastAdded = null;
                        lastValue = null;
                    }
                    else
                    {
                        lastValue = v;
                    }
                }
                else if (lastValue != null && lastValue != lastAdded)
                {
                    sb.Append("-");
                    sb.Append(lastValue);
                    lastAdded = null;
                    lastValue = null;
                }
            }

            if (sb.ToString().Length > 0)
            {
                Add(new NumberPart(sb.ToString(), CronType));
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Numbers.Equals(obj);

        /// <inheritdoc />
        public override int GetHashCode() => Numbers.GetHashCode();

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => Numbers.GetEnumerator();

        #endregion

        #region IEnumerable<NumberPart>

        /// <summary>
        ///     Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;NumberPart&gt;.</returns>
        public IEnumerator<NumberPart> GetEnumerator() => Numbers.GetEnumerator();

        #endregion

        /// <summary>
        ///     Class NumberComparer.
        ///     Implements the <see cref="System.Collections.Generic.IComparer{System.String}" />
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{System.String}" />
        internal class NumberComparer : IComparer<string>
        {
            #region IComparer<string>

            /// <summary>
            ///     Compares the specified a.
            /// </summary>
            /// <param name="a">a.</param>
            /// <param name="b">The b.</param>
            /// <returns>System.Int32.</returns>
            public int Compare(string a, string b)
            {
                var valA = a.Split('-')[0].GetInt();
                var valB = b.Split('-')[0].GetInt();
                return valA < valB ? -1 : valA > valB ? 1 : 0;
            }

            #endregion
        }
    }
}