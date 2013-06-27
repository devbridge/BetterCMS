using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetFiles
{
    [DataContract]
    public class GetFilesResponse : ListResponseBase<MediaModel>
    {
    }
}