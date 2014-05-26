using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Request for page creation.
    /// </summary>
    [Route("/page-properties/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostPagePropertiesRequest : RequestBase<SavePagePropertiesModel>, IReturn<PostPagePropertiesResponse>
    {
    }
}
