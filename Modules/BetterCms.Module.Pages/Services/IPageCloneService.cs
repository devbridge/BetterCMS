using System.Collections.Generic;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface IPageCloneService
    {
        PageProperties ClonePage(System.Guid pageId, string pageTitle, string pageUrl, IEnumerable<IAccessRule> userAccessList, bool cloneAsMasterPage);

        PageProperties ClonePageWithCulture(System.Guid pageId, string pageTitle, string pageUrl, IEnumerable<IAccessRule> userAccessList, System.Guid cultureId, System.Guid cultureGroupIdentifier);
    }
}