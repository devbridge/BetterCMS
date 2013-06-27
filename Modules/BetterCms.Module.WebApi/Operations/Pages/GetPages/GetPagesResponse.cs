using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPages
{
    [DataContract]
    public class GetPagesResponse : ListResponseBase<PageModel>
    {
    }
}