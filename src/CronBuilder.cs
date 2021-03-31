// ***********************************************************************
// Assembly         : Cron.Core
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : Chris Winland
// Last Modified On : 11-17-2020
// ***********************************************************************
// <copyright file="CronBuilder.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cron.Core.Enums;
using Cron.Core.Interfaces;
using Cron.Core.Sections;

namespace Cron.Core
{
    /// <summary>
    /// Represents a Cron expression, Description, and object that can be used to create and modify cron expressions.
    /// Implements the <see cref="Core.Interfaces.ICron" />
    /// </summary>
    /// <example>
    /// Create a Cron object.
    /// <code>
    /// schedule = new CronBuilder();
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
    /// schedule = new CronBuilder()
    /// .Add(CronDays.Friday)
    /// .Add(CronTimeSections.DayMonth, dayMonth)
    /// .Add(CronTimeSections.Years, years, true);
    /// </code>
    /// Create Cron with an existing expression
    /// <code>
    /// var cron = new CronBuilder(expression);
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
    /// var cron = new CronBuilder
    /// {
    /// { CronDays.Thursday, CronDays.Saturday }
    /// };
    /// </code>
    /// Create Initial Cron with Months.
    /// <code>
    /// var cron = new CronBuilder { { CronMonths.August, CronMonths.November } };
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
    [Serializable]
    [JsonConverter(typeof(CronConverter))]
    public class CronBuilder : ICron
    {
        #region Fields

        private const string EVERY_MINUTE = "Every minute";
        private const string MINUTE_FORMAT = "hh:mm tt";
        private const string SECOND_FORMAT = "hh:mm:ss tt";

        #endregion Fields

        #region Constructors

        [JsonConstructor]
        public CronBuilder() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CronBuilder" /> class.
        /// </summary>
        /// <example>
        /// Create a Cron object.
        /// <code>
        /// schedule = new CronBuilder();
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public CronBuilder(bool allowSeconds = false) => AllowSeconds = allowSeconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="CronBuilder" /> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="allowSeconds">Use seconds in the calculations.</param>
        /// <exception cref="InvalidDataException">This expression only has {cronArray.Length} parts. An expression must have 5, 6, or 7 parts.</exception>
        /// <example>
        /// Create Cron with an existing expression
        /// <code>
        /// var cron = new CronBuilder(expression);
        /// </code></example>
        /// <inheritdoc cref="ICron" />
        public CronBuilder(string expression, bool allowSeconds = false) : this(allowSeconds) =>
            BuildExpression(expression);

        #endregion Constructors

        #region Properties

        /// <inheritdoc />
        [JsonInclude]
        public bool AllowSeconds { get; set; }

        /// <summary>
        /// Day of the Month Information
        /// </summary>
        /// <value>The day month.</value>
        /// <inheritdoc cref="ICron" />
        [JsonIgnore]
        public ISection DayMonth { get; private set; } = new DateSection<CronDays>(CronTimeSections.DayMonth);

        /// <summary>
        /// Day of the Week Information
        /// </summary>
        /// <value>The day week.</value>
        /// <inheritdoc cref="ICron" />
        [JsonIgnore]
        public ISection DayWeek { get; private set; } = new DateSection<DayOfWeek>(CronTimeSections.DayWeek);

        /// <summary>
        /// Human readable description.
        /// </summary>
        /// <value>The description.</value>
        /// <inheritdoc cref="ICron" />
        [JsonIgnore]
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
        [JsonIgnore]
        public ISection Hours { get; private set; } = new TimeSection<int>(CronTimeSections.Hours);

        /// <summary>
        /// Minutes Information
        /// </summary>
        /// <value>The minutes.</value>
        /// <inheritdoc cref="ICron" />
        [JsonIgnore]
        public ISection Minutes { get; private set; } = new TimeSection<int>(CronTimeSections.Minutes);

        /// <summary>
        /// Months Information
        /// </summary>
        /// <value>The months.</value>
        /// <inheritdoc cref="ICron" />
        [JsonIgnore]
        public ISection Months { get; private set; } = new DateSection<CronMonths>(CronTimeSections.Months);

        /// <summary>
        /// Seconds Information
        /// </summary>
        /// <value>The seconds.</value>
        /// <inheritdoc cref="ICron" />
        [JsonIgnore]
        public ISection Seconds { get; private set; } = new TimeSection<int>(CronTimeSections.Seconds);

