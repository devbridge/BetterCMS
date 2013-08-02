using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [DataContract]
    public class GetMediaTreeResponse : ResponseBase<MediaTreeModel>
    {
    }
}