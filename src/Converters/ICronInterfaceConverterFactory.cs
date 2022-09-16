using Cron.Core.Interfaces;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    /// <summary>
    ///     Class ICronInterfaceConverterFactory.
    /// Implements the <see cref="JsonConverterFactory" />
    /// </summary>
    /// <seealso cref="JsonConverterFactory" />
    public class ICronInterfaceConverterFactory : JsonConverterFactory
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ICronInterfaceConverterFactory"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        public ICronInterfaceConverterFactory(Type interfaceType) => InterfaceType = interfaceType;

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
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(ICron).MakeGenericType(InterfaceType) &&
                   typeToConvert.GenericTypeArguments?[0] == InterfaceType;
        }

        /// <summary>
        ///     Creates the converter.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The options.</param>
        /// <returns>JsonConverter.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(typeof(CronConverter<>).MakeGenericType(InterfaceType));
    }
}
