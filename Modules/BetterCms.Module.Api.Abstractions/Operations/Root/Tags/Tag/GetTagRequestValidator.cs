using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    public class GetTagRequestValidator : AbstractValidator<GetTagRequest>
    {
        public GetTagRequestValidator()
        {
            RuleFor(request => request.TagId).Must(TagNameMustBeNullIfTagIdProvided).WithMessage("A TagName field must be null if TagId is provided.");
            RuleFor(request => request.TagName).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A TagId or TagName should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetTagRequest getTagRequest, string tagName)
        {
            return getTagRequest.TagId != null || !string.IsNullOrEmpty(getTagRequest.TagName);
        }

        private bool TagNameMustBeNullIfTagIdProvided(GetTagRequest getTagRequest, Guid? tagId)
        {
            return tagId != null && string.IsNullOrEmpty(getTagRequest.TagName) ||
                   tagId == null && !string.IsNullOrEmpty(getTagRequest.TagName);
        }
    }
}