using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    public abstract class ListResponseBase<TData> : ResponseBase<DataListResponse<TData>>
    {
    }
}