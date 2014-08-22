using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class ResponseBase<TData>
    {
        [DataMember]
        public TData Data { get; set; }
    }
}