using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    public abstract class ResponseBase<TData>
    {
        [DataMember]
        public TData Data { get; set; }
    }
}