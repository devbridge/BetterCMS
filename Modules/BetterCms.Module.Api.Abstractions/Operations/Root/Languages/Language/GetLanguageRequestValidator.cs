using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    public class GetLanguageRequestValidator : AbstractValidator<GetLanguageRequest>
    {
        public GetLanguageRequestValidator()
        {
            RuleFor(request => request.LanguageId).Must(LanguageCodeMustBeNullIfLanguageIdProvided).WithMessage("A LanguageCode field must be null if LanguageId is provided.");
            RuleFor(request => request.LanguageCode).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A LanguageId or LanguageCode should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetLanguageRequest getLanguageRequest, string languageCode)
        {
            return getLanguageRequest.LanguageId != null || !string.IsNullOrEmpty(getLanguageRequest.LanguageCode);
        }

        private bool LanguageCodeMustBeNullIfLanguageIdProvided(GetLanguageRequest getLanguageRequest, Guid? languageId)
        {
            return languageId != null && string.IsNullOrEmpty(getLanguageRequest.LanguageCode) ||
                   languageId == null && !string.IsNullOrEmpty(getLanguageRequest.LanguageCode);
        }
    }
}