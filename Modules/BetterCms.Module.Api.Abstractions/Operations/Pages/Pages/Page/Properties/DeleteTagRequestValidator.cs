using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Delete page properties request validator.
    /// </summary>
    public class DeletePagePropertiesRequestValidator : AbstractValidator<DeletePagePropertiesRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePagePropertiesRequestValidator"/> class.
        /// </summary>
        public DeletePagePropertiesRequestValidator()
        {
            RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Id and Version must be provided.");
            RuleFor(request => request.Data.Id).NotEmpty().WithMessage("Id field must be not null.");
            RuleFor(request => request.Data.Version).GreaterThan(0).WithMessage("Version filed must be not null.");
        }
    }
}