using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Sitemap.GetPageTranslations
{
    public class GetPageTranslationsCommand : CommandBase, ICommand<Guid, List<PageTranslationViewModel>>
    {
        public List<PageTranslationViewModel> Execute(Guid pageId)
        {
            if (!pageId.HasDefaultValue())
            {
                var page = Repository.FirstOrDefault<Root.Models.Page>(p => p.Id == pageId);
                if (page != null)
                {
                    if (page.LanguageGroupIdentifier == null)
                    {
                        return new List<PageTranslationViewModel>()
                            {
                                new PageTranslationViewModel
                                    {
                                        Id = page.Id,
                                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                                        Title = page.Title,
                                        PageUrl = page.PageUrl,
                                        PageUrlHash = page.PageUrlHash
                                    }
                            };
                    }

                    var translations =
                        Repository.AsQueryable<Root.Models.Page>().Where(p => p.LanguageGroupIdentifier == page.LanguageGroupIdentifier).Distinct().ToList();

                    return
                        translations.Select(
                            t =>
                            new PageTranslationViewModel
                                {
                                    Id = t.Id,
                                    LanguageId = t.Language != null ? t.Language.Id : (Guid?)null,
                                    Title = t.Title,
                                    PageUrl = t.PageUrl,
                                    PageUrlHash = t.PageUrlHash
                                }).ToList();
                }
            }

            return null;
        }
    }
}