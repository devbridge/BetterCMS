using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Sitemap.GetPageUrlForLanguage
{
    public class GetPageUrlForLanguageCommand : CommandBase, ICommand<GetPageUrlForLanguageCommandRequest, string>
    {
        public string Execute(GetPageUrlForLanguageCommandRequest request)
        {
            if (!request.PageId.HasDefaultValue())
            {
                var page = Repository.FirstOrDefault<Root.Models.Page>(p => p.Id == request.PageId);
                if (page != null)
                {
                    if (request.LanguageId.HasDefaultValue())
                    {
                        if (page.Language == null)
                        {
                            return page.PageUrl;
                        }

                        var pageWithoutLanguage =
                            Repository.AsQueryable<Root.Models.Page>()
                                      .FirstOrDefault(p => p.Language == null && p.LanguageGroupIdentifier == page.LanguageGroupIdentifier);

                        if (pageWithoutLanguage != null)
                        {
                            return pageWithoutLanguage.PageUrl;
                        }
                    }

                    if (page.Language != null && page.Language.Id == request.LanguageId)
                    {
                        return page.PageUrl;
                    }

                    var pageWithLanguage =
                        Repository.AsQueryable<Root.Models.Page>()
                                    .FirstOrDefault(p => p.Language.Id == request.LanguageId && p.LanguageGroupIdentifier == page.LanguageGroupIdentifier);

                    if (pageWithLanguage != null)
                    {
                        return pageWithLanguage.PageUrl;
                    }

                    if (page.Language == null)
                    {
                        return page.PageUrl;
                    }

                    var pageWithDefaultLanguage =
                        Repository.AsQueryable<Root.Models.Page>()
                                  .FirstOrDefault(p => p.Language == null && p.LanguageGroupIdentifier == page.LanguageGroupIdentifier);

                    if (pageWithDefaultLanguage != null)
                    {
                        return pageWithDefaultLanguage.PageUrl;
                    }
                }
            }

            return null;
        }
    }
}