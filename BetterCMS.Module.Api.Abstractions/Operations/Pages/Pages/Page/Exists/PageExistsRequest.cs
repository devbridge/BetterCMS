using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [Route("/page-exists/{PageUrl*}")]
    [DataContract]
    public class PageExistsRequest : RequestBase<PageExistsModel>, IReturn<PageExistsResponse>
    {
        [DataMember]
        public string PageUrl
        {
            get
            {
                return Data.PageUrl;
            }
            set
            {
                Data.PageUrl = value;
            }
        }
    }
}