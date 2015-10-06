using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    [DataContract]
    [Serializable]
    public class GetLayoutOptionsRequest : RequestBase<DataOptions>
    {
        [DataMember]
        public Guid LayoutId { get; set; }
    }
}
