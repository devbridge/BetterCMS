using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Category delete request.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteCategoryTreeRequest : DeleteRequestBase
    {
    }
}