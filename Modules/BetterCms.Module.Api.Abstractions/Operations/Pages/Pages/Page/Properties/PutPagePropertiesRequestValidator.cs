using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    public class PutPagePropertiesRequestValidator : AbstractValidator<PutPagePropertiesRequest>
    {
        public PutPagePropertiesRequestValidator()
        {
//            RuleFor(request => request.Data.Title).NotEmpty().WithMessage("Page title must be provided.");
            // TODO: implement.
        }
    }
}