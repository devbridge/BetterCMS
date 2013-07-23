using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [DataContract]
    public class GetFilesResponse : ListResponseBase<MediaModel>
    {
    }
}