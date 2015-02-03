using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    public class GetCategoryNodesRequestValidator : AbstractValidator<GetCategoryNodesRequest>
    {
        public GetCategoryNodesRequestValidator()
        {
            RuleFor(request => request.CategoryTreeId).Must(CategoryTreeIdMustBeProvided).WithMessage("A CategoryTreeId field must be provided.");
        }

        private bool CategoryTreeIdMustBeProvided(GetCategoryNodesRequest getPageRequest, Guid categoryTreeId)
        {
            return categoryTreeId != Guid.Empty;
        }
    }
}