using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    public class GetWidgetsRequestValidator : AbstractValidator<GetWidgetsRequest>
    {
        public GetWidgetsRequestValidator()
        {
            RuleFor(f => f.Data).Must(ContentTypeFieldIsReadonly).WithMessage("A WidgetType field is a dynamically calculated field. You can't sort or add filter by this column.");
        }

        private bool ContentTypeFieldIsReadonly(GetWidgetsRequest getPageContentsRequest, GetWidgetsModel data)
        {
            return !data.HasColumnInSortBySection("WidgetType") && !data.HasColumnInWhereSection("WidgetType");            
        } 
    }
}