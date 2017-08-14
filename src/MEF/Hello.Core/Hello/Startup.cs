using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.IO;
using System.Reflection;
using Command.FileLoader;
using Command.MEF.Common;
using Newtonsoft.Json;
using Programs.Repository;
using Synoptic;

namespace Hello
{
    public class Startup
    {
        public static CommandCompositionHelper CommandCompositionHelper { get; set; }

        public static void Initialize()
        {
            if (CommandCompositionHelper == null)
            {
                var root = Assembly.GetAssembly(typeof(Startup)).Location;
                var dir = Path.GetDirectoryName(root);
                var components = dir;
               
                //  var components = Path.Combine(dir, "components");
                CommandCompositionHelper = new CommandCompositionHelper(components);
                CommandCompositionHelper.AssembleCommandComponents();
                CommandCompositionHelper.Initialize();
                JsonFile.RootFolder = ""; // just making sure that the dll gets copied
            }

        }

        public Startup()
        {
            Startup.Initialize();
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
                var routeQuery = new RouteQuery()
                {
                    Body = strongInput.Body,
                    Method = strongInput.Method,
                    Route = strongInput.Url
                };

                runResult = await (new CommandRunner()).RunViaRouteAsync(routeQuery);
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