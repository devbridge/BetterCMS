using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Response for page properties delete operation.
    /// </summary>
    [DataContract]
    public class DeletePagePropertiesResponse : ResponseBase<bool>
    {
    }
}