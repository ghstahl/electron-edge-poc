using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Command.Common;
using Command.MEF.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Programs.Models;
using Synoptic;

namespace ProgramsCommand.Tests
{


    [TestClass]
    public class UnitTestMef{
        private CommandCompositionHelper CommandCompositionHelper { get; set; }
        public UnitTestMef()
        {
            var root = Assembly.GetAssembly(typeof(UnitTestMef)).Location;
            var dir = Path.GetDirectoryName(root);
            var components = dir;
            //  var components = Path.Combine(dir, "components");
            CommandCompositionHelper = new CommandCompositionHelper(components);
            CommandCompositionHelper.AssembleCommandComponents();
            CommandCompositionHelper.Initialize();
        }

        [TestMethod]
        public async Task TestMethod_is_installed_success()
        {
            var camelSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var count = 10;
            var json = JsonConvert.SerializeObject(
                new PageQuery() { Offset = 0, Count = count },
                camelSettings);


            var runResult = await (new CommandRunner()).RunViaRouteAsync(new[]
            {
                "v1/programs/page",
                "GET",
                $@"--body={json}"
            });

            var items = runResult.Value as InstalledApp[];
            Assert.AreEqual(count, items.Length);

            var displayName = items[0].DisplayName;

            json = JsonConvert.SerializeObject(
                new IsInstalledQuery() {DisplayName = displayName },
                camelSettings);

           
            runResult = await (new CommandRunner()).RunViaRouteAsync(new[]
            {
                "v1/programs/is-installed",
                "GET",
                $@"--body={json}"
            });
            PrimitiveValue<bool> result = runResult.Value as PrimitiveValue<bool>;
            Assert.AreEqual(true, result.Value);
        }

        [TestMethod]
        public async Task TestMethod_page_success()
        {
            var camelSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            
            var json = JsonConvert.SerializeObject(
                new PageQuery() {Offset = 0, Count = 100},
                camelSettings);


            var runResult = await (new CommandRunner()).RunViaRouteAsync(new[]
            {
                "v1/programs/page",
                "GET",
                $@"--body={json}"
            });

            var items = runResult.Value as InstalledApp[];
            Assert.AreEqual(100, items.Length);
        }
    }
}
