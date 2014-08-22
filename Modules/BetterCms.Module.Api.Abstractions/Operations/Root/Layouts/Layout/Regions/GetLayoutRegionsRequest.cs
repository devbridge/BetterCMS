using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [Route("/layouts/{LayoutId}/regions", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetLayoutRegionsRequest : RequestBase<DataOptions>, IReturn<GetLayoutRegionsResponse>
    {
        [DataMember]
        public System.Guid LayoutId
        {
            get; set;            
        }
    }
}