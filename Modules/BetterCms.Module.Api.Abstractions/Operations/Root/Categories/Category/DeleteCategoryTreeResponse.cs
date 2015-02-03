using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Category delete response.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteCategoryTreeResponse : DeleteResponseBase
    {
    }
}