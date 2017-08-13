using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Programs.Models;
using Synoptic;
using System.Diagnostics;
using Command.Common;

namespace Command.POC.Callbacks
{

    public class AddCallbackQuery
    {
        public int A { get; set; }
        public int B { get; set; }
        public Func<object, Task<object>> Add { get; set; }
    }

    public class HeartBeatRegistrationQuery
    {
        public Func<object, Task<object>> Heart { get; set; }
    }

    public class HeartBeatRegistrationResult
    {
        public string Key { get; set; }
    }

    public class HeartBeatUnregistrationQuery
    {
        public string Key { get; set; }
    }

    [Command(RouteBase = "v1/command-source")]
    public class CommandSource
    {
        public static Thread HeartbeatThread { get; set; }
        public static int Beats { get; set; }
        public static Dictionary<string, Func<object, Task<object>>> Hearts =
            new Dictionary<string, Func<object, Task<object>>>();

        public CommandSource()
        {
            lock (Hearts)
            {
                Beats = 0;
                if (HeartbeatThread == null)
                {
                    HeartbeatThread = new Thread(new ThreadStart(Beat));
                    HeartbeatThread.Start();
                }
            }
        }

        static void Beat()
        {
            while (true)
            {
                Thread.Sleep(1000);
                lock (Hearts)
                {
                    ++Beats;

                    foreach (var heart in Hearts)
                    {
                        var theBeat = new { key = (string)heart.Key, b = (int)Beats };

                        var t = Task.Run(() => heart.Value(theBeat));
                        t.Wait();
                        Debug.WriteLine(t.Result);
                    }
                }
            }
        }

        [CommandAction(Route = "immediate-callback", Method = "POST")]
        public async Task<PrimitiveValue<int>> ImmediateCallbackAsync([CommandParameter(FromBody = true)] AddCallbackQuery body)
        {
            var add = (Func<object, Task<object>>) body.Add;
            var twoNumbers = new {a = (int) body.A, b = (int) body.B};
            var addResult = (int) await add(twoNumbers);
            return new PrimitiveValue<int>(addResult);
        }

        [CommandAction(Route = "register-heart", Method = "POST")]
        public async Task<HeartBeatRegistrationResult> RegisterHeartbeatAsync(
            [CommandParameter(FromBody = true)] HeartBeatRegistrationQuery body)
        {
            lock (Hearts)
            {
                var heart = (Func<object, Task<object>>) body.Heart;
                var key = Guid.NewGuid().ToString();
                Hearts.Add(key, heart);
                return new HeartBeatRegistrationResult() {Key = key};
            }
        }

        [CommandAction(Route = "unregister-heart", Method = "POST")]
        public async Task<HeartBeatRegistrationResult> UnregisterHeartbeatAsync(
            [CommandParameter(FromBody = true)] HeartBeatUnregistrationQuery body)
        {
            lock (Hearts)
            {
                Hearts.Remove(body.Key);
                return new HeartBeatRegistrationResult() {Key = body.Key};
            }
        }
    }
}
