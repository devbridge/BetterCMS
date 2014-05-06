using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    public abstract class PutRequestBase<TData> : RequestBase<TData>
        where TData : new()
    {
        internal bool CreateOnly { get; set; }
    }
}