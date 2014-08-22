using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [Route("/page-exists/{PageUrl*}")]
    [DataContract]
    [Serializable]
    public class PageExistsRequest : IReturn<PageExistsResponse>
    {
        [DataMember]
        public string PageUrl { get; set; }
    }
}