using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Request for category tree creation.
    /// </summary>
    [Route("/categorytrees", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostCategoryTreeRequest : RequestBase<SaveCategoryTreeModel>, IReturn<PostCategoryTreeResponse>
    {
    }
}