using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    public class GetPagesRequestValidator : AbstractValidator<GetPagesRequest>
    {
        public GetPagesRequestValidator()
        {
            RuleFor(f => f.Data).Must(OptionsListIsReadonly).WithMessage("An Options field is a list. You can't sort or add filter by this column.");
            RuleFor(f => f.Data).Must(TagsListIsReadonly).WithMessage("An Tags field is a list. You can't sort or add filter by this column.");
        }

        private bool OptionsListIsReadonly(GetPagesRequest request, GetPagesModel data)
        {
            return !data.HasColumnInSortBySection("Options") && !data.HasColumnInWhereSection("Options");            
        }

        private bool TagsListIsReadonly(GetPagesRequest request, GetPagesModel data)
        {
            return !data.HasColumnInSortBySection("Tags") && !data.HasColumnInWhereSection("Tags");            
        }
    }
}
