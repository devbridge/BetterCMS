using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    public class GetNodesTreeRequestValidator : AbstractValidator<GetNodesTreeRequest>
    {
        public GetNodesTreeRequestValidator()
        {
            RuleFor(request => request.CategoryTreeId).Must(CategoryTreeIdMustBeProvided).WithMessage("A CategoryTreeId field must be provided.");
        }

        private bool CategoryTreeIdMustBeProvided(GetNodesTreeRequest getPageRequest, System.Guid categoryTreeId)
        {
            return categoryTreeId != Guid.Empty;
        }
    }
}