using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Programs.Models;
using Synoptic;

namespace Command.POC.Callbacks
{

    public class AddCallbackQuery
    {
        public int A { get; set; }
        public int B { get; set; }
        public Func<object, Task<object>> Add { get; set; }
    }

    [Command(RouteBase = "v1/command-source")]
    public class CommandSource
    {
        [CommandAction(Route = "immediate-callback", Method = "POST")]
        public async Task ImmediateCallbackAsync([CommandParameter(FromBody = true)]AddCallbackQuery body)
        {
            var add = (Func<object, Task<object>>)body.Add;
            var twoNumbers = new { a = (int)body.A, b = (int)body.B };
            var addResult = (int)await add(twoNumbers);
           
        }
    }
}
