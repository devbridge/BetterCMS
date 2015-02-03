using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Categories list get validator.
    /// </summary>
    public class GetCategoriesRequestValidator : AbstractValidator<GetCategoriesRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCategoriesRequestValidator"/> class.
        /// </summary>
        public GetCategoriesRequestValidator()
        {
            RuleFor(f => f.Data).Must(TagsListIsReadonly).WithMessage("An Tags field is a list. You can't sort or add filter by this column.");
        }

        /// <summary>
        /// Tags the list is readonly.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> validation passes, <c>false</c> otherwise.</returns>
        private bool TagsListIsReadonly(GetCategoriesRequest request, GetCategoriesModel data)
        {
            return !data.HasColumnInSortBySection("Tags") && !data.HasColumnInWhereSection("Tags");            
        }
    }
}
