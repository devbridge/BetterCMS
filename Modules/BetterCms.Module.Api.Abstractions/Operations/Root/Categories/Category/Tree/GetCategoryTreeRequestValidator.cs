using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    public class GetCategoryTreeRequestValidator : AbstractValidator<GetCategoryTreeRequest>
    {
        public GetCategoryTreeRequestValidator()
        {
            RuleFor(request => request.CategoryId).Must(CategoryIdMustBeProvided).WithMessage("A CategoryId field must be provided.");
        }

        private bool CategoryIdMustBeProvided(GetCategoryTreeRequest getPageRequest, System.Guid categoryId)
        {
            return categoryId != Guid.Empty;
        }
    }
}