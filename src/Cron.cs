using System;
using System.Collections.Generic;
using CronExpressionDescriptor;
using System.Linq;
using System.Reflection;
using Cron.Enums;
using Cron.Interfaces;
using System.Collections;

namespace Cron
{
    /// <inheritdoc cref="ICron" />
    public class Cron : ICron
    {
        #region Constructors

        /// <inheritdoc cref="ICron" />
        public Cron()
        {
        }

        /// <inheritdoc cref="ICron" />
        public Cron(string expression)
        {
            var cronArray = expression.Split(' ');

            var startingIndex = 0;

            foreach (CronTimeSections timeSection in this)
            {
                SetProperty(timeSection, IsApplyTime(cronArray.Length, timeSection, startingIndex)
                    ? new Section(timeSection, cronArray[startingIndex++])
                    : DisabledSection);
            }
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc cref="ICron" />
        public ISection DayMonth { get; private set; } = new Section(CronTimeSections.DayMonth);

        /// <inheritdoc cref="ICron" />
        public ISection DayWeek { get; private set; } = new Section(CronTimeSections.DayWeek);

        /// <inheritdoc cref="ICron" />
        public string Description => ExpressionDescriptor.GetDescription(Value);

        /// <inheritdoc cref="ICron" />
        public ISection Hours { get; private set; } = new Section(CronTimeSections.Hours);

        /// <inheritdoc cref="ICron" />
        public ISection Minutes { get; private set; } = new Section(CronTimeSections.Minutes);

        /// <inheritdoc cref="ICron" />
        public ISection Months { get; private set; } = new Section(CronTimeSections.Months);

        /// <inheritdoc cref="ICron" />
        public ISection Seconds { get; private set; } = new Section(CronTimeSections.Seconds);

        /// <inheritdoc cref="ICron" />
        public string Value => $"{Get(CronTimeSections.Seconds)} {Get(CronTimeSections.Minutes)} {Get(CronTimeSections.Hours)} {Get(CronTimeSections.DayMonth)} {Get(CronTimeSections.DayWeek)} {Get(CronTimeSections.Months)} {Get(CronTimeSections.Years)}";

        /// <inheritdoc cref="ICron" />
        public ISection Years { get; private set; } = new Section(CronTimeSections.Years);

        private static ISection DisabledSection => new Section(0) { Enabled = false };

        #endregion Properties

        #region Methods

        /// <inheritdoc cref="ICron" />
        public ICron Add(CronTimeSections time, int value, bool repeatEvery = false)
        {
            GetProperty(time).Add(value);
            if (repeatEvery)
            {
                GetProperty(time).Every = true;
            }

            return this;
        }

        /// <inheritdoc cref="ICron" />
        public ICron Add(CronTimeSections time, int minValue, int maxValue)
        {
            GetProperty(time)
                .Add(minValue, maxValue)
                .Every = false;
            return this;
        }

        /// <inheritdoc cref="ICron" />
        public ICron Add(CronDays value, bool repeatEvery = false) =>
            Add(CronTimeSections.DayWeek, (int)value, repeatEvery);

        /// <inheritdoc cref="ICron" />
        public ICron Add(CronMonths value, bool repeatEvery = false) =>
            Add(CronTimeSections.Months, (int)value, repeatEvery);

        /// <inheritdoc cref="ICron" />
        public ICron Add(CronDays minValue, CronDays maxValue) =>
            Add(CronTimeSections.DayWeek, (int)minValue, (int)maxValue);

        /// <inheritdoc cref="ICron" />
        public ICron Add(CronMonths minValue, CronMonths maxValue) =>
            Add(CronTimeSections.Months, (int)minValue, (int)maxValue);

        /// <inheritdoc cref="ICron" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enum.GetValues(typeof(CronTimeSections))
                .GetEnumerator();
        }

        /// <inheritdoc cref="ICron" />
        public void Remove(CronTimeSections time, int value) =>
            GetProperty(time)
                .Remove(value);

        public void Remove(CronTimeSections time, int minValue, int maxValue) =>
            GetProperty(time)
                .Remove(minValue, maxValue);

        /// <inheritdoc cref="ICron" />
        public ICron Reset(CronTimeSections time) => Set(time, new Section(time));

        /// <inheritdoc cref="ICron" />
        public ICron ResetAll()
        {
            foreach (CronTimeSections cronTimeSections in this)
            {
                Reset(cronTimeSections);
            }

            return this;
        }

        /// <inheritdoc cref="ICron" />
        public ICron Set(CronTimeSections time, ISection value)
        {
            SetProperty(time, value);
            return this;
        }

        /// <inheritdoc cref="ICron" />
        public override string ToString() => Value;

        private static bool IsApplyTime(int arrayLength, CronTimeSections time, int index) => ((arrayLength >= 6 || time != CronTimeSections.Seconds) && index < arrayLength);

        private string Get(CronTimeSections time) => GetProperty(time).ToString();

        private PropertyInfo GetFieldInfo(CronTimeSections time) => GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == time.ToString());

        private ISection GetProperty(CronTimeSections time) => GetFieldInfo(time).GetValue(this) as ISection;

        private void SetProperty(CronTimeSections time, ISection val) => GetFieldInfo(time).SetValue(this, val, null);

        #endregion Methods
    }
}
