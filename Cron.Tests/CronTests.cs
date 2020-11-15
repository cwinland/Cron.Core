// ***********************************************************************
// Assembly         : Cron.Tests
// Author           : chris
// Created          : 11-05-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="CronTests.cs" company="Cron.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Cron.Core.Interfaces;
using Cron.Core.Enums;

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
        public void Init()
        {
            schedule = new Cron.Core.Cron();
        }

        /// <summary>
        /// Defines the test method Cron_CanVerifyComplex.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="dayMonth">The day month.</param>
        /// <param name="years">The years.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="expectedDescription">The expected description.</param>
        [TestMethod]
        [DataRow(
            3,
            1,
            0,
            "*/3 */3,4 3-5,7-11 1 3 3,5 *",
            "Every 3 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 1 of the month, only on Wednesday,Friday, only in March"
        )]
        [DataRow(
            4,
            4,
            0,
            "*/4 */3,4 3-5,7-11 4 3 3,5 *",
            "Every 4 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 4 of the month, only on Wednesday,Friday, only in March"
        )]
        [DataRow(
            30,
            7,
            0,
            "*/30 */3,4 3-5,7-11 7 3 3,5 *",
            "Every 30 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 7 of the month, only on Wednesday,Friday, only in March"
        )]
        [DataRow(
            20,
            9,
            2022,
            "*/20 */3,4 3-5,7-11 9 3 3,5 2022",
            "Every 20 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 9 of the month, only on Wednesday,Friday, only in March, only in year 2022"
        )]
        [DataRow(
            20,
            12,
            0,
            "*/20 */3,4 3-5,7-11 12 3 3,5 *",
            "Every 20 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 12 of the month, only on Wednesday,Friday, only in March"
        )]
        public void Cron_CanVerifyComplex(int seconds, int dayMonth, int years, string expectedValue, string expectedDescription)
        {
            schedule = new Core.Cron()
                .Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)
                .Add(CronTimeSections.Minutes, 4)
                .Add(CronTimeSections.Minutes, 3, true)
                .Add(CronTimeSections.Hours, 3, 5)
                .Add(CronTimeSections.Hours, 7, 11)
                .Add(CronMonths.March)
                .Add(CronDays.Wednesday)
                .Add(CronDays.Friday)
                .Add(CronTimeSections.DayMonth, dayMonth);

            if (years > 0)
            {
                schedule.Add(CronTimeSections.Years, years);
            }

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

            if (years > 0)
            {
                schedule.Years.Select(x => x.MinValue)
                        .First()
                        .Should()
                        .Be(years);
            }

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
                .Be("* * * * * * *");

            schedule.Description
                .Should()
                .Be("Every minute");
        }

        /// <summary>
        /// Defines the test method Cron_YearOutOfRange.
        /// </summary>
        [TestMethod]
        public void Cron_YearOutOfRange()
        {
            Action act = () => schedule.Add(CronTimeSections.Years, 0);

            act.Should()
                .Throw<ArgumentOutOfRangeException>();
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
        [DataRow("2 3 4 5 6", "* 2 3 4 5 6 *", "At 03:02 AM, only on day 4 of the month, only on Saturday, only in May")]
        [DataRow("0 0 0 0", null, null)]
        [DataRow("0", null, null)]
        [DataRow("1 2", null, null)]
        [DataRow("* * 5-5 * *", "* * * 5 * * *", "Every minute, only on day 5 of the month")]
        [DataRow("* * 3-5 5-7 * 2", "* * 3-5 5-7 * 2 *", "Every minute, 03:00 AM-05:59 AM, only on day 5-7 of the month, only on Tuesday")]
        [DataRow("* * 5 * * 2", "* * 5 * * 2 *", "Every minute, 05:00 AM-05:59 AM, only on Tuesday")]
        [DataRow("* * * 5 * * 2020", "* * * 5 * * 2020", "Every minute, only on day 5 of the month, only in year 2020")]
        [DataRow("1-2 * * 5 * * 2020", "1-2 * * 5 * * 2020", "Only at 1-2 seconds past the minute, only on day 5 of the month, only in year 2020")]
        [DataRow("1-2 * * 5-7 * * 2020,2021", "1-2 * * 5-7 * * 2020,2021", "Only at 1-2 seconds past the minute, only on day 5-7 of the month, only in year 2020,2021")]
        [DataRow("* * 5 * *", "* * * 5 * * *", "Every minute, only on day 5 of the month")]
        public void Cron_CreateByExpressionDescriptionMatches(string expression, string matchExpression, string description)
        {
            Core.Cron Act1() => new Core.Cron(expression);

            if (matchExpression != null)
            {
                var cron = Act1();

                cron.ToString()
                    .Should()
                    .Be(matchExpression);
            }

            if (!string.IsNullOrEmpty(description))
            {
                Act1().Description.Should()
                    .Be(description);
            }
            else
            {
                Action act = () => _ = new Core.Cron(expression).Description;

                act
                    .Should()
                    .Throw<InvalidDataException>(
                        because: "This expression only has {0} parts. An expression must have 5, 6, or 7 parts.",
                        becauseArgs: expression.Split(' ')
                            .Length
                    );
            }
        }

        /// <summary>
        /// Defines the test method Cron_CanRemoveSeconds.
        /// </summary>
        [TestMethod]
        public void Cron_CanRemoveSecondsMinutes()
        {
            var cron = new Core.Cron
            {
                { CronTimeSections.Seconds, 5 },
                { CronTimeSections.Seconds, 9 },
                { CronTimeSections.Seconds, 7 },
            };

            cron.Value.Should()
                .Be("5,7,9 * * * * * *");

            cron.Remove(CronTimeSections.Seconds, 5)
                .Value.Should()
                .Be("7,9 * * * * * *");

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
            var cron = new Core.Cron
            {
                { CronTimeSections.Seconds, 5, 6 },
                { CronTimeSections.Seconds, 9 },
                { CronTimeSections.Seconds, 7 },
            };

            cron.Value.Should()
                .Be("5-6,7,9 * * * * * *");

            cron.Seconds.ToString()
                .Should()
                .Be("5-6,7,9");

            cron.Seconds.Remove(5, 6)
                .ToString()
                .Should()
                .Be("7,9");

            cron.Value.Should()
                .Be("7,9 * * * * * *");

            cron.Minutes.Add(5, 6)
                .ToString()
                .Should()
                .Be("5-6");
            cron.Remove(CronTimeSections.Minutes, 5, 6)
                .ToString()
                .Should()
                .Be("7,9 * * * * * *");
        }

        /// <summary>
        /// Defines the test method Cron_CanAddByDayWeekRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanAddByDayWeekRange()
        {
            var cron = new Core.Cron();
            cron.Add(CronDays.Thursday, CronDays.Saturday)
                .Value
                .Should()
                .Be("* * * * * 4-6 *");

            cron.DayWeek.Values.Any(x => x == "4-6")
                .Should()
                .BeTrue();
        }

        /// <summary>
        /// Defines the test method Cron_CanAddByCronMonthRange.
        /// </summary>
        [TestMethod]
        public void Cron_CanAddByCronMonthRange()
        {
            var cron = new Core.Cron();
            cron.Add(CronMonths.August, CronMonths.November)
                .Value.Should()
                .Be("* * * * 8-11 * *");

            cron.Months.Values.Any(x => x == "8-11")
                .Should()
                .BeTrue();
        }

        [TestMethod]
        public void Cron_EveryTimeTestsWithResets()
        {
            var cron = new Core.Cron();
            TestTimeSection(cron, cron.Seconds);

            cron = new Core.Cron();
            TestTimeSection(cron, cron.Minutes);

            cron = new Core.Cron();
            TestTimeSection(cron, cron.Hours);
        }

        [TestMethod]
        public void Cron_EveryTimeTestsNoResets()
        {
            var cron = new Core.Cron();
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
            var cron = new Core.Cron();
            cron.SectionList.Where(x => x.SectionType == CronSectionType.Date)
                .ToList()
                .ForEach(x => TestDateSection(cron, x));
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
            var cron = new Core.Cron();
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
            var cron = new Core.Cron();
            var val = cron.Value;

            cron.Add(CronTimeSections.Minutes, 4)
                .Value.Should()
                .NotBe(val);

            cron.Reset(cron.Minutes).Value.Should().Be(val);
        }

        [TestMethod]
        public void Cron_CanClearMinutes()
        {
            var cron = new Core.Cron();
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
            var cron = new Core.Cron();
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
            var cron = new Core.Cron { CronMonths.January, CronDays.Friday };
            cron.Years.Add(2020);

            cron.Years.Description.Should()
                .Be("only in year 2020");

            cron.Years.Add(2022)
                .Add(2024, 2026);

            cron.Years.Description.Should()
                .Be("only in year 2020,2022,2024-2026");

            cron.Description.Should()
                .Be("Every minute, only on Friday, only in January, only in year 2020,2022,2024-2026");

            cron.Hours.Add(4);
            cron.Description.Should()
                .Be("Every minute, 04:00 AM-04:59 AM, only on Friday, only in January, only in year 2020,2022,2024-2026");
        }

        /// <summary>
        /// Defines the test method Cron_DateEveryDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_DateEveryDescriptionMatches()
        {
            var cron = new Core.Cron();
            cron.Months.Add(1);
            cron.DayWeek.Add(5);
            cron.Years.Add(2021);

            cron.Description.Should()
                .Be("Every minute, only on Friday, only in January, only in year 2021");
        }

        /// <summary>
        /// Defines the test method Cron_TimeEveryDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_TimeEveryDescriptionMatches()
        {
            var cron = new Core.Cron();

            cron.Seconds.Add(5)
                .Every = true;
            cron.Minutes.Add(44)
                .Every = true;
            cron.Hours.Add(3)
                .Every = true;
            cron.Description.Should()
                .Be("Every 5 seconds, every 44 minutes, every 3 hours");
        }

        /// <summary>
        /// Defines the test method Cron_TimeAtDescriptionMatches.
        /// </summary>
        [TestMethod]
        public void Cron_TimeAtDescriptionMatches()
        {
            var cron = new Core.Cron();

            cron.Seconds.Add(5);
            cron.Minutes.Add(44);
            cron.Hours.Add(3);

            cron.Description.Should()
                .Be("At 03:44:05 AM");
        }

        /// <summary>
        /// Defines the test method Cron_DescriptionTimeMatches.
        /// </summary>
        [TestMethod]
        public void Cron_DescriptionTimeMatches()
        {
            var cron = new Core.Cron();
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
                .Be(cron.Description);

            cron.Hours.Add(3)
                .Every = true;

            cron.Hours.Description.Should()
                .Be("every 3 hours");

            cron.Description.Should()
                .Be(cron.Description);
        }

        [TestMethod]
        public void Ranges_Valid()
        {
            var cron = new Core.Cron();
            cron.Minutes.Every = false;
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

        [TestMethod]
        public void Int_IsInt()
        {
            var minutes = new Core.Cron()
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
