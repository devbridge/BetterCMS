using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPageContentHistoryByPageContentId
{
    [DataContract]
    public class GetPageContentHistoryByPageContentIdResponse : ListResponseBase<HistoryModel>
    {
    }
}