using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [DataContract]
    public class GetImagesResponse : ListResponseBase<MediaModel>
    {
    }
}