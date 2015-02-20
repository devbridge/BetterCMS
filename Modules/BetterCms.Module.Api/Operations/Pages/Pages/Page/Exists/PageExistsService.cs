using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    public class PageExistsService : Service, IPageExistsService
    {
        private readonly IRepository repository;

        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageExistsService" /> class.
        /// </summary>
        public PageExistsService(IRepository repository, IUrlService urlService)
        {
            this.repository = repository;
            this.urlService = urlService;
        }

        public PageExistsResponse Get(PageExistsRequest request)
        {
            var url = urlService.FixUrl(request.PageUrl);

            var id = repository
                .AsQueryable<Module.Root.Models.Page>(p => p.PageUrlHash == url.UrlHash())
                .Select(p => p.Id)
                .FirstOrDefault();

            return new PageExistsResponse
                       {
                           Data = new PageModel
                                      {
                                          Exists = !id.HasDefaultValue(),
                                          PageId = !id.HasDefaultValue() ? id : (System.Guid?)null
                                      }
                       };
        }
    }
}