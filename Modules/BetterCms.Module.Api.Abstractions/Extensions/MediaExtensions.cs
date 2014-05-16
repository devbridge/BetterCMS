using System;

using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

namespace BetterCms.Module.Api.Extensions
{
    public static class MediaExtensions
    {
        public static PutImageRequest ToPutRequest(this GetImageResponse response)
        {
            var model = MapImageModel(response);

            return new PutImageRequest { Data = model, ImageId = response.Data.Id };
        }

        public static PostImageRequest ToPostRequest(this GetImageResponse response)
        {
            var model = MapImageModel(response);

            return new PostImageRequest { Data = model };
        }

        public static PutFileRequest ToPutRequest(this GetFileResponse response)
        {
            var model = MapFileModel(response);

            return new PutFileRequest { Data = model, FileId = response.Data.Id };
        }

        public static PostFileRequest ToPostRequest(this GetFileResponse response)
        {
            var model = MapFileModel(response);

            return new PostFileRequest { Data = model };
        }

        public static PutFolderRequest ToPutRequest(this GetFolderResponse response)
        {
            var model = MapFolderModel(response);

            return new PutFolderRequest { Data = model, FolderId = response.Data.Id };
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
                            };

            return model;
        }

        private static SaveFileModel MapFileModel(GetFileResponse response)
        {
            throw new NotImplementedException();
            var model = new SaveFileModel
                            {
//                                Version = response.Data.Version,
//                                Title = response.Data.Title,
//                                Description = response.Data.Description,
//                                Caption = response.Data.Caption,
//                                FileSize = response.Data.FileSize,
//                                ImageUrl = response.Data.ImageUrl,
//                                Width = response.Data.Width,
//                                Height = response.Data.Height,
//                                ThumbnailUrl = response.Data.ThumbnailUrl,
//                                ThumbnailWidth = response.Data.ThumbnailWidth,
//                                ThumbnailHeight = response.Data.ThumbnailHeight,
//                                ThumbnailSize = response.Data.ThumbnailSize,
//                                IsArchived = response.Data.IsArchived,
//                                FolderId = response.Data.FolderId,
//                                PublishedOn = response.Data.PublishedOn,
//                                OriginalFileName = response.Data.OriginalFileName,
//                                OriginalFileExtension = response.Data.OriginalFileExtension,
//                                OriginalWidth = response.Data.OriginalWidth,
//                                OriginalHeight = response.Data.OriginalHeight,
//                                OriginalSize = response.Data.OriginalSize,
//                                OriginalUrl = response.Data.OriginalUrl,
//                                FileUri = response.Data.FileUri,
//                                IsUploaded = response.Data.IsUploaded,
//                                IsTemporary = response.Data.IsTemporary,
//                                IsCanceled = response.Data.IsCanceled,
//                                OriginalUri = response.Data.OriginalUri,
//                                ThumbnailUri = response.Data.ThumbnailUri,
//                                AccessRules = response.AccessRules
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
