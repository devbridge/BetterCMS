using BetterCms.Core.Services;
using BetterCms.Module.Pages.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft
{
    public class ContentDraftService : Service, IContentDraftService
    {
        private readonly IDraftService draftService;

        private readonly ISecurityService securityService;

        public ContentDraftService(IDraftService draftService, ISecurityService securityService)
        {
            this.draftService = draftService;
            this.securityService = securityService;
        }

        public DestroyContentDraftResponse Delete(DestroyContentDraftRequest request)
        {
            var version = request.Data != null ? request.Data.Version : 0;
            draftService.DestroyDraftContent(request.Id, version, securityService.GetCurrentPrincipal());

            return new DestroyContentDraftResponse { Data = true };
        }
    }
}