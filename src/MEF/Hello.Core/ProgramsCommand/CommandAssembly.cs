using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Command.MEF.Contracts;
using Programs.Repository;

namespace ProgramsCommand
{
    [Export(typeof(ICommandAssembly))]
    [ExportMetadata("CalciMetaData", "Add")]
    public class CommandAssembly : ICommandAssembly
    {
        public void Initialize()
        {
            var programsRepository = new ProgramsRepository();
            ProgramsCommand.Programs.ProgramsRepository = programsRepository;
            ProgramsCommand.Processes.ProgramsRepository = programsRepository;
        }
    }
}
