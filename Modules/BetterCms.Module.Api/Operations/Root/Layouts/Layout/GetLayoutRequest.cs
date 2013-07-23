using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [Route("/layouts/{LayoutId}", Verbs = "GET")]
    [DataContract]
    public class GetLayoutRequest : RequestBase<GetLayoutModel>, IReturn<GetLayoutResponse>
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