using System;
using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.ImagesGallery.Command.DeleteAlbum;
using BetterCms.Module.ImagesGallery.Command.GetAlbumForEdit;
using BetterCms.Module.ImagesGallery.Command.GetAlbumList;
using BetterCms.Module.ImagesGallery.Command.SaveAlbum;
using BetterCms.Module.ImagesGallery.Content.Resources;
using BetterCms.Module.ImagesGallery.ViewModels;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.ImagesGallery.Controllers
{
    /// <summary>
    /// Image gallery albums controller.
    /// </summary>
    [ActionLinkArea(ImagesGalleryModuleDescriptor.ImagesGalleryAreaName)]
    public class AlbumController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template for displaying albums list.
        /// </summary>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult ListTemplate()
        {
            var view = RenderView("List", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var albums = GetCommand<GetAlbumListCommand>().ExecuteCommand(request);

            return ComboWireJson(albums != null, view, albums, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the image gallery albums.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult AlbumsList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetAlbumListCommand>().ExecuteCommand(request);

            return WireJson(model != null, model);
        }

        /// <summary>
        /// Lists the albums list for selecting one.
        /// </summary>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SelectAlbum()
        {
            var view = RenderView("Select", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var albums = GetCommand<GetAlbumListCommand>().ExecuteCommand(request);

            return ComboWireJson(albums != null, view, albums, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the image gallery album.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SaveAlbum(AlbumEditViewModel model)
        {
            var success = false;
            AlbumEditViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveAlbumCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(ImagesGalleryGlobalization.CreateAlbum_CreatedSuccessfully_Message);
                    }

                    success = true;
                }
            }

            return WireJson(success, response);
        }

        /// <summary>
        /// Deletes the image gallery album.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteAlbum(string id, string version)
        {
            var request = new AlbumEditViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteAlbumCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(ImagesGalleryGlobalization.DeleteAlbum_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }

        /// <summary>
        /// Edit the album.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>View for editing an album</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditAlbum(string id)
        {
            var model = GetCommand<GetAlbumForEditCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("Edit", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create the album.
        /// </summary>
        /// <returns>View for creating new album</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult CreateAlbum()
        {
            var model = GetCommand<GetAlbumForEditCommand>().ExecuteCommand(new Guid());
            var view = RenderView("Edit", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }
    }
}
