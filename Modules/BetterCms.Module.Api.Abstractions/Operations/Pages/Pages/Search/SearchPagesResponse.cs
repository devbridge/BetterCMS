using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    [DataContract]
    public class SearchPagesResponse : ListResponseBase<SearchResultModel>
    {
    }
}