using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    [Serializable]
    public class GetLayoutRequest : RequestBase<GetLayoutModel>
    {
        [DataMember]
        public Guid LayoutId { get; set; }
    }
}