using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Response after language saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutLanguageResponse : SaveResponseBase
    {
    }
}