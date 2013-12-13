using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
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
        /// Initializes a new instance of the <see cref="GetPageForCloningWithCultureCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetPageForCloningWithCultureCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
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
                .Select(p => new ClonePageWithCultureViewModel
                        {
                            PageId = p.Id,
                            IsMasterPage = p.IsMasterPage
                        })
                .ToFuture();

            var cultureFutureQuery = Repository
                .AsQueryable<Root.Models.Culture>()
                .Where(c => c.Id == request.CultureId)
                .Select(c => new
                        {
                            Name = c.Name
                        })
                .ToFuture();

            ClonePageWithCultureViewModel model;
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

            model = pageFutureQuery.FirstOne();
            model.CultureName = cultureFutureQuery.FirstOne().Name;
            model.AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled;
            model.UserAccessList = accessRules;
            model.CultureId = request.CultureId;

            if (model.IsMasterPage)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            return model;
        }
    }
}