﻿using System.Text.Json;
using Cron.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Tests
{
    [TestClass]
    public class CronSerializationTests
    {
        [TestMethod]
        [DataRow("* * * * *", false, "{\"Value\":\"* * * * *\",\"AllowSeconds\":false}")]
        [DataRow("1 2 3 4 5", false, "{\"Value\":\"1 2 3 4 5\",\"AllowSeconds\":false}")]
        [DataRow("* * * * *", true, "{\"Value\":\"* * * * * *\",\"AllowSeconds\":true}")]
        [DataRow("1 2 3 4 5", true, "{\"Value\":\"* 1 2 3 4 5\",\"AllowSeconds\":true}")]
        public void Serialize(string expression, bool allowSeconds, string expectedSerialization)
        {
            var schedule = new CronBuilder(expression, allowSeconds);
            JsonSerializer.Serialize(schedule)
                          .Should()
                          .Be(expectedSerialization);
        }

        [TestMethod]

        //[DataRow("* * * * *", false, "{\"Value\":\"* * * * *\",\"AllowSeconds\":false}")]
        //[DataRow("1 2 3 4 5", false, "{\"Value\":\"1 2 3 4 5\",\"AllowSeconds\":false}")]
        //[DataRow("* * * * * *", true, "{\"Value\":\"* * * * *\",\"AllowSeconds\":true}")]
        [DataRow("1 1 2 3 4 5", true, "{\"Value\":\"1 1 2 3 4 5\",\"AllowSeconds\":true}")]
        public void Deserialize(string expectedExpression, bool allowSeconds, string serializedSchedule)
        {
            var newSchedule = JsonSerializer.Deserialize<CronBuilder>(serializedSchedule);
            newSchedule.Description.Should().Be(new CronBuilder(expectedExpression, allowSeconds).Description);
            newSchedule.Value.Should().Be(expectedExpression);
            newSchedule.AllowSeconds.Should().Be(allowSeconds);
        }
    }
}
