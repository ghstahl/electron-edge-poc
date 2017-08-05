using System;
using System.Threading.Tasks;

namespace Hello.Core
{

    public class Startup
    {
        public async Task<object> Invoke(object input)
        {
            return "Hello from dot net core 2";
        }
    }
}
