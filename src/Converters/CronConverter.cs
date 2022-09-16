using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    /// <summary>
    ///     Class CronConverter.
    /// Implements the <see cref="System.Text.Json.Serialization.JsonConverter{System.Collections.Generic.IList{M}}" />
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <seealso cref="System.Text.Json.Serialization.JsonConverter{System.Collections.Generic.IList{M}}" />
    public class CronConverter<M> : JsonConverter<IList<M>>
    {
        /// <summary>
        ///     Reads and converts the JSON to type <typeparamref name="T" />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override IList<M> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<List<M>>(ref reader, options);

        /// <summary>
        ///     Writes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, IList<M> value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
