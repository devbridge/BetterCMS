using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPageContents
{
    [DataContract]
    public class GetPageContentsResponse : ListResponseBase<PageContentModel>
    {
    }
}