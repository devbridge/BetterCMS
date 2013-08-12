using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    [DataContract]
    public class GetPageContentOptionsResponse : ListResponseBase<OptionModel>
    {
    }
}
