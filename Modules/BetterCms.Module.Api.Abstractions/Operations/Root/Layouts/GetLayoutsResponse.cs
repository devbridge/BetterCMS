using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [DataContract]
    [Serializable]
    public class GetLayoutsResponse : ListResponseBase<LayoutModel>
    {
    }
}