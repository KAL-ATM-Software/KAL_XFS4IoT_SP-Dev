/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    /// Converter for base64 string <-> byte array
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
}
