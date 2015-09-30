using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    public class PageExistsController : ApiController, IPageExistsService
    {
        private readonly IRepository repository;

        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageExistsController" /> class.
        /// </summary>
        public PageExistsController(IRepository repository, IUrlService urlService)
        {
            this.repository = repository;
            this.urlService = urlService;
        }
        [Route("bcms-api/page-exists/{PageUrl}")]
        public PageExistsResponse Get([ModelBinder(typeof(JsonModelBinder))] PageExistsRequest request)
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