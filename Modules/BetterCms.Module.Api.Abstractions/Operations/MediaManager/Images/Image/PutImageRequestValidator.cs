using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Put image request validator.
    /// </summary>
    public class PutImageRequestValidator : AbstractValidator<PutImageRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutImageRequestValidator"/> class.
        /// </summary>
        public PutImageRequestValidator()
        {
            this.RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Name must be provided.");
            this.RuleFor(request => request.Data.Caption).NotEmpty().WithMessage("Caption filed must be provided.");
            this.RuleFor(request => request.Data.FileUri).NotEmpty().WithMessage("FileUri filed must be provided.");
            this.RuleFor(request => request.Data.OriginalUri).NotEmpty().WithMessage("OriginalUri filed must be provided.");
            this.RuleFor(request => request.Data.ThumbnailUri).NotEmpty().WithMessage("ThumbnailUri filed must be provided.");
            this.RuleFor(request => request.Data).Must(VersionMustBeProvidedIfIdIsSet).WithMessage("Version filed must be provided as positive value for item update.");
        }

        /// <summary>
        /// Versions the must be provided if identifier is set.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if version is positive when id is not empty, <c>false</c> otherwise.</returns>
        private static bool VersionMustBeProvidedIfIdIsSet(PutImageRequest request, ImageModel data)
        {
            return data.Id == default(Guid) || data.Version > 0;
        }
    }
}