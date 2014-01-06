using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    [DataContract]
    public class GetPageTranslationsResponse : ListResponseBase<PageTranslationModel>
    {
    }
}