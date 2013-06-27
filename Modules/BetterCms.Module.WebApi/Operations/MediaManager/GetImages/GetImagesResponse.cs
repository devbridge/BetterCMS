using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetImages
{
    [DataContract]
    public class GetImagesResponse : ListResponseBase<MediaModel>
    {
    }
}