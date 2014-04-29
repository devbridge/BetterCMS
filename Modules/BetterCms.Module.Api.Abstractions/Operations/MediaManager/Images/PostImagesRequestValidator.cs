using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// Post image request validator.
    /// </summary>
    public class PostImagesRequestValidator : AbstractValidator<PostImagesRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostImagesRequestValidator"/> class.
        /// </summary>
        public PostImagesRequestValidator()
        {
            // TODO: update validation for all necessary fields.
            RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Name must be provided.");
            RuleFor(request => request.Data.Caption).NotEmpty().WithMessage("Caption filed must be provided.");
            RuleFor(request => request.Data).Must(VersionMustBeProvidedIfIdIsSet).WithMessage("Version filed must be provided as positive value for item update.");
        }

        /// <summary>
        /// Versions the must be provided if identifier is set.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if version is positive when id is not empty, <c>false</c> otherwise.</returns>
        private static bool VersionMustBeProvidedIfIdIsSet(PostImagesRequest request, Image.ImageModel data)
        {
            return data.Id == default(Guid) || data.Version > 0;
        }
    }
}