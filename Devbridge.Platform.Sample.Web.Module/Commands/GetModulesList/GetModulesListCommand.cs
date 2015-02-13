using System.Linq;

using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Web.Modules;
using Devbridge.Platform.Core.Web.Mvc.Commands;
using Devbridge.Platform.Sample.Web.Module.Models;

namespace Devbridge.Platform.Sample.Web.Module.Commands.GetModulesList
{
    public class GetModulesListCommand : CoreCommandBase, ICommandOut<ModulesListViewModel>
    {
        private readonly IModulesRegistration modulesRegistration;

        public GetModulesListCommand(IModulesRegistration modulesRegistration)
        {
            this.modulesRegistration = modulesRegistration;
        }

        public ModulesListViewModel Execute()
        {
            var modules = modulesRegistration.GetModules()
                    .Select(
                        m =>
                            new ModuleViewModel
                            {
                                Assembly = m.ModuleDescriptor.AssemblyName.Name,
                                Description = m.ModuleDescriptor.Description,
                                Name = m.ModuleDescriptor.Name,
                                Type = m.ModuleDescriptor is WebModuleDescriptor ? "Core.Web" : "Core"
                            })
                    .ToList();

            return new ModulesListViewModel {Modules = modules};
        }
    }
}
