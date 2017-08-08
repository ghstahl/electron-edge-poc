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
            var programsRepository = new ProgramsRepository();
            ProgramsCommand.Programs.ProgramsRepository = programsRepository;
            ProgramsCommand.Processes.ProgramsRepository = programsRepository;
        }
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