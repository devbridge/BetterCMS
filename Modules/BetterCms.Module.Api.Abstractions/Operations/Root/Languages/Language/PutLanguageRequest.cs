using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutLanguageRequest : PutRequestBase<SaveLanguageModel>
    {
    }
}