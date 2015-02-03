using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Data model for getting categories list.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetCategoriesModel : DataOptions
    {
    }
}