using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [DataContract]
    [Serializable]
    public class GetLayoutRegionsRequest : RequestBase<DataOptions>
    {
        [DataMember]
        public System.Guid LayoutId
        {
            get; set;            
        }
    }
}