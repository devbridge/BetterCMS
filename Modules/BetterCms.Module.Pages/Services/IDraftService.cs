using System;
using System.Security.Principal;

namespace BetterCms.Module.Pages.Services
{
    public interface IDraftService
    {
        Root.Models.Content DestroyDraftContent(Guid contentId, int version, IPrincipal principal);
    }
}