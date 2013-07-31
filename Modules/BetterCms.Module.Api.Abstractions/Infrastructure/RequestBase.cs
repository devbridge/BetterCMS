using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    public abstract class RequestBase<TData> : IRequest where TData : new()
    {
        protected RequestBase()
        {
            Data = new TData();
        }

        [DataMember]
        public TData Data { get; set; }

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
    }
}