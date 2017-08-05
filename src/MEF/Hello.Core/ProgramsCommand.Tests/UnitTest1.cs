using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Programs.Repository;
using Synoptic;

namespace ProgramsCommand.Tests
{
    public class FetchInit
    {
        public string Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public dynamic Body { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            var programsRepository = new ProgramsRepository();
            ProgramsCommand.Programs.ProgramsRepository = programsRepository;
            ProgramsCommand.Processes.ProgramsRepository = programsRepository;
        }
        [TestMethod]
        public void TestMethod1()
        {
            var camelSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var fetchInit = new FetchInit() { Headers = new Dictionary<string, string>(), Method = "Get" };

            var json = JsonConvert.SerializeObject(
                new IsInstalledQuery() { DisplayName = "Norton Internet Security"},
                camelSettings);

            fetchInit.Body = JObject.Parse(json);
            var jsonFetchInit = JsonConvert.SerializeObject(fetchInit, camelSettings);

            var fetchInit2 = JsonConvert.DeserializeObject<FetchInit>(jsonFetchInit);
            var jsonFetchInit2 = JsonConvert.SerializeObject(fetchInit2, camelSettings);

            var body = fetchInit2.Body;
            var jsonBody = JsonConvert.SerializeObject((object)body, camelSettings);

            var runResult = new CommandRunner().RunViaRoute(new[]
            {
                "v1/programs/is-installed",
                "GET",
                string.Format(@"--body={0}", json)
            });
        }
    }
}
