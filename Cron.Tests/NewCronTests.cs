using Cron.Core;
using Cron.Core.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using Cron.Core.Extensions;
namespace Cron.Tests
{
    public class NewCronTests
    {
        private NewCronBuilder cronBuilder;

        public NewCronTests()
        {
            cronBuilder = new();
        }

        [Fact]
        public void Test()
        {
            var cronBuilder = new NewCronBuilder();
            cronBuilder.Add(CronType.Day, 5);
            cronBuilder.Add(CronType.Hour, 5);
            cronBuilder.Add(CronType.Month, CronMonths.April);
            cronBuilder.Range(CronType.Month, CronMonths.January, CronMonths.April);
            cronBuilder.Range(CronType.Month, 1, 2);
            cronBuilder.List(CronType.Month,  new List<CronMonths> {CronMonths.January, CronMonths.April, CronMonths.December, CronMonths.July});
            var cronExpression = cronBuilder.Build();
        }

        private ICronExpression Create(Func<ICronBuilder, ICronBuilder> operation)
        {
            cronBuilder.Clear();
            return operation(cronBuilder).Build();
        }

        [Fact]
        public void Test2()
        {
            var x = Create(x => x
                .Increment(CronType.Minute, 0, 5)
                .List(CronType.Hour, 13, 18)
                .List(CronType.Hour, 4));
            var y = Create(x => x
                .Increment(CronType.Minute, 0, 5)
                .List(CronType.Hour, 13, 18)
                .List(CronType.Hour, 4)
                .Overwrite()
                .List(CronType.Hour, 4,3,2,1));
            var z = Create(x => x
                .Overwrite()
                .Increment(CronType.Minute, 0, 5)
                .List(CronType.Hour, 13, 18)
                .List(CronType.Hour, 4));
            var a = Create(x => x.Daily());
            var h = Create(x => x.Daily(5));
            var i = Create(x => x.Daily(4,39));
            var ii = i.ToBuilder().Daily(19).Yearly(12).Build();
            var iii = ii.ToBuilder().Build();
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
