using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Cron.Core.Interfaces;
using Cron.Core.Enums;

namespace Cron.Tests
{
    [TestClass]
    public class CronTests
    {
        private ICron schedule;

        [TestInitialize]
        public void Init()
        {
            schedule = new Core.Cron();
        }

        [TestMethod]
        [DataRow(
            3,
            1,
            1,
            "*/3 */3,4 3-5,7-11 1 3 3,5 */1",
            "Every 3 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 1 of the month, only on Wednesday,Friday, only in March"
        )]
        [DataRow(
            4,
            4,
            2,
            "*/4 */3,4 3-5,7-11 4 3 3,5 */2",
            "Every 4 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 4 of the month, only on Wednesday,Friday, only in March, every 2 years"
        )]
        [DataRow(
            50,
            7,
            3,
            "*/50 */3,4 3-5,7-11 7 3 3,5 */3",
            "Every 50 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 7 of the month, only on Wednesday,Friday, only in March, every 3 years"
        )]
        [DataRow(
            13,
            9,
            4,
            "*/13 */3,4 3-5,7-11 9 3 3,5 */4",
            "Every 13 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 9 of the month, only on Wednesday,Friday, only in March, every 4 years"
        )]
        [DataRow(
            22,
            12,
            5,
            "*/22 */3,4 3-5,7-11 12 3 3,5 */5",
            "Every 22 seconds, every 3,4 minutes, 03:00 AM-05:59 AM,07:00 AM-11:59 AM, only on day 12 of the month, only on Wednesday,Friday, only in March, every 5 years"
        )]
        public void Cron_CanVerifyComplex(int seconds, int dayMonth, int years, string expectedValue, string expectedDescription)
        {
            schedule = new Core.Cron();
            schedule
                .Add(time: CronTimeSections.Seconds, value: seconds, repeatEvery: true)
                .Add(CronTimeSections.Minutes, 4)
                .Add(CronTimeSections.Minutes, 3, true)
                .Add(CronTimeSections.Hours, 3, 5)
                .Add(CronTimeSections.Hours, 7, 11)
                .Add(CronMonths.March)
                .Add(CronDays.Wednesday)
                .Add(CronDays.Friday)
                .Add(CronTimeSections.DayMonth, dayMonth)
                .Add(CronTimeSections.Years, years, true);

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

            schedule.Years.Select(x => x.MinValue)
                .First()
                .Should()
                .Be(years);

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

        [TestMethod]
        public void Cron_YearOutOfRange()
        {
            Action act = () => schedule.Add(CronTimeSections.Years, 0);

            act.Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Cron_CanThrowSecondsOutOfRange()
        {
            Action act = () => schedule.Add(CronTimeSections.Seconds, 60);

            act.Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Cron_CanThrowMonthsOutOfRange()
        {
            Action act = () => schedule.Months.Add(13);

            act.Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [DataRow("2 3 4 5 6", "* 2 3 4 5 6 *", "At 03:02 AM, only on day 4 of the month, only on Saturday, only in May")]
        [DataRow("0 0 0 0", null, null)]
        [DataRow("0", null, null)]
        [DataRow("1 2", null, null)]
        [DataRow("* * 5-5 * *", "* * * 5 * * *", "Every minute, only on day 5 of the month")]
        [DataRow("* * 3-5 5-7 * */2", "* * 3-5 5-7 * */2 *", "Every minute, 03:00 AM-05:59 AM, only on day 5-7 of the month, every 2 day of the week")]
        [DataRow("* * 5 * * */2", "* * 5 * * */2 *", "Every minute, 05:00 AM-05:59 AM, every 2 day of the week")]
        [DataRow("* * 5 * * 2", "* * 5 * * 2 *", "Every minute, 05:00 AM-05:59 AM, only on Tuesday")]
        [DataRow("* * * 5 * * */2", "* * * 5 * * */2", "Every minute, only on day 5 of the month, every 2 years")]
        [DataRow("1-2 * * 5 * * */2", "1-2 * * 5 * * */2", "Only at 1-2 seconds past the minute, only on day 5 of the month, every 2 years")]
        [DataRow("1-2 * * 5-7 * * */2-3", "1-2 * * 5-7 * * */2-3", "Only at 1-2 seconds past the minute, only on day 5-7 of the month, every 2-3 years")]
        [DataRow("* * 5 * *", "* * * 5 * * *", "Every minute, only on day 5 of the month")]
        public void Cron_CreateByExpressionDescriptionMatches(string expression, string matchExpression, string description)
        {
            Core.Cron act1() => new Core.Cron(expression);

            if (matchExpression != null)
            {
                var cron = act1();

                cron.ToString()
                    .Should()
                    .Be(matchExpression);
            }

            if (!string.IsNullOrEmpty(description))
            {
                act1().Description.Should()
                    .Be(description);
            }
            else
            {
                Action act = () => _ = new Core.Cron(expression).Description;

                act
                    .Should()
                    .Throw<InvalidDataException>(
                        because: $"This expression only has {0} parts. An expression must have 5, 6, or 7 parts.",
                        becauseArgs: expression.Split(' ')
                            .Length
                    );
            }
        }

        [TestMethod]
        public void Cron_CanRemoveSeconds()
        {
            var cron = new Core.Cron
            {
                { CronTimeSections.Seconds, 5 },
                { CronTimeSections.Seconds, 9 },
                { CronTimeSections.Seconds, 7 }
            };

            cron.Value.Should()
                .Be("5,7,9 * * * * * *");

            cron.Remove(CronTimeSections.Seconds, 5);

            cron.Value.Should()
                .Be("7,9 * * * * * *");
        }

        [TestMethod]
        public void Cron_CanRemoveByRange()
        {
            var cron = new Core.Cron
            {
                { CronTimeSections.Seconds, 5, 6 },
                { CronTimeSections.Seconds, 9 },
                { CronTimeSections.Seconds, 7 }
            };

            cron.Value.Should()
                .Be("5-6,7,9 * * * * * *");

            cron.Remove(CronTimeSections.Seconds, 5, 6);

            cron.Value.Should()
                .Be("7,9 * * * * * *");
        }

        [TestMethod]
        public void Cron_CanAddByDayWeekRange()
        {
            var cron = new Core.Cron
            {
                { CronDays.Thursday, CronDays.Saturday }
            };
            cron.DayWeek.Values.Any(x => x == "4-6")
                .Should()
                .BeTrue();
        }

        [TestMethod]
        public void Cron_CanAddByCronMonthRange()
        {
            var cron = new Core.Cron
            {
                { CronMonths.August, CronMonths.November }
            };
            cron.Months.Values.Any(x => x == "8-11")
                .Should()
                .BeTrue();
        }

        [TestMethod]
        public void Cron_CanResetDayWeek()
        {
            var cron = new Core.Cron();
            var val = cron.Value;

            cron.Add(CronDays.Friday);
            val.Should()
                .NotBe(cron.Value);

            cron.Reset(CronTimeSections.DayWeek);

            val.Should()
                .Be(cron.Value);
        }

        [TestMethod]
        public void Cron_CanResetAll()
        {
            var cron = new Core.Cron();
            var val = cron.Value;

            cron.Add(CronDays.Friday);
            cron.Add(CronMonths.August);
            cron.Add(CronTimeSections.Seconds, 44);
            cron.Add(CronTimeSections.Minutes, 4, 8);
            cron.Add(CronDays.Monday);

            val.Should()
                .NotBe(cron.Value);

            cron.ResetAll();

            val.Should()
                .Be(cron.Value);
        }

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

        [TestMethod]
        public void Cron_DateEveryDescriptionMatches()
        {
            var cron = new Core.Cron();
            cron.Months.Add(1)
                .Every = true;
            cron.DayWeek.Add(5)
                .Every = true;
            cron.Years.Add(2).Every = true;

            cron.Description.Should()
                .Be("Every minute, every 5 day of the week, every 2 years");
        }

        [TestMethod]
        public void Cron_TimeEveryDescription1IsDescription2()
        {
            var cron = new Core.Cron();

            cron.Seconds.Add(5)
                .Every = true;
            cron.Minutes.Add(44)
                .Every = true;
            cron.Hours.Add(3)
                .Every = true;

            ValidateCronDesc(cron);
        }

        [TestMethod]
        public void Cron_TimeAtDescriptionMatches()
        {
            var cron = new Core.Cron();

            cron.Seconds.Add(5);
            cron.Minutes.Add(44);
            cron.Hours.Add(3);

            ValidateCronDesc(cron);
        }

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

        private static void ValidateCronDesc(Core.Cron cron)
        {
            cron.Description.Trim().Should()
                .Be(cron.Description.Trim());
        }
    }
}
