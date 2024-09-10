using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactCenter.Data
{
    internal static class JSONHelper
    {
        public static JsonSerializerOptions SerializerOptions
        { get; } = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
        };

        public const string JsonMIME = "application/json";
    }
}
