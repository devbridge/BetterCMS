using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Services
{
    public interface IContentProjectionService
    {
        PageContentProjection CreatePageContentProjection(
            GetPageToRenderRequest request, 
            PageContent pageContent, 
            List<PageContent> allPageContents, 
            IChildContent childContent = null);
    }
}
