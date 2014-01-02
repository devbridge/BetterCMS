using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    [DataContract]
    public class GetLanguageResponse : ResponseBase<LanguageModel>
    {
    }
}