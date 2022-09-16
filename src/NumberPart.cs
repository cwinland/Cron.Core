namespace Cron.Core
{
    public class NumberPart
    {
        #region Fields

        private string number;

        #endregion

        #region Properties

        public string Number
        {
            get => number;
            internal set
            {
                number = value;
                IsDirty = number != "*" && number != "?";
            }
        }

        public bool IsDirty { get; internal set; }

        #endregion

        public NumberPart(string number)
        {
            Number = number;
            IsDirty = false;
        }
    }
}