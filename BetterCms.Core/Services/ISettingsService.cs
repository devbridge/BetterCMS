using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Core.Services
{
    public interface ISettingsService
    {
        List<Setting> GetModuleSettings(Guid moduleId);

        void SaveModuleSettings(Guid moduleId, IEnumerable<Setting> settings);
    }
}
