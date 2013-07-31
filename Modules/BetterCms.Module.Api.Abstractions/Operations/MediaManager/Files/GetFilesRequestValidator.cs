using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class GetFilesRequestValidator : AbstractValidator<GetFilesRequest>
    {
        public GetFilesRequestValidator()
        {
            RuleFor(request => request.Data.IncludeFolders).Must(AtLeastOneFieldShouldBeProvided).WithMessage("At least one of: IncludeFolders and IncludeFiles should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetFilesRequest request, bool includeFolders)
        {
            return request.Data.IncludeFiles || includeFolders;
        }
    }
}