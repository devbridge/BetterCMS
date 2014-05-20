using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page save request for REST.
    /// </summary>
    [Route("/page-properties/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutPagePropertiesRequest : PutRequestBase<SavePagePropertiesModel>, IReturn<PutPagePropertiesResponse>
    {
    }
}