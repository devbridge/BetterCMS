using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Sitemaps list get validator.
    /// </summary>
    public class GetSitemapsRequestValidator : AbstractValidator<GetSitemapsRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapsRequestValidator"/> class.
        /// </summary>
        public GetSitemapsRequestValidator()
        {
            RuleFor(f => f.Data).Must(TagsListIsReadonly).WithMessage("An Tags field is a list. You can't sort or add filter by this column.");
        }

        /// <summary>
        /// Tags the list is readonly.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> validation passes, <c>false</c> otherwise.</returns>
        private bool TagsListIsReadonly(GetSitemapsRequest request, GetSitemapsModel data)
        {
            return !data.HasColumnInSortBySection("Tags") && !data.HasColumnInWhereSection("Tags");            
        }
    }
}
