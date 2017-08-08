using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
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
                return new Response() {StatusCode = 200, StatusMessage = "OK", Value = runResult.Value};
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new Response() {StatusCode = 404, StatusMessage = error, Value = null};
        }
    }
}