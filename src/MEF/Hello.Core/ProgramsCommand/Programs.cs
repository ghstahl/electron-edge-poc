using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Command.Common;
using Programs.Models;
using Programs.Repository;
using Synoptic;

namespace ProgramsCommand
{
    [Command(RouteBase = "v1/programs")]
    public class Programs
    {
        public static IProgramsRepository ProgramsRepository { get; set; }
        [CommandAction(Route = "load",Method = "POST")]
        public void PostLoad()
        {
            ProgramsRepository.LoadInstall();
        }

        [CommandAction(Route = "count", Method = "GET")]
        public PrimitiveValue<int> GetCount()
        {
            var result = ProgramsRepository.InstallCount;
            return new PrimitiveValue<int>(result);
        }

        [CommandAction(Route = "is-installed", Method = "GET")]
        public async Task<PrimitiveValue<bool>> GetIsInstalled([CommandParameter(FromBody = true)]IsInstalledQuery body)
        {
            var result = ProgramsRepository.IsInstalled(body.DisplayName);
            return new PrimitiveValue<bool>(result);
        }

        [CommandAction(Route = "page", Method = "GET")]
        public InstalledApp[] GetPage([CommandParameter(FromBody = true)]PageQuery body)
        {
            var result = ProgramsRepository.PageInstalled(body.Offset, body.Count);
            return result;
        }

        [CommandAction(Route = "launch-url", Method = "POST")]
        public LaunchUrlResult PostLaunchUrl([CommandParameter(FromBody = true)]LaunchUrlQuery body)
        {
            return ProgramsRepository.LaunchUrl(body.Url);
        }

        [CommandAction(Route = "launch-special", Method = "POST")]
        public LaunchResult PostLaunchSpecial([CommandParameter(FromBody = true)]LaunchSpecialQuery body)
        {
            return ProgramsRepository.LaunchSpecial(body);
        }

    }
}
