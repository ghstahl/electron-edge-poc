using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using Newtonsoft.Json.Converters;

namespace Hello
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
       
    }
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

    public class Startup
    {


        public static int count = 0;
        public async Task<object> Invoke(object input)
        {
            string json;
            try
            {
                ExpandoObject expandoInput = input as ExpandoObject;
                var expandoDict = expandoInput as IDictionary<string, object>;
                dynamic body = expandoDict["body"];
                json = expandoInput.ToJson();
            }
            catch (Exception e)
            {
                json = e.Message;
            }
     
            ++count;
            return $"Hello from dot net:[{count}],json[{json}]";
        }
    }
}
