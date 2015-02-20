using System;
using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Command.Images.GetImage;
using BetterCms.Module.MediaManager.Command.Images.GetImages;
using BetterCms.Module.MediaManager.Command.Images.SaveImage;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic for Pages module.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class ImagesController : CmsControllerBase
    {
        /// <summary>
        /// Gets the images list.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// List of images
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent, RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.ManageUsers)]
        public ActionResult GetImagesList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }
            options.SetDefaultPaging();

            var model = GetCommand<GetImagesCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }

            return Json(new WireJson
                            {
                                Success = success,
                                Data = model
                            });
        }

        /// <summary>
        /// Gets images list to insert to content.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <returns>
        /// The view.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.ManageUsers)]
        public ActionResult ImageInsert(string folderId)
        {
            var request = new MediaManagerViewModel { CurrentFolderId = folderId.ToGuidOrDefault() };
            var images = GetCommand<GetImagesCommand>().ExecuteCommand(request);
            var success = images != null;
            var view = RenderView("ImageInsert", new MediaImageViewModel());

            return ComboWireJson(success, view, images, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Image editor dialog.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns>The view.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult ImageEditor(string imageId)
        {
            var model = GetCommand<GetImageCommand>().ExecuteCommand(imageId.ToGuidOrDefault());
            var view = RenderView("ImageEditor", model ?? new ImageViewModel());
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Image insert editor.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns>Image insert view.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ImageEditorInsert(string imageId)
        {
            var model = GetCommand<GetImageCommand>().ExecuteCommand(imageId.ToGuidOrDefault());

            return View(model);
        }

        /// <summary>
        /// Image editor dialog post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult ImageEditor(ImageViewModel model)
        {
            if (GetCommand<SaveImageDataCommand>().ExecuteCommand(model))
            {
                var result = GetCommand<GetImageCommand>().ExecuteCommand(model.Id.ToGuidOrDefault());
                result.ThumbnailUrl += string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat));
                return Json(new WireJson { Success = result != null, Data = result });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Deletes image.
        /// </summary>
        /// <param name="id">The image id.</param>
        /// <param name="version">The image entity version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult ImageDelete(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteMediaCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(MediaGlobalization.DeleteImage_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.ManageUsers)]
        public ActionResult GetImage(string imageId)
        {
            var result = GetCommand<GetImageCommand>().ExecuteCommand(imageId.ToGuidOrDefault());
            if (result != null)
            {
                return Json(new WireJson { Success = true, Data = result });
            }

            return Json(new WireJson { Success = false });
        }
    }
}