using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [DataContract]
    public class GetContentHistoryResponse : ResponseBase<System.Collections.Generic.IList<HistoryContentModel>>
    {
    }
}