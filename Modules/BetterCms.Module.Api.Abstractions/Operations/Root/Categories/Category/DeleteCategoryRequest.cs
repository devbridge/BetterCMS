using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Category delete request.
    /// </summary>
    [Route("/categories/{Id}", Verbs = "DELETE")]
    [Serializable]
    [DataContract]
    public class DeleteCategoryRequest : DeleteRequestBase, IReturn<DeleteCategoryResponse>
    {
    }
}