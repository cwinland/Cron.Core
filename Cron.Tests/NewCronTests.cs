using Cron.Core;
using Cron.Core.Enums;
using FastMoq;
using FluentAssertions;
using System;
using Xunit;

namespace Cron.Tests
{
    public class NewCronTests : MockerTestBase<NewCronBuilder>
    {
        [Fact]
        public void CompoundTest()
        {
            Component.Add(CronType.Day, 5).ToString().Should().Be("0 0 5 ? ?");
            Component.Add(CronType.Hour, 5).Build().ToString().Should().Be("0 5 5 ? ?");
            Component.Add(CronType.Month, CronMonths.April).Build().ToString().Should().Be("0 5 5 4 ?");
            Component.Range(CronType.Month, CronMonths.January, CronMonths.April).Build().ToString().Should().Be("0 5 5 1-3,4 ?");
            Component.Range(CronType.Month, 1, 2).Build().ToString().Should().Be("0 5 5 1-3,4 ?");
            Component.List(CronType.Month,  CronMonths.January, CronMonths.April, CronMonths.December, CronMonths.July).Build().ToString().Should().Be("0 5 5 1-3,4,7,12 ?");
            Component.List(CronType.Month,  CronMonths.December, CronMonths.July).Build().ToString().Should().Be("0 5 5 1-3,4,7,12 ?");
        }

        private ICronExpression Create(Func<ICronBuilder, ICronBuilder> operation)
        {
            Component.Clear();
            return operation(Component).Build();
        }

        [Fact]
        public void ChainTests()
        {
            Create(x => x
                    .Increment(CronType.Minute, 0, 5)
                    .List(CronType.Hour, 13, 18)
                    .List(CronType.Hour, 4))
                .ToString()
                .Should().Be("0/5 4,13,18 ? ? ?");
            Create(x => x
                    .Increment(CronType.Minute, 0, 5)
                    .List(CronType.Hour, 13, 18)
                    .List(CronType.Hour, 4)
                    .Overwrite()
                    .List(CronType.Hour, 4, 3, 2, 1))
                .ToString()
                .Should().Be("0/5 1,2,3,4 ? ? ?");
            Create(x => x
                    .Overwrite()
                    .Increment(CronType.Minute, 0, 5)
                    .List(CronType.Hour, 13, 18)
                    .List(CronType.Hour, 4))
                .ToString()
                .Should().Be("0/5 4 ? ? ?");
            Create(x => x.Daily())
                .ToString()
                .Should().Be("0 0 * ? ?");
            Create(x => x.Daily(5))
                .ToString()
                .Should().Be("0 5 * ? ?");
            var i = Create(x => x.Daily(4,39));
            i.ToString().Should().Be("39 4 * ? ?");
            var ii = i.Builder.Daily(19).Yearly(12).Build();
            var iii = ii.Builder.Build();
            var j = Create(x => x
                .Daily(5)
                .Range(CronType.Minute, 1, 20));
            var b = Create(x => x.Hourly());
            var c = Create(x => x.Hourly(5));
            var d = Create(x => x.Hourly(3,5));
            var e = Create(x => x.Monthly());
            var f = Create(x => x.Monthly(5));
            var g = Create(x => x.Yearly(5));
        }

        [Fact]
        public void CallTest()
        {
            var t = new NewCronBuilder();
            t.Overwrite();
            t.List(CronType.Hour, 4, 5, 6).Overwrite();
        }
    }
}
