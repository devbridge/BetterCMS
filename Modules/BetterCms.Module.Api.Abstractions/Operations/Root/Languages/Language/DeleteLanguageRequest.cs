using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for language update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteLanguageRequest : DeleteRequestBase
    {
    }
}