using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    /// <summary>
    /// Request for getting a list of categorizable items
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetCategorizableItemsRequest : RequestBase<DataOptions>
    {
    }
}
