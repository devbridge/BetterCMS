using System;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface IMasterPageService
    {
        void SetPageMasterPages(Page page, Guid masterPageId);
    }
}