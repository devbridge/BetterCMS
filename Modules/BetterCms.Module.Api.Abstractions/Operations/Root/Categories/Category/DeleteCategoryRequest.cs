using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request for category update or creation.
    /// </summary>
    [Route("/categories/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteCategoryRequest : DeleteRequestBase, IReturn<DeleteCategoryResponse>
    {
    }
}