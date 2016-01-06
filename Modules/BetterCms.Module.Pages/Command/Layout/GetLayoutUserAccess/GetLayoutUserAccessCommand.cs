using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Layout.GetLayoutUserAccess
{
    public class GetLayoutUserAccessCommand : CommandBase, ICommand<GetLayoutUserAccessCommandRequest, IList<UserAccessViewModel>>
    {
        private readonly ISecurityService securityService;

        public GetLayoutUserAccessCommand(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        public IList<UserAccessViewModel> Execute(GetLayoutUserAccessCommandRequest request)
        {
            var principal = securityService.GetCurrentPrincipal();
            if (request.IsMasterPage)
            {
                return
                    Repository.AsQueryable<Root.Models.Page>()
                              .Where(x => x.Id == request.Id && !x.IsDeleted)
                              .SelectMany(x => x.AccessRules)
                              .OrderBy(x => x.IsForRole)
                              .ThenBy(x => x.Identity)
                              .Select(x => new UserAccessViewModel(x))
                              .ToList();
            }

            return AccessControlService.GetDefaultAccessList(principal).Select(f => new UserAccessViewModel(f)).ToList();
        }
    }
}