using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Page;

namespace BetterCms.Module.Pages.Services
{
    public interface ILayoutService
    {
        IList<TemplateViewModel> GetTemplates();
    }
}