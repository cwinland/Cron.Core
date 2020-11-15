// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : Chris Winland
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="Cron.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using Cron.Core.Enums;
using Cron.Core.Interfaces;
using Cron.Core.Sections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cron.Core
{
    /// <summary>
    /// Represents a Cron expression, Description, and object that can be used to create and modify cron expressions.
    /// Implements the <see cref="Core.Interfaces.ICron" />
    /// </summary>
    /// <example>
    /// Create a Cron object.
    /// <code>
    /// schedule = new Cron.Core.Cron();
    /// </code>
    /// Add Cron sections by section.
    /// <code>
    /// schedule.Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)
    /// schedule.Add(CronTimeSections.Minutes, 4)
    /// schedule.Add(CronTimeSections.Hours, 3, 5)
    /// </code>
    /// Add time sections by months.
    /// <code>
    /// schedule.Add(CronMonths.March)
    /// </code>
    /// Add time section by Day.
    /// <code>
    /// schedule.Add(CronDays.Wednesday)
    /// </code>
    /// Chain sections.
    /// <code>
    /// schedule = new Cron()
    /// .Add(CronDays.Friday)
    /// .Add(CronTimeSections.DayMonth, dayMonth)
    /// .Add(CronTimeSections.Years, years, true);
    /// </code>
    /// Create Cron with an existing expression
    /// <code>
    /// var cron = new Cron(expression);
    /// </code>
    /// Remove single value entry.
    /// <code>
    /// cron.Remove(CronTimeSections.Seconds, 5);
    /// </code>
    /// Remove Multiple value entry.
    /// <code>
    /// cron.Remove(CronTimeSections.Seconds, 5, 6);
    /// </code>
    /// Create Initial Cron with Days
    /// <code>
    /// var cron = new Cron
    /// {
    /// { CronDays.Thursday, CronDays.Saturday }
    /// };
    /// </code>
    /// Create Initial Cron with Months.
    /// <code>
    /// var cron = new Cron { { CronMonths.August, CronMonths.November } };
    /// </code>
    /// Reset Day of the Week section only.
    /// <code>
    /// cron.Reset(CronTimeSections.DayWeek);
    /// </code>
    /// Reset all sections to the defaults.
    /// <code>
    /// cron.ResetAll();
    /// </code></example>
    /// <inheritdoc cref="ICron" />
    public class Cron : ICron
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Cron" /> class.
        /// </summary>
        /// <example>
        /// Create a Cron object.
        /// <code>
        /// schedule = new Cron.Core.Cron();
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public Cron()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cron" /> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <exception cref="InvalidDataException">This expression only has {cronArray.Length} parts. An expression must have 5, 6, or 7 parts.</exception>
        /// <example>
        /// Create Cron with an existing expression
        /// <code>
        /// var cron = new Cron(expression);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public Cron(string expression) : this()
        {
            var cronArray = expression.Split(' ');
            var startingIndex = 0;

            if (cronArray.Length < 5)
            {
                throw new InvalidDataException($"This expression only has {cronArray.Length} parts. An expression must have 5, 6, or 7 parts.");
            }

            SectionList.Where(section => section.SectionType == CronSectionType.Time)
                       .ToList()
                       .ForEach(section => Set(
                                    IsApplyTime(cronArray.Length, section.Time, startingIndex)
                                        ? new TimeSection(
                                            section.Time,
                                            cronArray[startingIndex++])
                                        : new TimeSection(section.Time, false)
                                ));

            SectionList.Where(section => section.SectionType == CronSectionType.Date)
                       .ToList()
                       .ForEach(section => Set(
                                    IsApplyTime(cronArray.Length, section.Time, startingIndex)
                                        ? new DateSection(
                                            section.Time,
                                            cronArray[startingIndex++])
                                        : new DateSection(section.Time, false)
                                ));

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Day of the Month Information
        /// </summary>
        /// <value>The day month.</value>
        /// <inheritdoc cref="ICron" />
        public ISection DayMonth { get; private set; } = new DateSection(CronTimeSections.DayMonth);

        /// <summary>
        /// Day of the Week Information
        /// </summary>
        /// <value>The day week.</value>
        /// <inheritdoc cref="ICron" />
        public ISection DayWeek { get; private set; } = new DateSection(CronTimeSections.DayWeek);

        /// <summary>
        /// Human readable description.
        /// </summary>
        /// <value>The description.</value>
        /// <inheritdoc cref="ICron" />
        public string Description
        {
            get
            {
                var date = GetDate().Trim();
                var time = $"{GetTime()}{(date.Length > 0 ? "," : string.Empty)}".Trim();

                return $"{time} {date}".Trim();
            }
        }

        /// <summary>
        /// Hours Information
        /// </summary>
        /// <value>The hours.</value>
        /// <inheritdoc cref="ICron" />
        public ISection Hours { get; private set; } = new TimeSection(CronTimeSections.Hours);

        /// <summary>
        /// Minutes Information
        /// </summary>
        /// <value>The minutes.</value>
        /// <inheritdoc cref="ICron" />
        public ISection Minutes { get; private set; } = new TimeSection(CronTimeSections.Minutes);

        /// <summary>
        /// Months Information
        /// </summary>
        /// <value>The months.</value>
        /// <inheritdoc cref="ICron" />
        public ISection Months { get; private set; } = new DateSection(CronTimeSections.Months);

        /// <summary>
        /// Seconds Information
        /// </summary>
        /// <value>The seconds.</value>
        /// <inheritdoc cref="ICron" />
        public ISection Seconds { get; private set; } = new TimeSection(CronTimeSections.Seconds);

        /// <summary>
        /// Get a list of sections.
        /// </summary>
        /// <value>The section list.</value>
        public List<ISection> SectionList =>
            new SortedList<int, ISection>(
                typeof(Cron)
                    .GetRuntimeProperties()
                    .Where(x => x.PropertyType == typeof(ISection))
                    .Select(x => (ISection) x.GetValue(this))
                    .ToList()
                    .ToDictionary(s => (int) s.Time)
            ).Values.ToList();

        /// <summary>
        /// Cron Expression.
        /// </summary>
        /// <value>The value.</value>
        /// <inheritdoc cref="ICron" />
        public string Value => $"{Get(CronTimeSections.Seconds)} {Get(CronTimeSections.Minutes)} {Get(CronTimeSections.Hours)} {Get(CronTimeSections.DayMonth)} {Get(CronTimeSections.Months)} {Get(CronTimeSections.DayWeek)} {Get(CronTimeSections.Years)}";

        /// <summary>
        /// Year Information
        /// </summary>
        /// <value>The years.</value>
        /// <inheritdoc cref="ICron" />
        public ISection Years { get; private set; } = new DateSection(CronTimeSections.Years);

        private static List<CronTimeSections> TimeSections =>
            new SortedList<int, CronTimeSections>(
            Enum.GetValues(typeof(CronTimeSections))
                .Cast<CronTimeSections>()
                .ToList().ToDictionary(s => (int)s)).Values.ToList();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add time value for the specified time section.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// <param name="repeatEvery">Indicates if the value is a repeating time or specific time.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">repeatEvery</exception>
        /// <exception cref="ArgumentOutOfRangeException">repeatEvery</exception>
        /// <example>
        /// Add Cron sections by section.
        /// <code>
        /// schedule.Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)
        /// schedule.Add(CronTimeSections.Minutes, 4)
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public ICron Add(CronTimeSections time, int value, bool repeatEvery = false)
        {
            if (!Section.IsTimeCronSection(time) && repeatEvery)
            {
                throw new ArgumentOutOfRangeException(nameof(repeatEvery));
            }

            var prop = GetProperty(time);

            prop.Add(value);
            prop.Every = Section.IsTimeCronSection(time) && repeatEvery;

            return this;
        }

        /// <summary>
        /// Add time value for the specified time section.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="minValue">Starting value for the specified time section.</param>
        /// <param name="maxValue">Ending value for the specified time section.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <example>
        /// Add Cron sections by section.
        /// <code>
        /// schedule.Add(CronTimeSections.Hours, 3, 5);
        /// schedule.Add(CronTimeSections.Years, 2020, 2022);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public ICron Add(CronTimeSections time, int minValue, int maxValue)
        {
            var prop = GetProperty(time)
                .Add(minValue, maxValue);
            prop.Every = false;

            return this;
        }

        /// <summary>
        /// Add day of the week.
        /// </summary>
        /// <param name="value"><see cref="CronDays" /> representing the day of the week.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <example>
        /// Add time section by Day.
        /// <code>
        /// schedule.Add(CronDays.Wednesday);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public ICron Add(CronDays value) =>
            Add(CronTimeSections.DayWeek, (int)value);

        /// <summary>
        /// Add month.
        /// </summary>
        /// <param name="value"><see cref="CronMonths" /> representing the month.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <example>
        /// Add time sections by months.
        /// <code>
        /// schedule.Add(CronMonths.March);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public ICron Add(CronMonths value) =>
            Add(CronTimeSections.Months, (int)value);

        /// <summary>
        /// Add range of days in the week.
        /// </summary>
        /// <param name="minValue">Starting <see cref="CronDays" /> representing the day of the week.</param>
        /// <param name="maxValue">Ending <see cref="CronDays" /> representing the day of the week.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <example>
        /// Add time section by Day.
        /// <code>
        /// schedule.Add(CronDays.Wednesday, CronDays.Friday);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public ICron Add(CronDays minValue, CronDays maxValue) =>
            Add(CronTimeSections.DayWeek, (int)minValue, (int)maxValue);

        /// <summary>
        /// Add range of months.
        /// </summary>
        /// <param name="minValue">Starting <see cref="CronMonths" /> representing the day of the month.</param>
        /// <param name="maxValue">Ending <see cref="CronMonths" /> representing the day of the month.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <example>
        /// Add time sections by months.
        /// <code>
        /// schedule.Add(CronMonths.March, CronMonths.August);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public ICron Add(CronMonths minValue, CronMonths maxValue) =>
            Add(CronTimeSections.Months, (int)minValue, (int)maxValue);

        /// <inheritdoc />
        public IEnumerator<CronTimeSections> GetEnumerator() => TimeSections.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// Remove single value entry.
        /// <code>
        /// cron.Remove(CronTimeSections.Seconds, 5);
        /// </code><inheritdoc cref="ICron" />
        public ICron Remove(CronTimeSections time, int value)
        {
            GetProperty(time)
                .Remove(value);

            return this;
        }

        /// <summary>
        /// Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="minValue">Starting value for the specified time section.</param>
        /// <param name="maxValue">Ending value for the specified time section.</param>
        /// Remove Multiple value entry.
        /// <code>
        /// cron.Remove(CronTimeSections.Seconds, 5, 6);
        /// </code><inheritdoc cref="ICron" />
        public ICron Remove(CronTimeSections time, int minValue, int maxValue)
        {
            GetProperty(time)
                .Remove(minValue, maxValue);

            return this;
        }

        /// <summary>
        /// Clear the specific time element of the Cron object to defaults without any numerical cron representations.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <inheritdoc cref="ICron" />
        public ICron Reset(CronTimeSections time)
        {
            GetProperty(time).Clear();

            return this;
        }

        /// <summary>
        /// Resets the specified section.
        /// </summary>
        /// <param name="section"><see cref="ISection"/></param>
        /// <returns><see cref="ICron" /></returns>
        public ICron Reset(ISection section)
        {
            section.Clear();

            return this;
        }

        /// <summary>
        /// Clear the entire Cron object to defaults without any numerical cron representations.
        /// </summary>
        /// <returns><see cref="ICron" /></returns>
        /// <inheritdoc cref="ICron" />
        public ICron Reset()
        {
            SectionList.ForEach(section => section.Clear());

            return this;
        }

        /// <summary>
        /// Set time with <see cref="ISection" /> value.
        /// </summary>
        /// <param name="value">Value for the specified time section.</param>
        /// <returns><see cref="ICron" /></returns>
        /// <inheritdoc cref="ICron" />
        public ICron Set(ISection value)
        {
            GetFieldInfo(value.Time)
                .SetValue(this, value, null);
            return this;
        }

        //public ICron Set(CronTimeSections time, IEnumerable<ISectionValues> value)
        //{
        //    GetFieldInfo(time).SetValue(this, value, null);

        //    return this;
        //}

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <inheritdoc cref="ICron" />
        public override string ToString() => Value;

        private static bool IsApplyTime(int arrayLength, CronTimeSections time, int index) =>
            (arrayLength >= 6 || time != CronTimeSections.Seconds)
            && index < arrayLength;

        private static bool IsTimeSpecific(ISection section) => !(section.Enabled && (section.Every || section.ContainsRange));

        private string Get(CronTimeSections time) => GetProperty(time).ToString();

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

        private PropertyInfo GetFieldInfo(CronTimeSections time) => GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == time.ToString());

        private ISection GetProperty(CronTimeSections time) => GetFieldInfo(time).GetValue(this) as ISection;

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

            const string FORMAT_STRING = "hh:mm:ss tt";
            const string FORMAT_STRING2 = "hh:mm tt";
            var time = new DateTime().AddHours(hours.ToInt()).AddMinutes(minutes.ToInt()).AddSeconds(seconds.ToInt());
            var formattedTime = seconds.ToInt() > 0
                ? time.ToString(FORMAT_STRING)
                : time.ToString(FORMAT_STRING2);
            var endTime = new DateTime().AddHours(hours.ToInt()).AddMinutes(59).AddSeconds(59);
            var formattedEndTime = endTime.ToString(FORMAT_STRING2);

            return !seconds.Enabled && !minutes.Enabled
                ? $"Every minute, {formattedTime}-{formattedEndTime}"
                : $"At {formattedTime}";
        }

        #endregion Methods

    }
}
