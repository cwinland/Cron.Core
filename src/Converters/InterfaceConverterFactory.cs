using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    /// <summary>
    ///     Class InterfaceConverterFactory.
    /// Implements the <see cref="JsonConverterFactory" />
    /// </summary>
    /// <seealso cref="JsonConverterFactory" />
    public class InterfaceConverterFactory : JsonConverterFactory
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InterfaceConverterFactory"/> class.
        /// </summary>
        /// <param name="concrete">The concrete.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        public InterfaceConverterFactory(Type concrete, Type interfaceType)
        {
            ConcreteType = concrete;
            InterfaceType = interfaceType;
        }

        /// <summary>
        ///     Gets the type of the concrete.
        /// </summary>
        /// <value>The type of the concrete.</value>
        public Type ConcreteType { get; }
        /// <summary>
        ///     Gets the type of the interface.
        /// </summary>
        /// <value>The type of the interface.</value>
        public Type InterfaceType { get; }

        /// <summary>
        ///     When overridden in a derived class, determines whether the converter instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><see langword="true" /> if the instance can convert the specified object type; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert) => typeToConvert == InterfaceType;

        /// <summary>
        ///     Creates the converter.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The options.</param>
        /// <returns>JsonConverter.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(InterfaceConverter<,>).MakeGenericType(ConcreteType, InterfaceType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}
