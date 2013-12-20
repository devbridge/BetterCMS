using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.GetPageForCloningWithCulture
{
    public class GetPageForCloningWithCultureCommand : CommandBase, ICommand<GetPageForCloningWithCultureCommandRequest, ClonePageWithCultureViewModel>
    {
        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The culture service
        /// </summary>
        private readonly ICultureService cultureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPageForCloningWithCultureCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="cultureService">The culture service.</param>
        public GetPageForCloningWithCultureCommand(ICmsConfiguration cmsConfiguration, ICultureService cultureService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.cultureService = cultureService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ClonePageWithCultureViewModel Execute(GetPageForCloningWithCultureCommandRequest request)
        {
            var pageFutureQuery = Repository
                .AsQueryable<PageProperties>()
                .Where(p => p.Id == request.PageId && !p.IsDeleted)
                .Select(p =>
                    new
                    {
                        Model = new ClonePageWithCultureViewModel
                            {
                                PageId = p.Id,
                                IsMasterPage = p.IsMasterPage
                            },
                        CultureGroupIdentifier = p.CultureGroupIdentifier,
                        CultureId = p.Culture != null ? p.Culture.Id : (System.Guid?) null
                    })
                .ToFuture();

            var culturesFuture = cultureService.GetCultures();
            var result = pageFutureQuery.FirstOne();
            var model = result.Model;
            model.Cultures = culturesFuture.ToList();

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

            AddRemoveCultures(model.Cultures, result.CultureGroupIdentifier, result.CultureId);

            return model;
        }

        private void AddRemoveCultures(List<LookupKeyValue> cultures, System.Guid? cultureGroupIdentifier, System.Guid? pageCultureId)
        {
            var existingCultures = new List<System.Guid?>();
            if (cultureGroupIdentifier.HasValue)
            {
                existingCultures = Repository
                    .AsQueryable<Root.Models.Page>(p => p.CultureGroupIdentifier == cultureGroupIdentifier.Value)
                    .Select(p => p.Culture != null ? p.Culture.Id : (System.Guid?)null)
                    .ToArray()
                    .Concat(existingCultures)
                    .ToList();
            }
            else
            {
                existingCultures.Add(pageCultureId);
            }

            foreach (var cultureId in existingCultures)
            {
                var culture = cultures.FirstOrDefault(c => c.Key == cultureId.ToString().ToLowerInvariant());
                if (culture != null)
                {
                    cultures.Remove(culture);
                }
            }

            if (pageCultureId.HasValue && !existingCultures.Contains(null))
            {
                cultures.Insert(0, cultureService.GetInvariantCultureModel());
            }
        }
    }
}