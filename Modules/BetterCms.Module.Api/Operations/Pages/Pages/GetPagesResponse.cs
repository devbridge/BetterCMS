using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    public class GetPagesResponse : ListResponseBase<PageModel>
    {
    }
}