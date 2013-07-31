using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    public class GetPagesResponse : ListResponseBase<PageModel>
    {
    }
}