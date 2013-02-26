using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.MediaManager.Command.Upload;
using BetterCms.Module.MediaManager.Command.Upload.ConfirmUpload;
using BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload;
using BetterCms.Module.MediaManager.Command.Upload.UndoUpload;
using BetterCms.Module.MediaManager.Command.Upload.Upload;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Helpers;
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

        [HttpGet]
        public ActionResult MultiFileUpload(string folderId, string folderType)
        {
            var model = GetCommand<GetMultiFileUploadCommand>().ExecuteCommand(
                new GetMultiFileUploadRequest
                    {
                        FolderId = folderId.ToGuidOrDefault(),
                        Type = (MediaType)Enum.Parse(typeof(MediaType), folderType)
                    });

            return View(model);
        }

        [HttpGet]
        public ActionResult SingleFileUpload(string folderId, string folderType)
        {
            var model = GetCommand<GetMultiFileUploadCommand>().ExecuteCommand(
                new GetMultiFileUploadRequest
                {
                    FolderId = folderId.ToGuidOrDefault(),
                    Type = (MediaType)Enum.Parse(typeof(MediaType), folderType)
                });

            return View("SingleFileUpload",model);
        }

        [HttpPost]
        public WrappedJsonResult UploadSingleFile(HttpPostedFileWrapper uploadFile, string SelectedFolderId, string RootFolderType)
        {
            var rootFolderType = (MediaType)Enum.Parse(typeof(MediaType), RootFolderType);
            if (uploadFile != null && FileFormatIsValid(rootFolderType, uploadFile.ContentType))
            {
                UploadFileRequest request = new UploadFileRequest();
                request.RootFolderId = SelectedFolderId.ToGuidOrDefault();
                request.Type = rootFolderType;
                request.FileLength = uploadFile.ContentLength;
                request.FileName = uploadFile.FileName;
                request.FileStream = uploadFile.InputStream;

                var media = GetCommand<UploadCommand>().ExecuteCommand(request);
                if (media != null)
                {
                    return new WrappedJsonResult
                    {
                        Data = new
                        {
                            Success = true,
                            Id = media.Id,
                            fileName = media.OriginalFileName,
                            fileSize = media.Size,
                            Version = media.Version,
                            Type = request.Type
                        }
                    };
                }
            }

            List<string> messages = new List<string>();
            messages.AddRange(Messages.Error);

            return new WrappedJsonResult
            {
                Data = new
                {
                    Success = false,
                    Messages = messages.ToArray()
                }
            };
        }

        [HttpPost]
        public ActionResult UploadMedia(HttpPostedFileBase file)
        {
            var rootFolderId = Request.Form["rootFolderId"].ToGuidOrDefault();
            var rootFolderType = (MediaType)Enum.Parse(typeof(MediaType), Request.Form["rootFolderType"]);

            if (file != null && FileFormatIsValid(rootFolderType, file.ContentType))
            {
                UploadFileRequest request = new UploadFileRequest();
                request.RootFolderId = rootFolderId;
                request.Type = rootFolderType;
                request.FileLength = file.ContentLength;
                request.FileName = file.FileName;
                request.FileStream = file.InputStream;

                var media = GetCommand<UploadCommand>().ExecuteCommand(request);

                if (media != null)
                {
                    return Json(new WireJson(true, new { FileId = media.Id, Version = media.Version, Type = (int)rootFolderType }));
                }
            }
            return Json(new WireJson(false));
        }

        [HttpPost]
        public ActionResult RemoveFileUpload(string fileId, string version, string type)
        {
            var result = GetCommand<UndoUploadCommand>().ExecuteCommand(new UndoUploadRequest
                                                                            {
                                                                                FileId = fileId.ToGuidOrDefault(),
                                                                                Version = version.ToIntOrDefault(),
                                                                                Type = (MediaType)type.ToIntOrDefault()
                                                                            });

            return Json(
                new
                    {
                        Success = true
                    });
        }

        [HttpPost]
        public ActionResult SaveUploads(MultiFileUploadViewModel model)
        {
            var result = GetCommand<ConfirmUploadCommand>().ExecuteCommand(model);

            if (result == null)
            {
                Messages.AddError(MediaGlobalization.MultiFileUpload_SaveFailed);
            }
            else if (result.FolderIsDeleted)
            {
                Messages.AddError(MediaGlobalization.MultiFileUpload_SaveFailed_FolderDeleted);
            }

            return Json(new WireJson(result != null && !result.FolderIsDeleted, result));
        }

        private bool FileFormatIsValid(MediaType folderType, string fileType)
        {
            if (folderType == MediaType.Image)
            {
                switch (fileType)
                {
                    case "image/png":
                    case "image/jpg":
                    case "image/jpeg":
                    case "image/gif":
                        break;
                    default:
                        Messages.AddError(MediaGlobalization.FileUpload_Failed_ImageFormatNotSuported);
                        return false;
                }
            }

            return true;
        }
    }
}
