using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Post tag request validator.
    /// </summary>
    public class PostTagsRequestValidator : AbstractValidator<PostTagsRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostTagsRequestValidator"/> class.
        /// </summary>
        public PostTagsRequestValidator()
        {
            RuleFor(request => request.Data).NotNull().WithMessage("Data object with filled Name must be provided.");
            RuleFor(request => request.Data.Name).NotEmpty().WithMessage("Name filed must be provided.");
            RuleFor(request => request.Data).Must(VersionMustBeProvidedIfIdIsSet).WithMessage("Version filed must be provided as positive value for item update.");
        }

        /// <summary>
        /// Versions the must be provided if identifier is set.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if version is positive when id is not empty, <c>false</c> otherwise.</returns>
        private static bool VersionMustBeProvidedIfIdIsSet(PostTagsRequest request, TagModel data)
        {
            return data.Id == default(Guid) || data.Version > 0;
        }
    }
}