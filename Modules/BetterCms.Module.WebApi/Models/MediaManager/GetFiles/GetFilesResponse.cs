using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetFiles
{
    [DataContract]
    public class GetFilesResponse : ListResponseBase<MediaModel>
    {
    }
}