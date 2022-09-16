using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cron.Core
{
    public class NumberParts : IEnumerable<NumberPart>
    {
        #region Properties

        public List<NumberPart> Numbers { get; internal set; }

        public bool IsDirty => Numbers.Any(x => x.IsDirty);

        public bool Overwrite { get; set; }

        #endregion

        public NumberParts() => Numbers = new();

        public NumberParts(string initialValue) : this() => Add(initialValue).IsDirty = false;

        public NumberPart Add(NumberPart part)
        {
            part.IsDirty = true;
            Numbers.Add(part);
            return part;
        }

        public void ForEach(Action<NumberPart> action)
        {
            Numbers.ForEach(action);
        }

        public NumberPart Add(string value) => Add(new NumberPart(value));

        public NumberParts Clear()
        {
            Numbers.Clear();
            return this;
        }

        public NumberParts Reset()
        {
            Clear().Overwrite = false;
            return this;
        }

        public int Count => Numbers.Count;

        public NumberPart Replace(NumberPart part)
        {
            Numbers.Clear();
            return Add(part);
        }

        public NumberPart Replace(string value) => Replace(new NumberPart(value));

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => Numbers.GetEnumerator();

        #endregion

        #region IEnumerable<NumberPart>

        public IEnumerator<NumberPart> GetEnumerator() => Numbers.GetEnumerator();

        #endregion
    }
}