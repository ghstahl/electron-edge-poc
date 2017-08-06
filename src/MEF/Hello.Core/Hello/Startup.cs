using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Collections;

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


    public class Startup
    {


        public static int count = 0;
        public async Task<object> Invoke(object input)
        {
            string json = "not set";
            string jsonBody = "not set";
            string url = "not set";
            string method = "not set"; try
            {
                ExpandoObject expandoInput = input as ExpandoObject;
                var expandoDict = expandoInput as IDictionary<string, object>;
                url = expandoDict["url"] as string;
                method = expandoDict["method"] as string;
                ExpandoObject body = expandoDict["body"] as ExpandoObject; ;
                json = expandoInput.ToJson();
                jsonBody = body.ToJson();
                url.ValidateStartsWith("local://");
                url = url.RemoveFirst("local://");
                method.ValidateMethod();
            }
            catch (Exception e)
            {
                json = e.Message;
            }
     
            ++count;
            return $"Hello from dot net:[{count}],json[{json}],url[{url}],method[{method}],jsonBody[{jsonBody}]";
        }
    }
}
