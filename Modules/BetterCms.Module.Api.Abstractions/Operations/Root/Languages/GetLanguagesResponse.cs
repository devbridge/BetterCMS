using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    [DataContract]
    public class GetLanguagesResponse : ListResponseBase<LanguageModel>
    {
    }
}