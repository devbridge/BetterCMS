using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Response for language delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteLanguageResponse : DeleteResponseBase
    {
    }
}