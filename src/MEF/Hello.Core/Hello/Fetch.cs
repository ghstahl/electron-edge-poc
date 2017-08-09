using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Programs.Repository;
using Synoptic;

namespace Hello
{
    public class Fetch
    {
        public Fetch()
        {
            Startup.Initialize();
        }

        public async Task<object> Invoke(object input)
        {
            Input strongInput = null;

            string error = null;
            RunResult runResult = null;
            string jsonRunResult = null;
            Response response = new Response() {StatusCode = 404, StatusMessage = "", Value = null};
            try
            {
                strongInput = input.ToInput();
                ExpandoObject expandoInput = input as ExpandoObject;
                var expandoDict = expandoInput as IDictionary<string, object>;

                ExpandoObject body = expandoDict["body"] as ExpandoObject;

                var routeQuery = new RouteQuery()
                {
                    Body = strongInput.Body,
                    Method = strongInput.Method,
                    Route = strongInput.Url
                };
                runResult = await (new CommandRunner()).RunViaRouteAsync(routeQuery);

                response = new Response() {StatusCode = 200, StatusMessage = "OK", Value = runResult.Value};
            }
            catch (Exception e)
            {
                error = e.Message;
                response.StatusMessage = error;
            }
            var expandoValue = response.ToExpandoObject();
            return expandoValue;
        }
    }
}