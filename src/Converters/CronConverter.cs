using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    public class CronConverter<M> : JsonConverter<IList<M>>
    {
        public override IList<M> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<List<M>>(ref reader, options);

        public override void Write(Utf8JsonWriter writer, IList<M> value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
