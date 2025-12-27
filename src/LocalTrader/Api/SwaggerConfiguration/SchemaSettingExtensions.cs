using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.Generation.TypeMappers;
using StronglyTypedIds;

namespace LocalTrader.Api.SwaggerConfiguration;

public static class SchemaSettingExtensions
{
    public static void RegisterTypeMappers<TAssemblyMarker>(this JsonSchemaGeneratorSettings settings)
    {
        var mappers = typeof(TAssemblyMarker)
            .Assembly
            .GetTypes()
            .Where(x => Attribute.IsDefined(x, typeof(StronglyTypedIdAttribute)))
            .Select(GetMappers)
            .SelectMany(x => x);

        foreach (var mapper in mappers)
        {
            settings.TypeMappers.Add(mapper);
        }
    }

    private static IEnumerable<PrimitiveTypeMapper> GetMappers(Type type)
    {
        var primitiveType = type.GetProperty("Value")?.PropertyType;
        if (primitiveType is null) yield break;

        if (primitiveType == typeof(string)) yield break;
        
        var modAction = GetModAction(primitiveType);
        
        yield return new PrimitiveTypeMapper(type, x =>
        {
            x.Type = modAction.objectType;
            x.Format = modAction.format ?? x.Format;
        });
        
        var nullableType = typeof(Nullable<>).MakeGenericType(type);
        yield return new PrimitiveTypeMapper(nullableType, x =>
        {
            x.Type = modAction.objectType;
            x.Format = modAction.format ?? x.Format;
            x.IsNullableRaw = true;
        });
    }

    private static (JsonObjectType objectType, string? format) GetModAction(Type primitiveType)
    {
        return primitiveType switch
        {
            not null when primitiveType == typeof(bool)    => (JsonObjectType.Boolean, "" ),
            not null when primitiveType == typeof(char)    => (JsonObjectType.String, "" ),
            not null when primitiveType == typeof(string)  => (JsonObjectType.String, "" ),
            not null when primitiveType == typeof(Guid)    => (JsonObjectType.String, "uuid"),
            not null when primitiveType == typeof(short)   => (JsonObjectType.Integer, "int16"),
            not null when primitiveType == typeof(int)     => (JsonObjectType.Integer, "int32"),
            not null when primitiveType == typeof(long)    => (JsonObjectType.Integer, "int64"),
            not null when primitiveType == typeof(ushort)  => (JsonObjectType.Integer, "uint16"),
            not null when primitiveType == typeof(uint)    => (JsonObjectType.Integer, "uint32"),
            not null when primitiveType == typeof(ulong)   => (JsonObjectType.Integer, "uint64"),
            not null when primitiveType == typeof(float)   => (JsonObjectType.Number, "float"),
            not null when primitiveType == typeof(double)  => (JsonObjectType.Number, "double"),
            not null when primitiveType == typeof(decimal) => (JsonObjectType.Number, "double"),
            not null when primitiveType == typeof(byte)    => (JsonObjectType.Number, "uint8"),
            not null when primitiveType == typeof(sbyte)   => (JsonObjectType.Number, "int8"),
            _                                              => (JsonObjectType.Object, "" )};
    }
}