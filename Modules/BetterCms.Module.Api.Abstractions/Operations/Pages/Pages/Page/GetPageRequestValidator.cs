using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public class GetPageRequestValidator : AbstractValidator<GetPageRequest>
    {
        public GetPageRequestValidator()
        {
            RuleFor(request => request.PageId).Must(PageUrlMustBeNullIfPageIdProvided).WithMessage("A PageUrl field must be null if PageId is provided.");
            RuleFor(request => request.PageUrl).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A PageId or PageUrl should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetPageRequest getPageRequest, string pageUrl)
        {
            return getPageRequest.PageId != null || !string.IsNullOrEmpty(getPageRequest.PageUrl);
        }

        private bool PageUrlMustBeNullIfPageIdProvided(GetPageRequest getPageRequest, System.Guid? pageId)
        {
            return pageId != null && string.IsNullOrEmpty(getPageRequest.PageUrl) ||
                   pageId == null && !string.IsNullOrEmpty(getPageRequest.PageUrl);
        }
    }
}