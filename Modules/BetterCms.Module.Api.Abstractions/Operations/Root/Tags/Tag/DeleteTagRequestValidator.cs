using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Delete tag request validator.
    /// </summary>
    public class DeleteTagRequestValidator : AbstractValidator<DeleteTagRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTagRequestValidator"/> class.
        /// </summary>
        public DeleteTagRequestValidator()
        {
            RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Id and Version must be provided.");
            RuleFor(request => request.Data.Id).NotEmpty().WithMessage("Id field must be not null.");
            RuleFor(request => request.Data.Version).GreaterThan(0).WithMessage("Version filed must be not null.");
        }
    }
}