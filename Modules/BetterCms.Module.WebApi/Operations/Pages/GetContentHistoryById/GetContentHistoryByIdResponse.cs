using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetContentHistoryById
{
    [DataContract]
    public class GetPageContentHistoryByPageContentIdResponse : ListResponseBase<HistoryContentModel>
    {
    }
}