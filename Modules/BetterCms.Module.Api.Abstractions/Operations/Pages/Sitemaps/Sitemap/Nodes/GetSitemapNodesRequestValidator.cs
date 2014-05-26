using System;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    public class GetSitemapNodesRequestValidator : AbstractValidator<GetSitemapNodesRequest>
    {
        public GetSitemapNodesRequestValidator()
        {
            RuleFor(request => request.SitemapId).Must(SitemapIdMustBeProvided).WithMessage("A SitemapId field must be provided.");
        }

        private bool SitemapIdMustBeProvided(GetSitemapNodesRequest getPageRequest, Guid sitemapId)
        {
            return sitemapId != Guid.Empty;
        }
    }
}