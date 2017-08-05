using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hello
{
    public static class ExpandoObjectExtensions
    {
        public static ExpandoObject Clone(this ExpandoObject original)
        {
            var expandoObjectConverter = new ExpandoObjectConverter();
            var originalDoc = JsonConvert.SerializeObject(original, expandoObjectConverter);
            dynamic clone = JsonConvert.DeserializeObject<ExpandoObject>(originalDoc, expandoObjectConverter);
            return clone;
        }
        public static string ToJson(this ExpandoObject original)
        {
            var expandoObjectConverter = new ExpandoObjectConverter();
            var originalDoc = JsonConvert.SerializeObject(original, expandoObjectConverter);
            return originalDoc;
        }
    }
}