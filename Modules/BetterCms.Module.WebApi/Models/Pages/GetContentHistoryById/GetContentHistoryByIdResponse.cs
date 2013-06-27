using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetContentHistoryById
{
    [DataContract]
    public class GetPageContentHistoryByPageContentIdResponse : ListResponseBase<HistoryContentModel>
    {
    }
}