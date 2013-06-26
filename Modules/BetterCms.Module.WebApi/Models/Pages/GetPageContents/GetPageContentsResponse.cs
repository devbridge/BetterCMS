using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPageContents
{
    [DataContract]
    public class GetPageContentsResponse : ListResponseBase<ContentModel>
    {
    }
}