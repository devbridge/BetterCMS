using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    public class GetCategoryRequestValidator : AbstractValidator<GetCategoryRequest>
    {
        public GetCategoryRequestValidator()
        {
            RuleFor(request => request.CategoryId).Must(CategoryNameMustBeNullIfCategoryIdProvided).WithMessage("A CategoryName field must be null if CategoryId is provided.");
            RuleFor(request => request.CategoryName).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A CategoryId or CategoryName should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetCategoryRequest getCategoryRequest, string categoryName)
        {
            return getCategoryRequest.CategoryId != null || !string.IsNullOrEmpty(getCategoryRequest.CategoryName);
        }

        private bool CategoryNameMustBeNullIfCategoryIdProvided(GetCategoryRequest getCategoryRequest, System.Guid? categoryId)
        {
            return categoryId != null && string.IsNullOrEmpty(getCategoryRequest.CategoryName) ||
                   categoryId == null && !string.IsNullOrEmpty(getCategoryRequest.CategoryName);
        }
    }
}