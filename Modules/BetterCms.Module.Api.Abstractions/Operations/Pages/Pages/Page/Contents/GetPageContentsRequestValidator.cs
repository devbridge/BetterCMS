using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    public class GetPageContentsRequestValidator : AbstractValidator<GetPageContentsRequest>
    {
        public GetPageContentsRequestValidator()
        {
            RuleFor(f => f.Data).Must(ContentTypeFieldIsReadonly).WithMessage("A ContentType field is a dynamically calculated field. You can't sort or add filter by this column.");

            RuleFor(request => request.Data.RegionIdentifier).Must(RegionIdentifierMustBeNullIfRegionIdProvided).WithMessage("A RegionIdentifier field must be null if RegionId field is provided.");
        }

        private bool ContentTypeFieldIsReadonly(GetPageContentsRequest getPageContentsRequest, GetPageContentsModel data)
        {
            return !data.HasColumnInSortBySection("ContentType") && !data.HasColumnInWhereSection("ContentType");            
        }

        private bool RegionIdentifierMustBeNullIfRegionIdProvided(GetPageContentsRequest getPageContentsRequest, string regionId)
        {
            return !(!string.IsNullOrWhiteSpace(regionId) && getPageContentsRequest.Data.RegionId.HasValue);
        }
    }
}