using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    /// <summary>
    /// Request for page creation.
    /// </summary>
    [Route("/pages", Verbs = "POST")]
    [DataContract]
    public class PostPageRequest : RequestBase<SavePagePropertiesModel>, IReturn<PostPageResponse>
    {
    }
}
