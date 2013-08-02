using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [DataContract]
    public class GetContentHistoryResponse : ResponseBase<System.Collections.Generic.IList<HistoryContentModel>>
    {
    }
}