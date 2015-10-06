using System.Web.Http;

using BetterCms.Core.Services;
using BetterCms.Module.Api.ApiExtensions;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Pages.Services;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft
{
    public class ContentDraftController : ApiController, IContentDraftService
    {
        private readonly IDraftService draftService;

        private readonly ISecurityService securityService;

        public ContentDraftController(IDraftService draftService, ISecurityService securityService)
        {
            this.draftService = draftService;
            this.securityService = securityService;
        }

        [Route("bcms-api/contents/{Id}/draft")]
        [UrlPopulator]
        public DestroyContentDraftResponse Delete(DestroyContentDraftRequest request)
        {
            var version = request.Data != null ? request.Data.Version : 0;
            draftService.DestroyDraftContent(request.Id, version, securityService.GetCurrentPrincipal());

            return new DestroyContentDraftResponse { Data = true };
        }
    }
}