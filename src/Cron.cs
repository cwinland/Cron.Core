﻿using Cron.Core.Enums;
using Cron.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cron.Core
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
        public Cron(string expression) : this()
        {
            var cronArray = expression.Split(' ');

            var startingIndex = 0;

            if (cronArray.Length < 5)
            {
                throw new InvalidDataException($"This expression only has {cronArray.Length} parts. An expression must have 5, 6, or 7 parts.");
            }
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
        public string Description
        {
            get
            {
                var date = GetDate().Trim();
                var time = $"{GetTime()}{((date.Length > 0) ? "," : string.Empty)}".Trim();

                return $"{time} {date}".Trim();
            }
        }

        private string GetTime()
        {
            var seconds = GetProperty(CronTimeSections.Seconds);
            var minutes = GetProperty(CronTimeSections.Minutes);
            var hours = GetProperty(CronTimeSections.Hours);

            if (!seconds.Enabled &&
                !minutes.Enabled &&
                !hours.Enabled)
            {
                var text = !seconds.Enabled && !minutes.Enabled ? "minute" : "second";
                return $"Every {text}";
            }

            if (!IsTimeSpecific(seconds) ||
                !IsTimeSpecific(minutes) ||
                !IsTimeSpecific(hours))

            {
                var desc = new List<string>();

                if (seconds.Description.Length > 0)
                {
                    desc.Add(seconds.Description);
                }
                if (minutes.Description.Length > 0)
                {
                    desc.Add(minutes.Description);
                }
                else if (desc.Count == 0)
                {
                    desc.Add("Every minute");
                }
                if (hours.Description.Length > 0)
                {
                    desc.Add(hours.Description);
                }
                return string.Join(", ", desc);
            }
            else
            {
                const string FORMAT_STRING = "hh:mm:ss tt";
                const string FORMAT_STRING2 = "hh:mm tt";
                var time = new DateTime().AddHours(hours.ToInt()).AddMinutes(minutes.ToInt()).AddSeconds(seconds.ToInt());
                var formattedTime = (seconds.ToInt() > 0)
                    ? time.ToString(FORMAT_STRING)
                    : time.ToString(FORMAT_STRING2);
                var endTime = new DateTime().AddHours(hours.ToInt()).AddMinutes(59).AddSeconds(59);
                var formattedEndTime = endTime.ToString(FORMAT_STRING2);

                return ((!seconds.Enabled && !minutes.Enabled)
                    ? $"Every minute, {formattedTime}-{formattedEndTime}"
                    : $"At {formattedTime}");
            }
        }

        private static bool IsTimeSpecific(ISection section)
        {
            return !(section.Enabled && (section.Every || section.ContainsRange));
        }

        private string GetDate()
        {
            var desc = new List<string>();

            var dayMonth = GetProperty(CronTimeSections.DayMonth);
            var dayWeek = GetProperty(CronTimeSections.DayWeek);
            var months = GetProperty(CronTimeSections.Months);
            var years = GetProperty(CronTimeSections.Years);

            if (dayMonth.Description.Length > 0)
            {
                desc.Add(dayMonth.Description);
            }

            if (dayWeek.Description.Length > 0)
            {
                desc.Add(dayWeek.Description);
            }

            if (months.Description.Length > 0)
            {
                desc.Add(months.Description);
            }

            if (years.Description.Length > 0)
            {
                desc.Add(years.Description);
            }

            return string.Join(", ", desc);
        }

        private string GetSeperator(string current, CronTimeSections time)
        {
            return current.Length > 0 &&
                   GetProperty(time)
                       .Description
                       .Length >
                   0
                ? ","
                : string.Empty;
        }

        /// <inheritdoc cref="ICron" />
        public ISection Hours { get; private set; } = new Section(CronTimeSections.Hours);

        /// <inheritdoc cref="ICron" />
        public ISection Minutes { get; private set; } = new Section(CronTimeSections.Minutes);

        /// <inheritdoc cref="ICron" />
        public ISection Months { get; private set; } = new Section(CronTimeSections.Months);

        /// <inheritdoc cref="ICron" />
        public ISection Seconds { get; private set; } = new Section(CronTimeSections.Seconds);

        /// <inheritdoc cref="ICron" />
        public string Value => $"{Get(CronTimeSections.Seconds)} {Get(CronTimeSections.Minutes)} {Get(CronTimeSections.Hours)} {Get(CronTimeSections.DayMonth)} {Get(CronTimeSections.Months)} {Get(CronTimeSections.DayWeek)} {Get(CronTimeSections.Years)}";

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

        /// <inheritdoc cref="ICron" />
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

        /// <inheritdoc cref="ICron" />
        public DateTime Next(DateTime dateTime)
        {
            var nextDateTime = new DateTime(dateTime.Ticks);
            var timeDiff = GetProperty(CronTimeSections.Years)
                .Next(dateTime);

            timeDiff = timeDiff.Add(Months.Next(dateTime.Add(timeDiff)));
            timeDiff = timeDiff.Add(DayMonth.Next(dateTime.Add(timeDiff)));
            timeDiff = timeDiff.Add(DayWeek.Next(dateTime.Add(timeDiff)));
            timeDiff = timeDiff.Add(Hours.Next(dateTime.Add(timeDiff)));

            if (!Seconds.Enabled && !Minutes.Enabled && timeDiff.Minutes == 0)
            {
                timeDiff = timeDiff.Add(new TimeSpan(0, 1, 0));
            }
            else
            {
                timeDiff = timeDiff.Add(GetProperty(CronTimeSections.Minutes)
                                            .Next(dateTime.Add(timeDiff)));
                timeDiff = timeDiff.Add(GetProperty(CronTimeSections.Seconds)
                                            .Next(dateTime.Add(timeDiff)));
            }

            return nextDateTime.Add(timeDiff);
        }

        private static bool IsApplyTime(int arrayLength, CronTimeSections time, int index) => (arrayLength >= 6 || time != CronTimeSections.Seconds) && index < arrayLength;

        private string Get(CronTimeSections time) => GetProperty(time).ToString();

        private PropertyInfo GetFieldInfo(CronTimeSections time) => GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == time.ToString());

        private ISection GetProperty(CronTimeSections time) => GetFieldInfo(time).GetValue(this) as ISection;

        private void SetProperty(CronTimeSections time, ISection val) => GetFieldInfo(time).SetValue(this, val, null);

        private int SectionCount =>
            Value.Split(' ')
                .Length;

        #endregion Methods
    }
}
