using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.GetPageForCloningWithLanguage
{
    public class GetPageForCloningWithLanguageCommand : CommandBase, ICommand<GetPageForCloningWithLanguageCommandRequest, ClonePageWithLanguageViewModel>
    {
        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The language service
        /// </summary>
        private readonly ILanguageService languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPageForCloningWithLanguageCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="languageService">The language service.</param>
        public GetPageForCloningWithLanguageCommand(ICmsConfiguration cmsConfiguration, ILanguageService languageService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.languageService = languageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ClonePageWithLanguageViewModel Execute(GetPageForCloningWithLanguageCommandRequest request)
        {
            var pageFutureQuery = Repository
                .AsQueryable<PageProperties>()
                .Where(p => p.Id == request.PageId && !p.IsDeleted)
                .Select(p =>
                    new
                    {
                        Model = new ClonePageWithLanguageViewModel
                            {
                                PageId = p.Id,
                                IsMasterPage = p.IsMasterPage
                            },
                        LanguageGroupIdentifier = p.LanguageGroupIdentifier,
                        LanguageId = p.Language != null ? p.Language.Id : (System.Guid?) null
                    })
                .ToFuture();

            var languagesFuture = languageService.GetLanguagesLookupValues();
            var result = pageFutureQuery.FirstOne();
            var model = result.Model;
            model.Languages = languagesFuture.ToList();
            model.ShowWarningAboutNoCultures = !model.Languages.Any();

            if (model.IsMasterPage)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            IList<UserAccessViewModel> accessRules;
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                accessRules = Repository
                    .AsQueryable<Root.Models.Page>()
                    .Where(x => x.Id == request.PageId && !x.IsDeleted)
                    .SelectMany(x => x.AccessRules)
                    .OrderBy(x => x.Identity)
                    .ToFuture()
                    .ToList()
                    .Select(x => new UserAccessViewModel(x))
                    .ToList();
            }
            else
            {
                accessRules = null;
            }

            model.AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled;
            model.UserAccessList = accessRules;

            AddRemoveLanguages(model.Languages, result.LanguageGroupIdentifier, result.LanguageId);

            return model;
        }

        private void AddRemoveLanguages(List<LookupKeyValue> languages, System.Guid? languageGroupIdentifier, System.Guid? pageLanguageId)
        {
            var existingLanguages = new List<System.Guid?>();
            if (languageGroupIdentifier.HasValue)
            {
                existingLanguages = Repository
                    .AsQueryable<Root.Models.Page>(p => p.LanguageGroupIdentifier == languageGroupIdentifier.Value)
                    .Select(p => p.Language != null ? p.Language.Id : (System.Guid?)null)
                    .ToArray()
                    .Concat(existingLanguages)
                    .ToList();
            }
            else
            {
                existingLanguages.Add(pageLanguageId);
            }

            foreach (var languageId in existingLanguages)
            {
                var language = languages.FirstOrDefault(c => c.Key == languageId.ToString().ToLowerInvariant());
                if (language != null)
                {
                    languages.Remove(language);
                }
            }

            if (pageLanguageId.HasValue && !existingLanguages.Contains(null))
            {
                languages.Insert(0, languageService.GetInvariantLanguageModel());
            }
        }
    }
}