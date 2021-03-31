// ***********************************************************************
// Assembly         : Cron.Tests
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 11-17-2020
// ***********************************************************************
// <copyright file="CronTests.cs" company="Cron.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Linq;
using Cron.Core;
using Cron.Core.Enums;
using Cron.Core.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Tests
{
    /// <summary>
    /// Defines test class CronTests.
    /// </summary>
    [TestClass]
    public class CronTests
    {
        private ICron schedule;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init() => schedule = new CronBuilder();

        /// <summary>
        /// Defines the test method Cron_CanVerifyComplex.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="dayMonth">The day month.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="expectedDescription">The expected description.</param>
        [TestMethod]
        [DataRow(3,
                 1,
                 "*/3 */3,4 3-5,7-11 1 3 3,5",
                 "Every 3 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 1 of the month, only in March, only on Wednesday,Friday")]
        [DataRow(4,
                 4,
                 "*/4 */3,4 3-5,7-11 4 3 3,5",
                 "Every 4 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 4 of the month, only in March, only on Wednesday,Friday")]
        [DataRow(30,
                 7,
                 "*/30 */3,4 3-5,7-11 7 3 3,5",
                 "Every 30 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 7 of the month, only in March, only on Wednesday,Friday")]
        [DataRow(20,
                 9,
                 "*/20 */3,4 3-5,7-11 9 3 3,5",
                 "Every 20 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 9 of the month, only in March, only on Wednesday,Friday")]
        [DataRow(20,
                 12,
                 "*/20 */3,4 3-5,7-11 12 3 3,5",
                 "Every 20 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 12 of the month, only in March, only on Wednesday,Friday")]
        public void Cron_CanVerifyComplex(int seconds,
                                          int dayMonth,
                                          string expectedValue,
                                          string expectedDescription)
        {
            schedule = new CronBuilder(true)
                       .Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)
                       .Add(CronTimeSections.Minutes, 4)
                       .Add(CronTimeSections.Minutes, 3, true)
                       .Add(CronTimeSections.Hours, 3, 5)
                       .Add(CronTimeSections.Hours, 7, 11)
                       .Add(CronMonths.March)
                       .Add(CronDays.Wednesday)
                       .Add(CronDays.Friday)
                       .Add(CronTimeSections.DayMonth, dayMonth);

            schedule.Seconds.Select(x => x.MinValue)
                    .First()
                    .Should()
                    .Be(seconds);

            schedule.Minutes.Select(x => x.MinValue)
                    .ToList()
                    .Should()
                    .NotBeEmpty()
                    .And.HaveCount(2)
                    .And.Contain(3)
                    .And.Contain(4);

            schedule.Hours
                    .ToList()
                    .Should()
                    .NotBeEmpty()
                    .And.HaveCount(2)
                    .And.Contain(x => x.MinValue == 3 && x.MaxValue == 5)
                    .And.Contain(x => x.MinValue == 7 && x.MaxValue == 11);

            schedule.Months.Select(x => x.MinValue)
                    .First()
                    .Should()
                    .Be(3);

            schedule.DayWeek.Select(x => x.MinValue)
                    .Should()
                    .NotBeEmpty()
                    .And.HaveCount(2)
                    .And.Contain(3)
                    .And.Contain(5);

            schedule.DayMonth.Select(x => x.MinValue)
                    .First()
                    .Should()
                    .Be(dayMonth);

            schedule.Value
                    .Should()
                    .Be(expectedValue);

            schedule.Description
                    .Should()
                    .Be(expectedDescription);

            schedule.Seconds.Every.Should()
                    .BeTrue();
            schedule.Minutes.Every.Should()
                    .BeTrue();
            schedule.Hours.Every.Should()
                    .BeFalse();

            schedule.Seconds.ToString()
                    .Should()
                    .Be($"*/{seconds}");

            schedule.DayMonth.ToString()
                    .Should()
                    .Be($"{dayMonth}");
        }

        /// <summary>
        /// Defines the test method Cron_CanVerifyDefault.
        /// </summary>
        [TestMethod]
        public void Cron_CanVerifyDefault()
        {
            schedule.ToString()
                    .Should()
                    .Be("* * * * *");

            schedule.Description
                    .Should()
                    .Be("Every minute");
        }

        /// <summary>
        /// Defines the test method Cron_CanThrowSecondsOutOfRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanThrowSecondsOutOfRange()
        {
            Action act = () => schedule.Add(CronTimeSections.Seconds, 60);

            act.Should()
               .Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Defines the test method Cron_CanThrowMonthsOutOfRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanThrowMonthsOutOfRange()
        {
            Action act = () => schedule.Months.Add(13);

            act.Should()
               .Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Defines the test method Cron_CreateByExpressionDescriptionMatches.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="matchExpression">The match expression.</param>
        /// <param name="description">The description.</param>
        [TestMethod]
        [DataRow("2 3 4 5 6",
                 "* 2 3 4 5 6",
                 "At 03:02 AM, only on day 4 of the month, only in May, only on Saturday")]
        [DataRow("0 0 0 0", null, null)]
        [DataRow("0", null, null)]
        [DataRow("1 2", null, null)]
        [DataRow("* * 5-5 * *", "* * * 5 * *", "Every minute, only on day 5 of the month")]
        [DataRow("* * 3-5 5-7 * 2",
                 "* * 3-5 5-7 * 2",
                 "Every minute, 03:00 AM-05:59 AM, only on day 5-7 of the month, only on Tuesday")]
        [DataRow("* * 5 * * 2", "* * 5 * * 2", "Every minute, 05:00 AM-05:59 AM, only on Tuesday")]
        [DataRow("* * * 5 * *", "* * * 5 * *", "Every minute, only on day 5 of the month")]
        [DataRow("1-2 * * 5 * *",
                 "1-2 * * 5 * *",
                 "Only at 1-2 seconds past the minute, only on day 5 of the month")]
        [DataRow("1-2 * * 5-7 * *",
                 "1-2 * * 5-7 * *",
                 "Only at 1-2 seconds past the minute, only on day 5-7 of the month")]
        [DataRow("* * 5 * *", "* * * 5 * *", "Every minute, only on day 5 of the month")]
        public void Cron_CreateByExpressionDescriptionMatches(string expression, string matchExpression,
                                                              string description)
        {
            CronBuilder Act1() => new CronBuilder(expression, true);

            if (matchExpression != null)
            {
                var cron = Act1();

                cron.ToString()
                    .Should()
                    .Be(matchExpression);
            }

            if (!string.IsNullOrEmpty(description))
            {
                Act1()
                    .Description.Should()
                    .Be(description);
            }
            else
            {
                Action act = () => _ = new CronBuilder(expression).Description;

                act
                    .Should()
                    .Throw<InvalidDataException>(
                        "This expression only has {0} parts. An expression must have 5, 6, or 7 parts.",
                        expression.Split(' ')
                                  .Length);
            }
        }

        /// <summary>
        /// Defines the test method Cron_CanRemoveSeconds.
        /// </summary>
        [TestMethod]
        public void Cron_CanRemoveSecondsMinutes()
        {
            var cron = new CronBuilder(true)
            {
                { CronTimeSections.Seconds, 5 }, { CronTimeSections.Seconds, 9 }, { CronTimeSections.Seconds, 7 },
            };

            cron.Value.Should()
                .Be("5,7,9 * * * * *");

            cron.Remove(CronTimeSections.Seconds, 5)
                .Value.Should()
                .Be("7,9 * * * * *");

            cron.Minutes.Add(5)
                .ToString()
                .Should()
                .Be("5");

            cron.Minutes.Remove(5)
                .ToString()
                .Should()
                .Be("*");
        }

        /// <summary>
        /// Defines the test method Cron_CanRemoveByRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanRemoveByRange()
        {
            var cron = new CronBuilder(true)
            {
                { CronTimeSections.Seconds, 5, 6 },
                { CronTimeSections.Seconds, 9 },
                { CronTimeSections.Seconds, 7 },
            };

            cron.Value.Should()
                .Be("5-6,7,9 * * * * *");

            cron.Seconds.ToString()
                .Should()
                .Be("5-6,7,9");

            cron.Seconds.Remove(5, 6)
                .ToString()
                .Should()
                .Be("7,9");

            cron.Value.Should()
                .Be("7,9 * * * * *");

            cron.Minutes.Add(5, 6)
                .ToString()
                .Should()
                .Be("5-6");
            cron.Remove(CronTimeSections.Minutes, 5, 6)
                .ToString()
                .Should()
                .Be("7,9 * * * * *");
        }

        /// <summary>
        /// Defines the test method Cron_CanAddByDayWeekRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanAddByDayWeekRangeNoSeconds()
        {
            var cron = new CronBuilder();
            cron.Add(CronDays.Thursday, CronDays.Saturday)
                .Value
                .Should()
                .Be("* * * * 4-6");

            cron.DayWeek.Values.Any(x => x == "4-6")
                .Should()
                .BeTrue();
        }

        /// <summary>
        /// Defines the test method Cron_CanAddByDayWeekRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanAddByDayWeekRangeWithSeconds()
        {
            var cron = new CronBuilder(true);
            cron.Add(CronDays.Thursday, CronDays.Saturday)
                .Value
                .Should()
                .Be("* * * * * 4-6");

            cron.DayWeek.Values.Any(x => x == "4-6")
                .Should()
                .BeTrue();
        }

        /// <summary>
        /// Defines the test method Cron_CanAddByCronMonthRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanAddByCronMonthRangeNoSeconds()
        {
            var cron = new CronBuilder();
            cron.Add(CronMonths.August, CronMonths.November)
                .Value.Should()
                .Be("* * * 8-11 *");

            cron.Months.Values.Any(x => x == "8-11")
                .Should()
                .BeTrue();
        }

        [TestMethod]
        public void Cron_CanAddByCronMonthRangeWithSeconds()
        {
            var cron = new CronBuilder(true);
            cron.Add(CronMonths.August, CronMonths.November)
                .Value.Should()
                .Be("* * * * 8-11 *");

            cron.Months.Values.Any(x => x == "8-11")
                .Should()
                .BeTrue();
        }

        /// <summary>
        /// Defines the test method Cron_EveryTimeTestsWithResets.
        /// </summary>
        [TestMethod]
        public void Cron_EveryTimeTestsWithResets()
        {
            var cron = new CronBuilder();
            TestTimeSection(cron, cron.Seconds);

            cron = new CronBuilder();
            TestTimeSection(cron, cron.Minutes);

            cron = new CronBuilder();
            TestTimeSection(cron, cron.Hours);
        }

        /// <summary>
        /// Defines the test method Cron_EveryTimeTestsNoResets.
        /// </summary>
        [TestMethod]
        public void Cron_EveryTimeTestsNoResets()
        {
            var cron = new CronBuilder();
            TestTimeSection(cron, cron.Seconds);
            TestTimeSection(cron, cron.Minutes);
            TestTimeSection(cron, cron.Hours);
        }

        private static void TestTimeSection(ICron cron, ISection section)
        {
            section.Add(1)
                   .Every.Should()
                   .BeFalse();
            cron.Add(section.Time, 2);
            section.Every.Should()
                   .BeFalse();
            cron.Add(section.Time, 3, true);
            section.Every.Should()
                   .BeTrue();
        }

        /// <summary>
        /// Defines the test method Cron_EveryDateTests.
        /// </summary>
        [TestMethod]
        public void Cron_EveryDateTests()
        {
            var cron = new CronBuilder();
            cron.SectionList.Where(x => x.Item2.SectionType == CronSectionType.Date)
                .ToList()
                .ForEach(x => TestDateSection(cron, x.Item2));
        }

        private static void TestDateSection(ICron cron, ISection section)
        {
            section.Add(1)
                   .Every.Should()
                   .BeFalse();
            cron.Add(section.Time, 2);
            section.Every.Should()
                   .BeFalse();
            Action act = () => cron.Add(section.Time, 3, true);
            act.Should()
               .Throw<Exception>();
        }

        /// <summary>
        /// Defines the test method Cron_CanResetDayWeek.
        /// </summary>
        [TestMethod]
        public void Cron_CanResetDayWeek()
        {
            var cron = new CronBuilder();
            var val = cron.Value;

            cron.Add(CronDays.Friday)
                .Value.Should()
                .NotBe(val);

            cron.Reset(CronTimeSections.DayWeek)
                .Value.Should()
                .Be(val);
        }

        /// <summary>
        /// Defines the test method Cron_CanResetDayWeek.
        /// </summary>
        [TestMethod]
        public void Cron_CanResetMinutes()
        {
            var cron = new CronBuilder();
            var val = cron.Value;

            cron.Add(CronTimeSections.Minutes, 4)
                .Value.Should()
                .NotBe(val);

            cron.Reset(cron.Minutes).Value.Should().Be(val);
        }

        /// <summary>
        /// Defines the test method Cron_CanClearMinutes.
        /// </summary>
        [TestMethod]
        public void Cron_CanClearMinutes()
        {
            var cron = new CronBuilder();
            var val = cron.Minutes.ToString();

            cron
                .Add(CronTimeSections.Minutes, 4)
                .Minutes.ToString()
                .Should()
                .NotBe(val);

            cron.Minutes.Clear()
                .ToString()
                .Should()
                .Be(val);
        }

        /// <summary>
        /// Defines the test method Cron_CanResetAll.
        /// </summary>
        [TestMethod]
        public void Cron_CanResetAll()
        {
            var cron = new CronBuilder();
            var val = cron.Value;

            cron.Add(CronDays.Friday)
                .Add(CronMonths.August)
                .Add(CronTimeSections.Seconds, 44)
                .Add(CronTimeSections.Minutes, 4, 8)
                .Add(CronDays.Monday)
                .Value.Should()
                .NotBe(val);

            cron.Reset()
                .Value.Should()
                .Be(val);
        }

        /// <summary>
        /// Defines the test method Cron_DateAtDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_DateAtDescriptionMatches()
        {
            var cron = new CronBuilder { CronMonths.January, CronDays.Friday, };
            cron.Minutes.Add(20);

            cron.Minutes.Description.Should()
                .Be("only at 20 minutes past the hour");

            cron.Minutes.Add(22)
                .Add(24, 26);

            cron.Minutes.Description.Should()
                .Be("only at 20,22,24-26 minutes past the hour");

            cron.Description.Should()
                .Be("only at 20,22,24-26 minutes past the hour, only in January, only on Friday");

            cron.Hours.Add(4);
            cron.Hours.Description.Should().Be("only at 4 hours");

            cron.Description.Should()
                .Be("only at 20,22,24-26 minutes past the hour, only at 4 hours, only in January, only on Friday");
        }

        /// <summary>
        /// Defines the test method Cron_DateEveryDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_DateEveryDescriptionMatches()
        {
            var cron = new CronBuilder();
            cron.Months.Add(1);
            cron.DayWeek.Add(5);
            cron.Minutes.Add(20);

            cron.Description.Should()
                .Be("At 12:20 AM, only in January, only on Friday");
        }

        /// <summary>
        /// Defines the test method Cron_TimeEveryDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_TimeEveryDescriptionMatches()
        {
            var cron = new CronBuilder(true);

            cron.Seconds.Add(5)
                .Every = true;
            cron.Minutes.Add(44)
                .Every = true;
            cron.Hours.Add(3)
                .Every = true;
            cron.Description.Should()
                .Be("Every 5 seconds, every 44 minutes, every 3 hours");
            cron.AllowSeconds = false;
            cron.Description.Should()
                .Be("every 44 minutes, every 3 hours");
        }

        /// <summary>
        /// Defines the test method Cron_TimeAtDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_TimeAtDescriptionMatches()
        {
            var cron = new CronBuilder(true);

            cron.Seconds.Add(5);
            cron.Minutes.Add(44);
            cron.Hours.Add(3);

            cron.Description.Should()
                .Be("At 03:44:05 AM");

            cron.AllowSeconds = false;
            cron.Description.Should()
                .Be("At 03:44 AM");
        }

        /// <summary>
        /// Defines the test method Cron_DescriptionTimeMatches.
        /// </summary>
        [TestMethod]
        public void Cron_DescriptionTimeMatches()
        {
            var cron = new CronBuilder(true);
            cron.Seconds.Add(4)
                .Every = true;
            cron.Seconds.Description.Should()
                .Be("Every 4 seconds");

            cron.Description.Should()
                .Be("Every 4 seconds");

            cron.Minutes.Add(5)
                .Every = true;

            cron.Minutes.Description.Should()
                .Be("every 5 minutes");

            cron.Description.Should()
                .Be("Every 4 seconds, every 5 minutes");

            cron.Hours.Add(3)
                .Every = true;

            cron.Hours.Description.Should()
                .Be("every 3 hours");

            cron.Description.Should()
                .Be("Every 4 seconds, every 5 minutes, every 3 hours");

            cron.AllowSeconds = false;

            cron.Description.Should()
                .Be("every 5 minutes, every 3 hours");
        }

        /// <summary>
        /// Defines the test method Ranges_Valid.
        /// </summary>
        [TestMethod]
        public void Ranges_Valid()
        {
            var cron = new CronBuilder { Minutes = { Every = false, }, };
            cron.Minutes.IsValidRange(44)
                .Should()
                .BeTrue();
            cron.Minutes.Every = true;
            cron.Minutes.IsValidRange(44)
                .Should()
                .BeFalse();

            cron.Months.IsValidRange(1)
                .Should()
                .BeTrue();

            cron.Months.IsValidRange(12)
                .Should()
                .BeTrue();

            cron.Months.IsValidRange(13)
                .Should()
                .BeFalse();
        }

        /// <summary>
        /// Defines the test method Int_IsInt.
        /// </summary>
        [TestMethod]
        public void Int_IsInt()
        {
            var minutes = new CronBuilder()
                          .Minutes
                          .Add(5);
            minutes.IsInt()
                   .Should()
                   .BeTrue();
            minutes.Every = true;
            minutes.IsInt()
                   .Should()
                   .BeFalse();
        }
    }
}
