using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree
{
    public class GetSitemapTreeRequestValidator : AbstractValidator<GetSitemapTreeRequest>
    {
        public GetSitemapTreeRequestValidator()
        {
            RuleFor(request => request.SitemapId).Must(SitemapIdMustBeProvided).WithMessage("A SitemapId field must be provided.");
        }

        private bool SitemapIdMustBeProvided(GetSitemapTreeRequest getPageRequest, System.Guid sitemapId)
        {
            return sitemapId != Guid.Empty;
        }
    }
}