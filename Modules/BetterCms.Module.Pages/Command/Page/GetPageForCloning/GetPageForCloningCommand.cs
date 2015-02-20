using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.GetPageForCloning
{   
    public class GetPageForCloningCommand : CommandBase, ICommand<Guid, ClonePageViewModel>
    {
        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPageForCloningCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetPageForCloningCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ClonePageViewModel Execute(Guid request)
        {
            var pageQuery = Repository
                .AsQueryable<PageProperties>()
                .Where(p => p.Id == request && !p.IsDeleted)
                .Select(p => new ClonePageViewModel
                        {
                            PageId = p.Id,
                            IsMasterPage = p.IsMasterPage
                        })
                .ToFuture();

            ClonePageViewModel model;
            IList<UserAccessViewModel> accessRules;
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                accessRules = Repository
                    .AsQueryable<Root.Models.Page>()
                    .Where(x => x.Id == request && !x.IsDeleted)
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

            model = pageQuery.FirstOne();
            model.AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled;
            model.UserAccessList = accessRules;

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