using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    public class GetPagePropertiesRequestValidator : AbstractValidator<GetPagePropertiesRequest>
    {
        public GetPagePropertiesRequestValidator()
        {
            RuleFor(request => request.PageId).Must(PagePropertiesNameMustBeNullIfPagePropertiesIdProvided).WithMessage("A PageUrl field must be null if PageId is provided.");
            RuleFor(request => request.PageUrl).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A PageId or PageUrl should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetPagePropertiesRequest getPagePropertiesRequest, string pageUrl)
        {
            return getPagePropertiesRequest.PageId != null || !string.IsNullOrEmpty(getPagePropertiesRequest.PageUrl);
        }

        private bool PagePropertiesNameMustBeNullIfPagePropertiesIdProvided(GetPagePropertiesRequest getPagePropertiesRequest, System.Guid? pageId)
        {
            return pageId != null && string.IsNullOrEmpty(getPagePropertiesRequest.PageUrl) ||
                   pageId == null && !string.IsNullOrEmpty(getPagePropertiesRequest.PageUrl);
        }
    }
}