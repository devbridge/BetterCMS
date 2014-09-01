using System.Collections.Generic;

using BetterCms.Core.Modules;

namespace BetterCms.Module.Root.Services
{
    public interface ISettingsService
    {
        List<ModuleDescriptor> GetActiveModules();
    }
}
