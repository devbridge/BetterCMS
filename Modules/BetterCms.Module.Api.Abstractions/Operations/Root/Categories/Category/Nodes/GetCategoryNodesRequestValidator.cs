using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    public class GetCategoryNodesRequestValidator : AbstractValidator<GetCategoryNodesRequest>
    {
        public GetCategoryNodesRequestValidator()
        {
            RuleFor(request => request.CategoryId).Must(CategoryIdMustBeProvided).WithMessage("A CategoryId field must be provided.");
        }

        private bool CategoryIdMustBeProvided(GetCategoryNodesRequest getPageRequest, Guid categoryId)
        {
            return categoryId != Guid.Empty;
        }
    }
}