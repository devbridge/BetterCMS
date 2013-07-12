using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Vimeo.Services;
using BetterCms.Module.Vimeo.Services.Models;

namespace BetterCms.Module.Vimeo.Command.Videos.SaveVideos
{
    public class SaveVideosCommand : CommandBase, ICommand<SaveVideosRequest, SaveVideosResponse>
    {
        private readonly IVimeoService vimeoService;

        public SaveVideosCommand(IVimeoService vimeoService)
        {
            this.vimeoService = vimeoService;
        }

        public SaveVideosResponse Execute(SaveVideosRequest request)
        {
            var response = new SaveVideosResponse() { SelectedFolderId = request.FolderId };
            MediaFolder folder = null;
            if (!request.FolderId.HasDefaultValue())
            {
                folder = Repository.AsProxy<MediaFolder>(request.FolderId);
                if (folder.IsDeleted)
                {
                    response.FolderIsDeleted = true;
                    return response;
                }
            }

            List<Models.Video> medias = new List<Models.Video>();
            UnitOfWork.BeginTransaction();
            foreach (var videoId in request.VideosIds)
            {
                var video = vimeoService.GetVideo(videoId);
                var file = new Models.Video()
                    {
                        Title = video.Title,
                        IsArchived = false,
                        Type = MediaType.Video,
                        ContentType = MediaContentType.File,
                        Folder = folder,
                        PublishedOn = DateTime.Now,
                        Description = video.Description,

                        OriginalFileName = video.Title,
                        OriginalFileExtension = null,
                        FileUri = new Uri(vimeoService.GetPlayerUrl(video.Id)),
                        PublicUrl = vimeoService.GetVideoUrl(video.Id),
                        Size = 0,
                        IsTemporary = false,
                        IsUploaded = video.Is_Transcoding == 0,
                        IsCanceled = false,

                        VideoId = video.Id,
                        ThumbnailUrl = video.Thumbnails.GetThumbnailUrl(300, 300),
                        AuthorName = video.Owner != null ? video.Owner.Display_Name : null,
                        AuthorUrl = video.Owner != null ? video.Owner.ProfileUrl : null,

                    };
                Repository.Save(file);
                medias.Add(file);
            }

            UnitOfWork.Commit();

            response.Medias =
                medias.Select(
                    file =>
                    (MediaFileViewModel)
                    new MediaVideoViewModel
                        {
                            Id = file.Id,
                            Name = file.Title,
                            Type = file.Type,
                            Version = file.Version,
                            ContentType = MediaContentType.File,
                            PublicUrl = file.PublicUrl,
                            FileExtension = file.OriginalFileExtension,
                            IsProcessing = !file.IsUploaded.HasValue,
                            IsFailed = file.IsUploaded.HasValue && !file.IsUploaded.Value
                        }).ToList();

            return response;
        }
    }
}