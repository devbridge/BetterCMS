using System.Web.Mvc;

using BetterCms.Module.MediaManager.Command.Images;
using BetterCms.Module.MediaManager.Command.Images.GetImages;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic for Pages module.
    /// </summary>
    public class ImagesController : CmsControllerBase
    {
        /// <summary>
        /// Gets the images list.
        /// </summary>
        /// <returns>List of images</returns>
        public ActionResult GetImagesList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }

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
        /// <returns>
        /// The view.
        /// </returns>
        public ActionResult ImageInsert()
        {
            var images = GetCommand<GetImagesCommand>().ExecuteCommand(new MediaManagerViewModel());
            var json = new
            {
                Data = new WireJson
                {
                    Success = true,
                    Data = images
                },
                Html = RenderView("ImageInsert", new MediaImageViewModel())
            };

            return WireJson(true, json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Image editor dialog.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns>The view.</returns>
        [HttpGet]
        public ActionResult ImageEditor(string imageId)
        {
            var model = GetCommand<GetImageCommand>().Execute(imageId.ToGuidOrDefault());

            return View(model);
        }

        /// <summary>
        /// Image insert editor.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImageEditorInsert(string imageId)
        {
            var model = GetCommand<GetImageCommand>().Execute(imageId.ToGuidOrDefault());

            return View(model);
        }

        /// <summary>
        /// Image editor dialog post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult ImageEditor(ImageViewModel model)
        {
            GetCommand<SaveImageDataCommand>().Execute(model);
            var result = GetCommand<GetImageCommand>().Execute(model.Id.ToGuidOrDefault());

            return Json(new WireJson { Success = true, Data = result });
        }

        /// <summary>
        /// Image cropper dialog.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns>The view.</returns>
        [HttpGet]
        public ActionResult ImageCropper(string imageId)
        {
            var model = GetCommand<GetImageCommand>().Execute(imageId.ToGuidOrDefault());
            return View(model);
        }

        /// <summary>
        /// Image cropper dialog post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult ImageCropper(ImageViewModel model)
        {
            GetCommand<CropImageCommand>().Execute(model);
            var result = GetCommand<GetImageCommand>().Execute(model.Id.ToGuidOrDefault());

            return Json(new WireJson { Success = true, Data = result });
        }

        /// <summary>
        /// Image resize.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult ImageResize(string imageId, string width, string height, string version)
        {
            var request = new ResizeImageCommandRequest
                              {
                                  Id = imageId.ToGuidOrDefault(),
                                  Width = width.ToIntOrDefault(),
                                  Height = height.ToIntOrDefault(),
                                  Version = version.ToIntOrDefault(),
                              };
            GetCommand<ResizeImageCommand>().Execute(request);
            var model = GetCommand<GetImageCommand>().Execute(request.Id);

            return Json(new WireJson { Success = true, Data = model });
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
        public ActionResult GetImage(string imageId)
        {
            var result = GetCommand<GetImageCommand>().Execute(imageId.ToGuidOrDefault());

            return Json(new WireJson { Success = true, Data = result });
        }
    }
}