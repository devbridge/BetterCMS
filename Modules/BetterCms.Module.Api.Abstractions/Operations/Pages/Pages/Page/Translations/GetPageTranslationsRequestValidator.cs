using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    public class GetPageTranslationsRequestValidator : AbstractValidator<GetPageTranslationsRequest>
    {
        public GetPageTranslationsRequestValidator()
        {
            RuleFor(request => request.PageId).Must(PagePropertiesNameMustBeNullIfPagePropertiesIdProvided).WithMessage("A PageUrl field must be null if PageId is provided.");
            RuleFor(request => request.PageUrl).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A PageId or PageUrl should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetPageTranslationsRequest request, string pageUrl)
        {
            return request.PageId != null || !string.IsNullOrEmpty(request.PageUrl);
        }

        private bool PagePropertiesNameMustBeNullIfPagePropertiesIdProvided(GetPageTranslationsRequest request, System.Guid? pageId)
        {
            return pageId != null && string.IsNullOrEmpty(request.PageUrl) ||
                   pageId == null && !string.IsNullOrEmpty(request.PageUrl);
        }
    }
}