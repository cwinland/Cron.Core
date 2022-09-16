using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    /// <summary>
    ///     Class InterfaceConverter.
    /// Implements the <see cref="JsonConverter{I}" />
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <seealso cref="JsonConverter{I}" />
    public class InterfaceConverter<M, I> : JsonConverter<I> where M : class, I
    {
        /// <summary>
        ///     Reads and converts the JSON to type <typeparamref name="T" />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override I Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<M>(ref reader, options);

        /// <summary>
        ///     Writes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        public override void Write(Utf8JsonWriter writer, I value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value as M, options);
    }
}