        /// <summary>
        /// Get a list of sections, originally sorted.
        /// </summary>
        /// <value>The section list.</value>
        [JsonIgnore]
        public List<Tuple<PropertyInfo, ISection>> SectionList => new SortedList<int, Tuple<PropertyInfo, ISection>>(
                typeof(CronBuilder)
                    .GetRuntimeProperties()
                    .Where(x => x.PropertyType ==
                                typeof(ISection))
                    .Select(x => new Tuple<PropertyInfo, ISection>(x, (ISection)x.GetValue(this)))
                    .ToList()
                    .ToDictionary(s => (int)s.Item2.Time)).Values
                                                          .ToList();

        /// <summary>
        /// Cron Expression.
        /// </summary>
        /// <value>The value.</value>
        /// <inheritdoc cref="ICron" />
        [JsonInclude]
        public string Value
        {
            get =>
                AllowSeconds
                    ? $"{Get(CronTimeSections.Seconds)} {Get(CronTimeSections.Minutes)} {Get(CronTimeSections.Hours)} {Get(CronTimeSections.DayMonth)} {Get(CronTimeSections.Months)} {Get(CronTimeSections.DayWeek)}"
                    : $"{Get(CronTimeSections.Minutes)} {Get(CronTimeSections.Hours)} {Get(CronTimeSections.DayMonth)} {Get(CronTimeSections.Months)} {Get(CronTimeSections.DayWeek)}"
            ;
            set => BuildExpression(value);
        }

        private void BuildExpression(string expression)
        {
            var cronArray = expression.Split(' ');

            if (cronArray.Length < 5 ||
                cronArray.Length > 6)
            {
                throw new InvalidDataException(
                    $"This expression only has {cronArray.Length} parts. An expression must have 5, or 6 parts.");
            }

            var startingIndex = 0;
            var mi = typeof(CronBuilder)
                     .GetRuntimeMethods()
                     .First(x => x.Name.Equals("CreateSection"));

            SectionList
                .ToList()
                .ForEach(section =>
                         {
                             var (prop, sectionData) = section;
                             var fooRef = mi.MakeGenericMethod(sectionData.GetType().GetGenericArguments().First());
                             var incrementVar =
                                 (cronArray.Length == 6 && AllowSeconds ||
                                  sectionData.Time != CronTimeSections.Seconds) &&
                                 startingIndex < cronArray.Length;

                             var result = (ISection)fooRef.Invoke(null,
                                                                  new object[]
                                                                  {
                                                                      sectionData, cronArray, startingIndex,
                                                                  });

                             if (incrementVar)
                             {
                                 startingIndex++;
                             }

                             prop.SetValue(this, result);
                         });
        }

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
            var section = GetProperty(time);

            if (repeatEvery && section.SectionType != CronSectionType.Time)
            {
                throw new ArgumentOutOfRangeException(nameof(repeatEvery));
            }

            section.Add(value)
                   .Every = repeatEvery && section.SectionType == CronSectionType.Time;

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
        public ICron Add(CronDays value) => Add(CronTimeSections.DayWeek, (int)value);

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
        public ICron Add(CronMonths value) => Add(CronTimeSections.Months, (int)value);

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
        public IEnumerator<CronTimeSections> GetEnumerator() =>
            new SortedList<int, CronTimeSections>(Enum.GetValues(typeof(CronTimeSections))
                                                      .Cast<CronTimeSections>()
                                                      .ToList()
                                                      .ToDictionary(s => (int)s)).Values.ToList()
                .GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Remove <see cref="CronTimeSections" /> with a specified value.
        /// </summary>
        /// <param name="time">The type of time section such as seconds, minutes, etc. See <see cref="CronTimeSections" />.</param>
        /// <param name="value">Value for the specified time section.</param>
        /// <returns>ICron.</returns>
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
        /// <returns>ICron.</returns>
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
        /// <param name="section"><see cref="ISection" /></param>
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
            SectionList.ForEach(section => section.Item2.Clear());

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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <inheritdoc cref="ICron" />
        public override string ToString() => Value;

        private static bool AddDescription(ISection section, ICollection<string> desc)
        {
            if (!section.Enabled ||
                string.IsNullOrEmpty(section.Description))
            {
                return false;
            }

            desc.Add(section.Description);

            return true;
        }

        private static ISection CreateDisabledSection<T>(ISection section) =>
            section.SectionType == CronSectionType.Time
                ? (ISection)new TimeSection<T>(section.Time, false)
                : new DateSection<T>(section.Time, false);

