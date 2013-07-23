using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [DataContract]
    public class GetImagesResponse : ListResponseBase<MediaModel>
    {
    }
}