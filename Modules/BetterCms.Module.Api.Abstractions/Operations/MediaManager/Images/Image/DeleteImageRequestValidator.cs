using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Delete image request validator.
    /// </summary>
    public class DeleteImageRequestValidator : AbstractValidator<DeleteImageRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteImageRequestValidator"/> class.
        /// </summary>
        public DeleteImageRequestValidator()
        {
            this.RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Id and Version must be provided.");
            this.RuleFor(request => request.ImageId).NotEmpty().WithMessage("Id field must be not null.");
            this.RuleFor(request => request.Data.Version).GreaterThan(0).WithMessage("Version filed must be not null.");
        }
    }
}