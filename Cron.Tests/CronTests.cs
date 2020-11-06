using System;
using System.Collections.Generic;
using System.Linq;
using Cron.Enums;
using Cron.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Tests
{
    [TestClass]
    public class CronTests
    {
        private Cron schedule;

        [TestInitialize]
        public void Init()
        {
            schedule = new Cron();
        }

        [TestMethod]
        [DataRow(
            3,
            1,
            1,
            "*/3 */3,4 3-5,7-11 1 3,5 3 */1",
            "Every 3 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 1 of the month, only on Wednesday, only in March and May"
        )]
        [DataRow(
            4,
            4,
            2,
            "*/4 */3,4 3-5,7-11 4 3,5 3 */2",
            "Every 4 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 4 of the month, only on Wednesday, only in March and May, every 2 years"
        )]
        [DataRow(
            50,
            7,
            3,
            "*/50 */3,4 3-5,7-11 7 3,5 3 */3",
            "Every 50 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 7 of the month, only on Wednesday, only in March and May, every 3 years"
        )]
        [DataRow(
            13,
            9,
            4,
            "*/13 */3,4 3-5,7-11 9 3,5 3 */4",
            "Every 13 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 9 of the month, only on Wednesday, only in March and May, every 4 years"
        )]
        [DataRow(
            22,
            12,
            5,
            "*/22 */3,4 3-5,7-11 12 3,5 3 */5",
            "Every 22 seconds, every 3,4 minutes, at 03:00 AM through 05:59 AM and 07:00 AM through 11:59 AM, on day 12 of the month, only on Wednesday, only in March and May, every 5 years"
        )]
        public void Cron_CanVerifyComplex(int seconds, int dayMonth, int years, string expectedValue, string expectedDescription)
        {
            schedule = new Cron();
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
                .Be("Every second");
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
        [DataRow("* * * 5 * * */2", "Every second, on day 5 of the month, every 2 years")]
        [DataRow("1-2 * * 5 * * */2", "Seconds 1 through 2 past the minute, on day 5 of the month, every 2 years")]
        [DataRow("1-2 * * 5-7 * * */2-3", "Seconds 1 through 2 past the minute, between day 5 and 7 of the month, every 2-3 years")]
        [DataRow("* * 5 * * */2", "Every second, between 05:00 AM and 05:59 AM, every 2 days of the week")]
        [DataRow("* * 3-5 5-7 * */2", "Every second, between 03:00 AM and 05:59 AM, between day 5 and 7 of the month, every 2 days of the week")]
        [DataRow("* * 5-5 * *", "Every minute, on day 5 of the month")]
        [DataRow("* * 5 * *", "Every minute, on day 5 of the month")]
        [DataRow("2 3 4 5 6", "At 03:02 AM, on day 4 of the month, only on Saturday, only in May")]
        [DataRow("0 0 0 0", null)]
        [DataRow("0", null)]
        [DataRow("1 2", null)]
        public void Cron_CreateByExpression(string expression, string description)
        {
            var cron = new Cron(expression);

            if (!string.IsNullOrEmpty(description))
            {
                cron.Description.Should()
                    .Be(description);
            }
            else
            {
                Action act = () => _ = cron.Description;

                act
                    .Should()
                    .Throw<FormatException>(
                        because: "Error: Expression only has {0} parts. At least 5 part are required",
                        becauseArgs: expression.Split(' ')
                            .Length
                    );
            }
        }

        [TestMethod]
        public void Cron_CanRemoveSeconds()
        {
            var cron = new Cron
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
            var cron = new Cron
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
            var cron = new Cron
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
            var cron = new Cron
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
            var cron = new Cron();
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
            var cron = new Cron();
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
    }
}
