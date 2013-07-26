using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    public class GetImagesRequestValidator : AbstractValidator<GetImagesRequest>
    {
        public GetImagesRequestValidator()
        {
            RuleFor(request => request.Data.IncludeFolders).Must(AtLeastOneFieldShouldBeProvided).WithMessage("At least one of: IncludeFolders and IncludeImages should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetImagesRequest request, bool includeFolders)
        {
            return request.Data.IncludeImages || includeFolders;
        }
    }
}