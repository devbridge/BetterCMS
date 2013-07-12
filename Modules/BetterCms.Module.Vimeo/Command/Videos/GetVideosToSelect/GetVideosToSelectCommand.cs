using System;
using System.ComponentModel;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Vimeo.Services;
using BetterCms.Module.Vimeo.Services.Models;
using BetterCms.Module.Vimeo.ViewModels.Video;

namespace BetterCms.Module.Vimeo.Command.Videos.GetVideosToSelect
{
    internal class GetVideosToSelectCommand : CommandBase, ICommand<VideoListSearchViewModel, VideoListViewModel>
    {
        private readonly IVimeoService vimeoService;

        public GetVideosToSelectCommand(IVimeoService vimeoService)
        {
            this.vimeoService = vimeoService;
        }

        public VideoListViewModel Execute(VideoListSearchViewModel request)
        {
            var searchIsEmpty = string.IsNullOrEmpty(request.SearchQuery);

            var userId = string.Empty;
            if (request.OnlyMine)
            {
                userId = vimeoService.GetCurrentUserId();
            }
            else if (searchIsEmpty)
            {
                return new VideoListViewModel(new BindingList<VideoViewModel>(), request, 0) { OnlyMine = request.OnlyMine };
            }

            var result = searchIsEmpty
                             ? vimeoService.GetUserVideos(userId, request.PageNumber, request.PageSize)
                             : vimeoService.SearchVideo(request.SearchQuery, userId, request.PageNumber, request.PageSize);

            var videos = result.Video.AsQueryable();
            var items =
                videos.Select(
                    v =>
                    new VideoViewModel
                        {
                            VideoId = v.Id,
                            Title = v.Title,
                            Description = v.Description,
                            Duration = TimeSpan.FromSeconds(v.Duration),
                            Width = v.Width,
                            Height = v.Height,
                            IsPublic = v.IsPublic(),
                            OwnerName = v.Owner != null ? v.Owner.Display_Name : string.Empty,
                            IsTranscoding = v.Is_Transcoding == 1,
                            ThumbnailUrl = v.Thumbnails.GetThumbnailUrl(300, 300)
                        }).ToList();

            return new VideoListViewModel(items, request, result.Total) { OnlyMine = request.OnlyMine };
        }
    }
}