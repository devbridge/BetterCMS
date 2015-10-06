using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Request for getting category trees list.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetCategoryTreesRequest : RequestBase<GetCategoryTreesModel>
    {
    }
}