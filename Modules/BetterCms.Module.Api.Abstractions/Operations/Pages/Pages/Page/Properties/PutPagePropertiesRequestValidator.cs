using System;

using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Put page properties request validator.
    /// </summary>
    public class PutPagePropertiesRequestValidator : AbstractValidator<PutTagRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutPagePropertiesRequestValidator"/> class.
        /// </summary>
        public PutPagePropertiesRequestValidator()
        {
            this.RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Name must be provided.");
            this.RuleFor(request => request.Data).Must(VersionMustBeProvidedIfIdIsSet).WithMessage("Version filed must be provided as positive value for item update.");
        }

        /// <summary>
        /// Versions the must be provided if identifier is set.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if version is positive when id is not empty, <c>false</c> otherwise.</returns>
        private static bool VersionMustBeProvidedIfIdIsSet(PutTagRequest request, Root.Tags.TagModel data)
        {
            return data.Id == default(Guid) || data.Version > 0;
        }
    }
}