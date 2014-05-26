using System;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public class GetSitemapsRequestValidator : AbstractValidator<GetSitemapsRequest>
    {
        public GetSitemapsRequestValidator()
        {
            RuleFor(f => f.Data).Must(TagsListIsReadonly).WithMessage("An Tags field is a list. You can't sort or add filter by this column.");
        }

        private bool TagsListIsReadonly(GetSitemapsRequest request, GetSitemapsModel data)
        {
            return !data.HasColumnInSortBySection("Tags") && !data.HasColumnInWhereSection("Tags");            
        }
    }
}
