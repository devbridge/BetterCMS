using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    [DataContract]
    [Serializable]
    public class GetCategorizableItemsResponse : ListResponseBase<CategorizableItemModel>
    {
    }
}
