using System.Runtime.Serialization;

using BetterCms.Core.Api.DataContracts;

namespace BetterCms.Module.WebApi.Models
{
    [DataContract]
    public class ListResponseBase<TData> : ResponseBase<DataListResponse<TData>>
    {
    }
}