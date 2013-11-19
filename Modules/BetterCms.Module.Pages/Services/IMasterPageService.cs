using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface IMasterPageService
    {
        IList<Guid> GetPageMasterPageIds(Guid masterPageId);

        void SetPageMasterPages(Page page, Guid masterPageId);
        
        void SetPageMasterPages(Page page, IList<Guid> masterPageIds);

        IList<OptionValueEditViewModel> GetMasterPageOptionValues(System.Guid id);
    }
}