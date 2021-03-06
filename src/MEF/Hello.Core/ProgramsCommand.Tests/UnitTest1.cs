﻿using System;
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
    public class FetchInit
    {
        public string Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public dynamic Body { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        private CommandCompositionHelper CommandCompositionHelper { get; set; }
        public UnitTest1()
        {

            var root = Assembly.GetAssembly(typeof(UnitTestMef)).Location;
            var dir = Path.GetDirectoryName(root);
            var components = dir;
            var dd = Programs.ProgramsRepository;
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

            var routeQuery = new RouteQuery()
            {
                Body = new PageQuery() {Offset = 0, Count = count},
                Method = "GET",
                Route = "v1/programs/page"
            };

            var runResult = await (new CommandRunner()).RunViaRouteAsync(routeQuery);

            var items = runResult.Value as InstalledApp[];
            Assert.AreEqual(count, items.Length);

            var displayName = items[0].DisplayName;

            json = JsonConvert.SerializeObject(
                new IsInstalledQuery() {DisplayName = displayName },
                camelSettings);

            routeQuery = new RouteQuery()
            {
                Body = new IsInstalledQuery() {DisplayName = displayName},
                Method = "GET",
                Route = "v1/programs/is-installed"
            };
            runResult = await (new CommandRunner()).RunViaRouteAsync(routeQuery);
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

            var routeQuery = new RouteQuery()
            {
                Body = new PageQuery() { Offset = 0, Count = 100 },
                Method = "GET",
                Route = "v1/programs/page"
            };
            var runResult = await (new CommandRunner()).RunViaRouteAsync(routeQuery);

            var items = runResult.Value as InstalledApp[];
            Assert.AreEqual(100, items.Length);
        }
    }
}
