using System;
using System.IO;
using System.Linq;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

using ServiceStack;

namespace BetterCms.Module.Api.Extensions
{
    public static class MediaExtensions
    {
        public static PutImageRequest ToPutRequest(this GetImageResponse response)
        {
            var model = MapImageModel(response);

            return new PutImageRequest { Data = model, Id = response.Data.Id };
        }

        public static PostImageRequest ToPostRequest(this GetImageResponse response)
        {
            var model = MapImageModel(response);

            return new PostImageRequest { Data = model };
        }

        public static PutFileRequest ToPutRequest(this GetFileResponse response)
        {
            var model = MapFileModel(response);

            return new PutFileRequest { Data = model, Id = response.Data.Id };
        }

        public static PostFileRequest ToPostRequest(this GetFileResponse response)
        {
            var model = MapFileModel(response);

            return new PostFileRequest { Data = model };
        }

        public static PutFolderRequest ToPutRequest(this GetFolderResponse response)
        {
            var model = MapFolderModel(response);

            return new PutFolderRequest { Data = model, Id = response.Data.Id };
        }

        public static PostFolderRequest ToPostRequest(this GetFolderResponse response)
        {
            var model = MapFolderModel(response);

            return new PostFolderRequest { Data = model };
        }

        private static SaveImageModel MapImageModel(GetImageResponse response)
        {
            var model = new SaveImageModel
                            {
                                Version = response.Data.Version,
                                Title = response.Data.Title,
                                Description = response.Data.Description,
                                Caption = response.Data.Caption,
                                FileSize = response.Data.FileSize,
                                ImageUrl = response.Data.ImageUrl,
                                Width = response.Data.Width,
                                Height = response.Data.Height,
                                ThumbnailUrl = response.Data.ThumbnailUrl,
                                ThumbnailWidth = response.Data.ThumbnailWidth,
                                ThumbnailHeight = response.Data.ThumbnailHeight,
                                ThumbnailSize = response.Data.ThumbnailSize,
                                IsArchived = response.Data.IsArchived,
                                FolderId = response.Data.FolderId,
                                PublishedOn = response.Data.PublishedOn,
                                OriginalFileName = response.Data.OriginalFileName,
                                OriginalFileExtension = response.Data.OriginalFileExtension,
                                OriginalWidth = response.Data.OriginalWidth,
                                OriginalHeight = response.Data.OriginalHeight,
                                OriginalSize = response.Data.OriginalSize,
                                OriginalUrl = response.Data.OriginalUrl,
                                FileUri = response.Data.FileUri,
                                IsUploaded = response.Data.IsUploaded,
                                IsTemporary = response.Data.IsTemporary,
                                IsCanceled = response.Data.IsCanceled,
                                OriginalUri = response.Data.OriginalUri,
                                ThumbnailUri = response.Data.ThumbnailUri,
                                Tags = response.Tags != null ? response.Tags.Select(t => t.Name).ToList() : null,
                                Categories = response.Data.Categories != null ? response.Data.Categories.Select(t => t.Id).ToList() : null,
                            };

            return model;
        }

        private static SaveFileModel MapFileModel(GetFileResponse response)
        {
            var model = new SaveFileModel
                            {
                                Version = response.Data.Version,
                                FolderId = response.Data.FolderId,
                                Title = response.Data.Title,
                                IsArchived = response.Data.IsArchived,
                                PublishedOn = response.Data.PublishedOn,
                                Description = response.Data.Description,
                                OriginalFileName = response.Data.OriginalFileName,
                                OriginalFileExtension = response.Data.OriginalFileExtension,
                                FileUri = response.Data.FileUri,
                                PublicUrl = response.Data.FileUrl,
                                FileSize = response.Data.FileSize,
                                IsTemporary = response.Data.IsTemporary,
                                IsUploaded = response.Data.IsUploaded,
                                IsCanceled = response.Data.IsCanceled,
                                Tags = response.Tags != null ? response.Tags.Select(t => t.Name).ToList() : null,
                                AccessRules = response.AccessRules,
                                ThumbnailId = response.Data.ThumbnailId,
                                Categories = response.Data.Categories != null ? response.Data.Categories.Select(t => t.Id).ToList() : null,
                            };

            return model;
        }

        private static SaveFolderModel MapFolderModel(GetFolderResponse response)
        {
            var model = new SaveFolderModel
                            {
                                Version = response.Data.Version,
                                Title = response.Data.Title,
                                IsArchived = response.Data.IsArchived,
                                ParentFolderId = response.Data.ParentFolderId,
                                Type = response.Data.Type
                            };
            return model;
        }
    }
}
