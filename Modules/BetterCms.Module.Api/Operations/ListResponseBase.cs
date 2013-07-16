using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    public abstract class ListResponseBase<TData> : ResponseBase<DataListResponse<TData>>
    {
    }
}