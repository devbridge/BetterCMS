using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    [DataContract]
    [Serializable]
    public class GetImageRequest : RequestBase<GetImageModel>
    {
        [DataMember]
        public System.Guid ImageId
        {
            get; set;
        }
    }
}