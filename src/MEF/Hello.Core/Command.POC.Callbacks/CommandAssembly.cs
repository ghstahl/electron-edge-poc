using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Command.MEF.Contracts;

namespace Command.POC.Callbacks
{
    [Export(typeof(ICommandAssembly))]
    public class CommandAssembly : ICommandAssembly
    {
        public void Initialize()
        {

        }
    }
}
