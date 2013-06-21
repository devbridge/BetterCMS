using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPages
{
    [DataContract]
    public class GetPagesResponse : ListResponseBase<PageModel>
    {
    }
}