        private static ISection CreateExpressionSection<T>(ISection section, string value) =>
            section.SectionType == CronSectionType.Time
                ? (ISection)
                new TimeSection<T>(section.Time,
                                   value)
                : new DateSection<T>(section.Time,
                                     value);

        private static ISection
#pragma warning disable IDE0051 // Remove unused private members

            // ReSharper disable once UnusedMember.Local
            CreateSection<T>(ISection section, IReadOnlyList<string> cronArray, int startingIndex) =>
#pragma warning restore IDE0051 // Remove unused private members
            (cronArray.Count >= 6 || section.Time != CronTimeSections.Seconds) &&
            startingIndex < cronArray.Count
                ? CreateExpressionSection<T>(section, cronArray[startingIndex])
                : CreateDisabledSection<T>(section);

        private static string GetSpecificTime(ISection seconds, ISection minutes, ISection hours)
        {
            var time = new DateTime().AddHours(hours.ToInt())
                                     .AddMinutes(minutes.ToInt())
                                     .AddSeconds(seconds.ToInt());

            var formattedTime = seconds.ToInt() > 0
                ? time.ToString(SECOND_FORMAT)
                : time.ToString(MINUTE_FORMAT);

            var formattedEndTime = new DateTime().AddHours(hours.ToInt())
                                                 .AddMinutes(59)
                                                 .AddSeconds(59)
                                                 .ToString(MINUTE_FORMAT);

            return !seconds.Enabled && !minutes.Enabled
                ? $"{EVERY_MINUTE}, {formattedTime}-{formattedEndTime}"
                : $"At {formattedTime}";
        }

        private static string GetVariableTime(ISection seconds, ISection minutes, ISection hours)
        {
            var desc = new List<string>();

            AddDescription(seconds, desc);

            if (!AddDescription(minutes, desc) &&
                desc.Count == 0)
            {
                desc.Add(EVERY_MINUTE);
            }

            AddDescription(hours, desc);

            return string.Join(", ", desc);
        }

        private static bool IsTimeSpecific(ISection section) =>
            !(section.Enabled && (section.Every || section.ContainsRange));

        private string Get(CronTimeSections time) => GetProperty(time).ToString();

        private string GetDate()
        {
            var desc = new List<string>();

            SectionList.Where(section => section.Item2.SectionType == CronSectionType.Date)
                       .ToList()
                       .ForEach(section => AddDescription(section.Item2, desc));

            return string.Join(", ", desc);
        }

        private PropertyInfo GetFieldInfo(CronTimeSections time) =>
            GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == time.ToString());

        private ISection GetProperty(CronTimeSections time) => GetFieldInfo(time).GetValue(this) as ISection;

        private string GetTime()
        {
            var seconds = AllowSeconds
                ? GetProperty(CronTimeSections.Seconds)
                : CreateDisabledSection<ISection>(Seconds);
            var minutes = GetProperty(CronTimeSections.Minutes);
            var hours = GetProperty(CronTimeSections.Hours);

            if (seconds.Enabled ||
                minutes.Enabled ||
                hours.Enabled)
            {
                return !IsTimeSpecific(seconds) ||
                       !IsTimeSpecific(minutes) ||
                       !IsTimeSpecific(hours)
                    ? GetVariableTime(seconds, minutes, hours)
                    : GetSpecificTime(seconds, minutes, hours);
            }

            var text = seconds.Enabled || minutes.Enabled ? "second" : "minute";

            return $"Every {text}";
        }

        #endregion Methods
    }

    /// <inheritdoc />
    public class CronConverter : JsonConverter<CronBuilder>
    {
        /// <inheritdoc />
        public override CronBuilder
            Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token");
            }

            var message = new CronBuilder(true);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return message;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected PropertyName token");
                }

                var propName = reader.GetString();
                reader.Read();

                switch (propName)
                {
                    case nameof(CronBuilder.AllowSeconds):
                        message.AllowSeconds = reader.GetBoolean();

                        break;
                    case nameof(CronBuilder.Value):
                        message.Value = reader.GetString();

                        break;
                }
            }

            throw new JsonException("Expected EndObject token");
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, CronBuilder value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(CronBuilder.Value), value.Value);
            writer.WriteBoolean(nameof(CronBuilder.AllowSeconds), value.AllowSeconds);
            writer.WriteEndObject();
        }
    }
}
