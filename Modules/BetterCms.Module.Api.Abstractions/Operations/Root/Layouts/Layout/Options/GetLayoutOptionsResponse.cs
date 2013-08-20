using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    [DataContract]
    public class GetLayoutOptionsResponse : ListResponseBase<OptionModel>
    {
    }
}
