using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class RequestBase<TData> : IRequest where TData : new()
    {
        protected RequestBase()
        {
            Data = new TData();
            User = new ApiIdentity();
        }

        [DataMember]
        public TData Data { get; set; }
        
        [DataMember]
        public ApiIdentity User { get; set; }

        object IRequest.Data
        {
            get
            {
                return Data;
            }
            set
            {
                Data = (TData)value;
            }
        }
    }
    
    public interface IRequest
    {
        object Data { get; set; }

        ApiIdentity User { get; set; }
    }
}