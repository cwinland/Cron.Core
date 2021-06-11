using Cron.Core.Interfaces;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    public class ICronInterfaceConverterFactory : JsonConverterFactory
    {
        public ICronInterfaceConverterFactory(Type interfaceType) => InterfaceType = interfaceType;

        public Type InterfaceType { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.Equals(typeof(ICron).MakeGenericType(InterfaceType)) &&
                typeToConvert.GenericTypeArguments[0].Equals(InterfaceType))
            {
                return true;
            }

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(typeof(CronConverter<>).MakeGenericType(InterfaceType));
    }
}
