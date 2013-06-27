using System.Web.Mvc;

using BetterCms.Core.Mvc.Commands;

namespace BetterCms.Module.Api.Helpers
{
    public abstract class CmsApiControllerBase : Controller
    {
        public virtual ICommandResolver CommandResolver { get; set; }
    }
}