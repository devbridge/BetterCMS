using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [Route("/layouts/{LayoutId}/regions", Verbs = "GET")]
    [DataContract]
    public class GetLayoutRegionsRequest : RequestBase<GetLayoutRegionsModel>, IReturn<GetLayoutRegionsResponse>
    {
        [DataMember]
        public System.Guid LayoutId
        {
            get
            {
                return Data.LayoutId;
            }
            set
            {
                Data.LayoutId = value;
            }
        }
    }
}