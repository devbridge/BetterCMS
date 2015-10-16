using System;
using System.Collections.Generic;

using BetterCms.Module.Root.ViewModels.Rendering;

namespace BetterCms.Module.Root.Services
{
    public interface IRenderingService
    {
        IEnumerable<JavaScriptModuleInclude> GetJavaScriptIncludes();

        IEnumerable<string> GetStyleSheetIncludes(bool includePrivateCssFiles, bool includePublicCssFiles, Type moduleDescriptorType = null);
    }
}