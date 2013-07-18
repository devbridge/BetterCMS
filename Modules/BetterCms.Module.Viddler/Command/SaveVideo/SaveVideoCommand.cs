using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Models;
using BetterCms.Module.Viddler.Services;

namespace BetterCms.Module.Viddler.Command.Videos.SaveVideos
{
    internal class SaveVideoCommand : CommandBase, ICommand<SaveVideoRequest, Video>
    {
        private readonly IViddlerService viddlerService;

        public SaveVideoCommand(IViddlerService viddlerService)
        {
            this.viddlerService = viddlerService;
        }

        public Video Execute(SaveVideoRequest request)
        {
            var sesionId = viddlerService.GetSessionId();
            var data = viddlerService.GetVideoDetails(sesionId, request.VideoId);
            var video = new Video
                {
                    OriginalFileName = data.Title,
                    OriginalFileExtension = string.Empty,
                    FileUri = new Uri(viddlerService.GetPlayerUrl(data.Id)),
                    PublicUrl = viddlerService.GetVideoUrl(data.Id),
                    Size = data.Length,
                    IsTemporary = true,
                    IsUploaded = data.IsReady ? true : (bool?)null,
                    Title = data.Title,
                    Type = MediaType.Video,
                    ContentType = MediaContentType.File,
                    PublishedOn = DateTime.Now,
                    Description = data.Description,
                    VideoId = data.Id,
                    ThumbnailUrl = data.ThumbnailUrl,
                };
            Repository.Save(video);
            UnitOfWork.Commit();
            return video;
        }
    }
}