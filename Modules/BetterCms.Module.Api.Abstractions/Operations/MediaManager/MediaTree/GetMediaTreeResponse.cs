using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [DataContract]
    [Serializable]
    public class GetMediaTreeResponse : ResponseBase<MediaTreeModel>
    {
    }
}