/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace XFS4IoT
{
    /// <summary>
    /// Converter for base64 string &lt;-&gt; byte array
    /// Adding attribute [JsonConverter(typeof(Base64Converter))] in the message property for target field to be converted
    /// </summary>
    internal class Base64Converter : JsonConverter<List<byte>>
    {
        public override List<byte> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetBytesFromBase64().ToList();
        }

        public override void Write(Utf8JsonWriter writer, List<byte> value, JsonSerializerOptions options)
        {
            writer.WriteBase64StringValue(value.ToArray());
        }
    }

    /// <summary>
    /// Converter for properties which can be a single string element or an array of strings.
    /// In C# we always access these properties as a string list.
    /// If there is only a single element in the string list then a single string will be output rather than a string array.
    /// </summary>
    internal class StringOrArrayConverter : JsonConverter<Dictionary<string, List<string>>>
    {
        public override Dictionary<string, List<string>> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Unexpected start token for JSON reader. " + reader.TokenType);

            Dictionary<string, List<string>> values = new Dictionary<string, List<string>>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Unexpected token for JSON reader. " + reader.TokenType);

                string propName = reader.GetString();
                List<string> value = new List<string>();

                if (!reader.Read())
                    throw new JsonException("Failed to read token after property name.");

                if (reader.TokenType == JsonTokenType.String)
                {
                    value.Add(reader.GetString());
                }
                else if (reader.TokenType == JsonTokenType.StartArray)
                {
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType == JsonTokenType.String)
                            value.Add(reader.GetString());
                        else
                            throw new JsonException("Unexpected token type inside JSON array. " + reader.TokenType);
                    }
                }
                else
                    throw new JsonException("Unexpected token type reading value. " + reader.TokenType);

                values.Add(propName, value);
            }

            return values;
        }

        public override void Write(
            Utf8JsonWriter writer,
            Dictionary<string, List<string>> values,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var kv in values)
            {
                if (kv.Value == null || kv.Value.Count == 0)
                    continue; //Don't output empty values.

                writer.WritePropertyName(kv.Key);

                if (kv.Value.Count == 1)
                    writer.WriteStringValue(kv.Value[0]); //Treat single element as a string
                else
                {
                    writer.WriteStartArray(); //Treat multiple elements as an array.
                    foreach (var item in kv.Value)
                        writer.WriteStringValue(item);
                    writer.WriteEndArray();
                }
            }
            writer.WriteEndObject();
        }
    }
}
