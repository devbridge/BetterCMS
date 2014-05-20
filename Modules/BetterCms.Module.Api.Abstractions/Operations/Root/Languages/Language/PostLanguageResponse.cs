using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Language creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostLanguageResponse : SaveResponseBase
    {
    }
}