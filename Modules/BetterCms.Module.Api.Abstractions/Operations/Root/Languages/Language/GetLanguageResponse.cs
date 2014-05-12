using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    [DataContract]
    [Serializable]
    public class GetLanguageResponse : ResponseBase<LanguageModel>
    {
    }
}