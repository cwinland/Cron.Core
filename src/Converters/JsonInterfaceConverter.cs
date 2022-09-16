using Cron.Core.Interfaces;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cron.Core.Converters
{
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.

    /// <summary>
    ///     When placed on a property, field, or type, specifies the converter type to use.
    /// </summary>
    /// <remarks>The specified converter type must derive from <see cref="JsonConverter" />.
    /// When placed on a property or field, the specified converter will always be used.
    /// When placed on a type, the specified converter will be used unless a compatible converter is added to
    /// <see cref="JsonSerializerOptions.Converters" /> or there is another <see cref="JsonConverterAttribute" /> on a property or field
    /// of the same type.</remarks>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonInterfaceConverterAttribute : JsonConverterAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonInterfaceConverterAttribute"/> class.
        /// </summary>
        /// <param name="converterType">Type of the converter.</param>
        public JsonInterfaceConverterAttribute(Type converterType) : base(converterType) { }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert) => new InterfaceConverterFactory(typeof(CronBuilder), typeof(ICron));
    }
}

