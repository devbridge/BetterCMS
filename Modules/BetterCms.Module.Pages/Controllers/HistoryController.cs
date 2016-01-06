using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.History.DestroyContentDraft;
using BetterCms.Module.Pages.Command.History.GetContentHistory;
using BetterCms.Module.Pages.Command.History.GetContentVersion;
using BetterCms.Module.Pages.Command.History.RestoreContentVersion;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

using NHibernate.Hql.Ast.ANTLR.Tree;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Content history management.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class HistoryController : CmsControllerBase
    {
        /// <summary>
        /// Contents the history.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns>Content history view html.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ContentHistory(string contentId)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(new GetContentHistoryRequest
                                                                                      {
                                                                                          ContentId = contentId.ToGuidOrDefault(),
                                                                                      });
            return View(model);
        }

        /// <summary>
        /// Contents the history.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Content history view html.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult ContentHistory(GetContentHistoryRequest request)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(request);

            return View(model);
        }

        /// <summary>
        /// Contents the version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Content preview html.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ContentVersion(string id)
        {
            var model = GetCommand<GetContentVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return View(model);
        }

        /// <summary>
        /// Restores the page content version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isUserConfirmed">Determines, if user is confirmed the restoring of a content version.</param>
        /// <param name="includeChildRegions">Determines, if child regions should be included to the results.</param>
        /// <returns>
        /// Json result.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult RestorePageContentVersion(string id, string isUserConfirmed, string includeChildRegions)
        {
            try
            {
                var request = new RestorePageContentViewModel
                {
                    PageContentId = id.ToGuidOrDefault(), 
                    IsUserConfirmed = isUserConfirmed.ToBoolOrDefault(),
                    IncludeChildRegions = includeChildRegions.ToBoolOrDefault()
                };
                var response = GetCommand<RestoreContentVersionCommand>().ExecuteCommand(request);

                return WireJson(response != null, response);
            }
            catch (ConfirmationRequestException exc)
            {
                return Json(new WireJson { Success = false, Data = new { ConfirmationMessage = exc.Resource() } });
            }
        }

        /// <summary>
        /// Destroys the content draft.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="includeChildRegions">Determines, if child regions should be included to the results.</param>
        /// <returns>
        /// Json result.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult DestroyContentDraft(string id, string version, string includeChildRegions)
        {
            var request = new DestroyContentDraftCommandRequest
                              {
                                  Id = id.ToGuidOrDefault(),
                                  Version = version.ToIntOrDefault(),
                                  IncludeChildRegions = includeChildRegions.ToBoolOrDefault()
                              };
            var response = GetCommand<DestroyContentDraftCommand>().ExecuteCommand(request);

            return WireJson(response != null, response);
        }
    }
}
