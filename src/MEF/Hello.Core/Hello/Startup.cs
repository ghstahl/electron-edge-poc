using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Collections;
using Newtonsoft.Json;
using Programs.Repository;
using Synoptic;

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
        public Startup()
        {
            var programsRepository = new ProgramsRepository();
            ProgramsCommand.Programs.ProgramsRepository = programsRepository;
            ProgramsCommand.Processes.ProgramsRepository = programsRepository;
        }

        public static int count = 0;

        public async Task<object> Invoke(object input)
        {
            Input strongInput = null;
            string json = null;
            string error = null;
            RunResult runResult = null;
            string jsonRunResult = null;
            try
            {
                strongInput = input.ToInput();
                ExpandoObject expandoInput = input as ExpandoObject;
                var expandoDict = expandoInput as IDictionary<string, object>;

                ExpandoObject body = expandoDict["body"] as ExpandoObject;
                
                json = expandoInput.ToJson();
                runResult = new CommandRunner().RunViaRoute(new[]
                {
                    strongInput.Url,
                    strongInput.Method,
                    $@"--body={strongInput.JsonBody}"
                });
                jsonRunResult = JsonConvert.SerializeObject(runResult);
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            ++count;
            if (!string.IsNullOrEmpty(error))
            {
                return $"Hello from dot net:[{count}],error[{error}]";
            }
            return $"Hello from dot net:[{count}],json[{json}]," +
                   $"url[{strongInput.Url}],method[{strongInput.Method}]," +
                   $"jsonHeaders[{strongInput.JsonHeaders}],jsonBody[{strongInput.JsonBody}]," +
                   $"jsonRunResult:[{jsonRunResult}]";
        }
    }
}