using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [Route("/page-properties/{PageId}", Verbs = "GET")]
    [Route("/page-properties/by-url/{PageUrl*}", Verbs = "GET")]
    [DataContract]
    public class GetPagePropertiesRequest : RequestBase<GetPagePropertiesModel>, IReturn<GetPagePropertiesResponse>
    {
        [DataMember]
        public System.Guid? PageId
        {
            get
            {
                return Data.PageId;
            }
            set
            {
                Data.PageId = value;
            }
        }

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