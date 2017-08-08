using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Programs.Models;
using Synoptic;

namespace Command.POC.Callbacks
{
    [Command(RouteBase = "v1/command-source")]
    public class CommandSource
    {
        [CommandAction(Route = "immediate-callback", Method = "POST")]
        public async Task ImmediateCallbackAsync([CommandParameter(FromBody = true)]CallbackQuery body)
        {
            await body.CallbackFunc("Hello from ImmediateCallbackAsync");
        }
    }
}
