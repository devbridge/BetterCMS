using System;
using System.Collections.Generic;
using System.Linq;
using BetterCms.Core.Exceptions;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.MediaManager.ViewModels.Upload;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.ConfirmUpload
{
    public class ConfirmUploadCommand : CommandBase, ICommand<MultiFileUploadViewModel, ConfirmUploadResponse>
    {
        private readonly ICmsConfiguration cmsConfiguration;
        
        private readonly IMediaFileService fileService;

        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmUploadCommand" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="accessControlService">The access control service.</param>
        public ConfirmUploadCommand(ICmsConfiguration cmsConfiguration, IMediaFileService fileService, IAccessControlService accessControlService)
        {
            this.accessControlService = accessControlService;
            this.cmsConfiguration = cmsConfiguration;
            this.fileService = fileService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        public ConfirmUploadResponse Execute(MultiFileUploadViewModel request)
        {
            ConfirmUploadResponse response = new ConfirmUploadResponse { SelectedFolderId = request.SelectedFolderId ?? Guid.Empty, ReuploadMediaId = request.ReuploadMediaId };

            if (request.UploadedFiles != null && request.UploadedFiles.Count > 0)
            {
                MediaFolder folder = null;

                if (request.SelectedFolderId != null && request.SelectedFolderId.Value != Guid.Empty)
                {
                    folder = Repository.AsProxy<MediaFolder>(request.SelectedFolderId.Value);
                    if (folder.IsDeleted)
                    {
                        response.FolderIsDeleted = true;
                        return response;
                    }
                }

                UnitOfWork.BeginTransaction();

                List<MediaFile> files = new List<MediaFile>();
                var updateAccessControl = true;

                if (request.ReuploadMediaId.HasDefaultValue())
                {
                    UpdateMedia(request, folder, files);
                }
                else
                {
                    // Re-upload performed.
                    var fileId = request.UploadedFiles.FirstOrDefault();
                    var file = Repository.FirstOrDefault<MediaFile>(fileId);

                    if (!fileId.HasDefaultValue())
                    {
                        var originalMedia = Repository.First<MediaFile>(request.ReuploadMediaId);
                        if (cmsConfiguration.Security.AccessControlEnabled && !(originalMedia is IAccessControlDisabled))
                        {
                            AccessControlService.DemandAccess(originalMedia, Context.Principal, AccessLevel.ReadWrite);
                        }

                        file.Original = originalMedia;

                        // Do not update access control, if re-uploading
                        updateAccessControl = false;

                        file.Title = originalMedia.Title;
                        file.Description = originalMedia.Description;
                        file.IsArchived = originalMedia.IsArchived;
                        file.Folder = originalMedia.Folder;
                        file.Image = originalMedia.Image;
                        if (file is MediaImage && originalMedia is MediaImage)
                        {
                            ((MediaImage)file).Caption = ((MediaImage)originalMedia).Caption;
                            ((MediaImage)file).ImageAlign = ((MediaImage)originalMedia).ImageAlign;
                        }

                        file.IsTemporary = false;
                        file.PublishedOn = DateTime.Now;

                        var temp = (MediaFile)file.Clone();
                        originalMedia.CopyDataTo(file);
                        temp.CopyDataTo(originalMedia);
                        files.Add(originalMedia);
                    }
                }

                if (updateAccessControl && cmsConfiguration.Security.AccessControlEnabled)
                {
                    foreach (var file in files)
                    {
                        if (!(file is MediaImage))
                        {
                            var currentFile = file;
                            var fileEntity = Repository.AsQueryable<MediaFile>().Where(f => f.Id == currentFile.Id).FetchMany(f => f.AccessRules).ToList().FirstOne();

                            accessControlService.UpdateAccessControl(
                                fileEntity, request.UserAccessList != null ? request.UserAccessList.Cast<IAccessRule>().ToList() : new List<IAccessRule>());
                        }
                    }                    
                }

                UnitOfWork.Commit();

                if (request.ReuploadMediaId.HasDefaultValue())
                {
                    foreach (var file in files)
                    {
                        if (file is MediaImage)
                        {
                            file.PublicUrl += string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat));
                            ((MediaImage)file).PublicThumbnailUrl += string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat));
                        }
                    }
                }

                response.Medias = files.Select(Convert).ToList();

                // Notify.
                foreach (var mediaFile in files)
                {
                    Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaFile);
                }
            }

            return response;
        }

        private void UpdateMedia(MultiFileUploadViewModel request, MediaFolder folder, List<MediaFile> files)
        {
            foreach (var fileId in request.UploadedFiles)
            {
                if (!fileId.HasDefaultValue())
                {
                    var file = Repository.FirstOrDefault<MediaFile>(fileId);
                    if (folder != null && (file.Folder == null || file.Folder.Id != folder.Id))
                    {
                        file.Folder = folder;
                    }
                    file.IsTemporary = false;
                    file.PublishedOn = DateTime.Now;
                    Repository.Save(file);

                    files.Add(file);
                }
            }
        }

        private MediaFileViewModel Convert(MediaFile file)
        {
            MediaFileViewModel model;
            bool isProcessing = !file.IsUploaded.HasValue;
            bool isFailed = file.IsUploaded.HasValue && !file.IsUploaded.Value;

            if (file.Type == MediaType.File)
            {
                model = new MediaFileViewModel();

                if (cmsConfiguration.Security.AccessControlEnabled)
                {
                    SetIsReadOnly(model, ((IAccessSecuredObject)file).AccessRules);
                }

                if (file.Image != null)
                {
                    model.ThumbnailUrl = file.Image.PublicThumbnailUrl;
                    model.Tooltip = file.Image.Caption ?? file.Image.Title;
                }
            }
            else if (file.Type == MediaType.Audio)
            {
                model = new MediaAudioViewModel();
            }
            else if (file.Type == MediaType.Video)
            {
                model = new MediaVideoViewModel();
            }
            else if (file.Type == MediaType.Image)
            {
                var imageFile = (MediaImage)file;
                model = new MediaImageViewModel
                {
                    ThumbnailUrl = imageFile.PublicThumbnailUrl,
                    Tooltip = imageFile.Caption,
                    Width = imageFile.Width,
                    Height = imageFile.Height
                };
                isProcessing = isProcessing || !imageFile.IsOriginalUploaded.HasValue || !imageFile.IsThumbnailUploaded.HasValue;
                isFailed = isFailed
                    || (imageFile.IsOriginalUploaded.HasValue && !imageFile.IsOriginalUploaded.Value)
                    || (imageFile.IsThumbnailUploaded.HasValue && !imageFile.IsThumbnailUploaded.Value);
            }
            else
            {
                throw new CmsException(string.Format("A file type {0} is not supported.", file.Type));
            }

            model.Id = file.Id;
            model.Name = file.Title;
            model.Type = file.Type;
            model.Version = file.Version;
            model.ContentType = MediaContentType.File;
            model.PublicUrl = fileService.GetDownloadFileUrl(file.Type, file.Id, file.PublicUrl);
            model.FileExtension = file.OriginalFileExtension;
            model.IsProcessing = isProcessing;
            model.IsFailed = isFailed;
            model.SizeText = file.SizeAsText();

            return model;
        }
    }
}
