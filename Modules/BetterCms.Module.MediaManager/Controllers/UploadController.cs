using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Command.Upload;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.Upload;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    public class UploadController : CmsControllerBase
    {        
        private readonly IMediaImageService mediaService;

        public UploadController(IMediaImageService mediaService)
        {
            this.mediaService = mediaService;
        }

        public ActionResult MultiFileUpload(string folderId)
          {
              var model = new MultiFileUploadViewModel();
              model.RootFolderId = folderId.ToGuidOrDefault();
              model.Folders = new Dictionary<Guid, string>();

              return View(model);
          }

          public ActionResult UploadMedia(HttpPostedFileBase file)
          {
              var rootFolderId = Request.Form["rootFolderId"].ToGuidOrDefault();
              var rootFolderType = (MediaType)Enum.Parse(typeof(MediaType), Request.Form["rootFolderType"]);

              UploadFileRequest request = new UploadFileRequest();
              request.RootFolderId = rootFolderId;
              request.Type = rootFolderType;
              request.FileLength = file.ContentLength;
              request.FileName = file.FileName;
              request.FileStream = file.InputStream;

              var media = GetCommand<UploadFileCommand>().ExecuteCommand(request);

              if (media != null)
              {
                  return Json(
                      new WireJson(
                          true,
                          new
                              {
                                  FileId = media.Id                                  
                              }));
              }

              return Json(new WireJson(false));              
          }

        public ActionResult RemoveFileUpload(string[] fileNamesToRemove)
        {
            return Json(
                new
                    {
                        Success = true
                    });
        }

        public ActionResult SaveUploads(MultiFileUploadViewModel model)
        {
            return Json(new WireJson(true));
        }
    }
}
