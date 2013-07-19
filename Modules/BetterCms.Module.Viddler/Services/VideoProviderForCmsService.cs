using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.Extensions;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Viddler.Models;

using MvcContrib.Sorting;

namespace BetterCms.Module.Viddler.Services
{
    public class VideoProviderForCmsService : IMediaVideoService
    {
        private readonly IRepository repository;

        private readonly IStatusUpdaterService statusUpdaterService;

        public VideoProviderForCmsService(IRepository repository, IStatusUpdaterService statusUpdaterService)
        {
            this.repository = repository;
            this.statusUpdaterService = statusUpdaterService;
        }

        public Tuple<IEnumerable<MediaViewModel>, int> GetItems(MediaManagerViewModel request)
        {
            var query = repository
                .AsQueryable<Media>()
                .Where(m => !m.IsDeleted
                    && m.Original == null
                    && m.Type == MediaType.Video
                    && (m is MediaFolder || m is Video && !((Video)m).IsTemporary));

            if (!request.IncludeArchivedItems)
            {
                query = query.Where(m => !m.IsArchived);
            }

            if (request.Tags != null)
            {
                foreach (var tagKeyValue in request.Tags)
                {
                    var id = tagKeyValue.Key.ToGuidOrDefault();
                    query = query.Where(m => m.MediaTags.Any(mt => mt.Tag.Id == id));
                }
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery) || request.Tags != null)
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(m => m.Title.Contains(searchQuery) || m.Description.Contains(searchQuery));

                var result = new List<Media>();
                var mediaList = query.ToList();

                foreach (var media in mediaList)
                {
                    if (media.IsChild(request.CurrentFolderId, request.IncludeArchivedItems))
                    {
                        result.Add(media);
                    }
                }

                return ToResponse(request, result.AsQueryable());
            }

            if (!request.CurrentFolderId.HasDefaultValue())
            {
                query = query.Where(m => m.Folder.Id == request.CurrentFolderId);
            }
            else
            {
                query = query.Where(m => m.Folder == null);
            }

            return ToResponse(request, query);
        }

        private Tuple<IEnumerable<MediaViewModel>, int> ToResponse(MediaManagerViewModel request, IQueryable<Media> query)
        {
            var count = query.ToRowCountFutureValue();
            query = AddOrder(query, request).AddPaging(request);

            var items = query.Select(SelectItem).ToList();

            var stillProcessing = items.Where(v => v is MediaVideoViewModel && ((MediaVideoViewModel)v).IsProcessing).Select(v => v.Id).ToList();
            statusUpdaterService.UpdateStatus(stillProcessing);
            return new Tuple<IEnumerable<MediaViewModel>, int>(items, count.Value);
        }

        private IQueryable<Media> AddOrder(IQueryable<Media> query, MediaManagerViewModel request)
        {
            var orderedQuery = request.Direction == SortDirection.Descending
                ? query.OrderBy(m => m.ContentType)
                : query.OrderByDescending(m => m.ContentType);

            return orderedQuery.ThenBy(request.Column, request.Direction);
        }

        private MediaViewModel SelectItem(Media media)
        {
            if (media is MediaFolder)
            {
                return new MediaFolderViewModel().Fill(media);
            }

            var video = media as Video;
            if (video != null)
            {
                var model = new MediaVideoViewModel();
                model.Fill(media);

                model.PublicUrl = video.PublicUrl;
                model.FileExtension = video.OriginalFileExtension;
                model.Size = video.Size;
                model.IsProcessing = video.IsUploaded == null;
                model.IsFailed = video.IsUploaded == false;

                return model;
            }

            throw new InvalidOperationException("Cannot convert media to view model. Wrong entity passed.");
        }

    }
}