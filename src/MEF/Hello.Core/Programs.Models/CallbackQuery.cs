using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programs.Models
{
    public class CallbackQuery
    {
        public Func<object, Task<object>> CallbackFunc { get; set; }
    }
}
