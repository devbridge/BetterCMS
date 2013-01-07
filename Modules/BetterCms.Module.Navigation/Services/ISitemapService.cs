using System;
using Page = BetterCms.Module.Pages.Models.PageProperties;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// </summary>    
    public interface IPageService
    {        
        void ValidatePageUrl(string url, Guid? pageId = null);
    }
}