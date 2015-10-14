using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request to get category data.
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetCategoryTreeRequest : RequestBase<GetCategoryTreeModel>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}