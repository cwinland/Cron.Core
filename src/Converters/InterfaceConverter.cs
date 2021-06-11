using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    public class InterfaceConverter<M, I> : JsonConverter<I> where M : class, I
    {
        public override I Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<M>(ref reader, options);

        public override void Write(Utf8JsonWriter writer, I value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value as M, options);
    }
}
