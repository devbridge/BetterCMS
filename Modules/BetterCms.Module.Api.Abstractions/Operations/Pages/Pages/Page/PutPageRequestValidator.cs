using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public class PutPageRequestValidator : AbstractValidator<PutPageRequest>
    {
        public PutPageRequestValidator()
        {
//            RuleFor(request => request.Data.Title).NotEmpty().WithMessage("Page title must be provided.");
            // TODO: implement.
        }
    }
}