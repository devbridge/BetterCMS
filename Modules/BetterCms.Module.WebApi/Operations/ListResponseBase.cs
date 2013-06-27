using System.Runtime.Serialization;

using BetterCms.Core.Api.DataContracts;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    public abstract class ListResponseBase<TData> : ResponseBase<DataListResponse<TData>>
    {
    }
}