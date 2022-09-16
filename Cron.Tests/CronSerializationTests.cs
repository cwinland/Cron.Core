using Cron.Core;
using Cron.Core.Interfaces;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Cron.Tests
{
    
    public class CronSerializationTests
    {
        [Theory]
        [InlineData("* * * * *", false, "{\"Value\":\"* * * * *\",\"AllowSeconds\":false}")]
        [InlineData("1 2 3 4 5", false, "{\"Value\":\"1 2 3 4 5\",\"AllowSeconds\":false}")]
        [InlineData("* * * * *", true, "{\"Value\":\"* * * * * *\",\"AllowSeconds\":true}")]
        [InlineData("1 2 3 4 5", true, "{\"Value\":\"* 1 2 3 4 5\",\"AllowSeconds\":true}")]
        public void Serialize(string expression, bool allowSeconds, string expectedSerialization)
        {
            var schedule = new CronBuilder(expression, allowSeconds);
            JsonSerializer.Serialize(schedule)
                          .Should()
                          .Be(expectedSerialization);
        }

        [Theory]
        [InlineData("* * * * *", false, "{\"Value\":\"* * * * *\",\"AllowSeconds\":false}")]
        [InlineData("1 2 3 4 5", false, "{\"Value\":\"1 2 3 4 5\",\"AllowSeconds\":false}")]
        [InlineData("* * * * * *", true, "{\"Value\":\"* * * * *\",\"AllowSeconds\":true}")]
        [InlineData("1 1 2 3 4 5", true, "{\"Value\":\"1 1 2 3 4 5\",\"AllowSeconds\":true}")]
        public void Deserialize(string expectedExpression, bool allowSeconds, string serializedSchedule)
        {
            var newSchedule = JsonSerializer.Deserialize<CronBuilder>(serializedSchedule);
            newSchedule.Description.Should().Be(new CronBuilder(expectedExpression, allowSeconds).Description);
            newSchedule.Value.Should().Be(expectedExpression);
            newSchedule.AllowSeconds.Should().Be(allowSeconds);
        }

        [Theory]
        [InlineData("* * * * *", false, "{\"Value\":\"* * * * *\",\"AllowSeconds\":false}")]
        [InlineData("1 2 3 4 5", false, "{\"Value\":\"1 2 3 4 5\",\"AllowSeconds\":false}")]
        [InlineData("* * * * *", true, "{\"Value\":\"* * * * * *\",\"AllowSeconds\":true}")]
        [InlineData("1 2 3 4 5", true, "{\"Value\":\"* 1 2 3 4 5\",\"AllowSeconds\":true}")]
        public void SerializeInterface(string expression, bool allowSeconds, string expectedSerialization)
        {
            ICron schedule = new CronBuilder(expression, allowSeconds);
            JsonSerializer.Serialize(schedule)
                          .Should()
                          .Be(expectedSerialization);
        }

        [Theory]
        [InlineData("* * * * *", false, "{\"Value\":\"* * * * *\",\"AllowSeconds\":false}")]
        [InlineData("1 2 3 4 5", false, "{\"Value\":\"1 2 3 4 5\",\"AllowSeconds\":false}")]
        [InlineData("* * * * * *", true, "{\"Value\":\"* * * * *\",\"AllowSeconds\":true}")]
        [InlineData("1 1 2 3 4 5", true, "{\"Value\":\"1 1 2 3 4 5\",\"AllowSeconds\":true}")]
        public void DeserializeInterface(string expectedExpression, bool allowSeconds, string serializedSchedule)
        {
            var newSchedule = JsonSerializer.Deserialize<ICron>(serializedSchedule);
            newSchedule.Description.Should().Be(new CronBuilder(expectedExpression, allowSeconds).Description);
            newSchedule.Value.Should().Be(expectedExpression);
            newSchedule.AllowSeconds.Should().Be(allowSeconds);
        }
    }
}
