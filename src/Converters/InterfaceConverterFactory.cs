using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    public class InterfaceConverterFactory : JsonConverterFactory
    {
        public InterfaceConverterFactory(Type concrete, Type interfaceType)
        {
            ConcreteType = concrete;
            InterfaceType = interfaceType;
        }

        public Type ConcreteType { get; }
        public Type InterfaceType { get; }

        public override bool CanConvert(Type typeToConvert) => typeToConvert == InterfaceType;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(InterfaceConverter<,>).MakeGenericType(ConcreteType, InterfaceType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}
