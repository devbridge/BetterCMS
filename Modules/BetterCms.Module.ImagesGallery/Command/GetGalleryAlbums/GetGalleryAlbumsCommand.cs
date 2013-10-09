using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Web;

using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.ImagesGallery.Command.GetGalleryAlbums
{
    public class GetGalleryAlbumsCommand : CommandBase, ICommand<RenderWidgetViewModel, GalleryViewModel>
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGalleryAlbumsCommand" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public GetGalleryAlbumsCommand(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public GalleryViewModel Execute(RenderWidgetViewModel request)
        {
            var id = request.GetOptionValue<Guid?>(ImagesGalleryModuleConstants.OptionKeys.GalleryFolder);

            var albumQuery = Repository.AsQueryable<MediaFolder>();

            if (id.HasValue)
            {
                var folderProxy = Repository.AsProxy<MediaFolder>(id.Value);
                albumQuery = albumQuery.Where(f => f.Folder == folderProxy);
            }
            else
            {
                albumQuery = albumQuery.Where(f => f.Folder == null);
            }

            // Load list of albums
            List<AlbumViewModel> albums = albumQuery
                .Select(a => new AlbumViewModel
                    {
                        Url = a.Id.ToString(),
                        Title = a.Title,
                        CoverImageUrl = a.Medias.Where(m => m is MediaImage && !m.IsDeleted).OrderBy(m => m.Title).Select(m => ((MediaImage)m).PublicUrl).FirstOrDefault(),
                        ImagesCount = a.Medias.Count(m => m is MediaImage && !m.IsDeleted),
                        LastUpdateDate = a.Medias.Where(m => m is MediaImage && !m.IsDeleted).Max(m => m.ModifiedOn)
                    })
                .ToList();
            
            var urlPattern = GetAlbumUrlPattern(request);
            albums.ForEach(a => a.Url = string.Format(urlPattern, a.Url));

            return new GalleryViewModel
                       {
                           Albums = albums.ToList(),
                           LoadCmsStyles = request.GetOptionValue<bool>(ImagesGalleryModuleConstants.OptionKeys.LoadCmsStyles),
                           ImagesPerSection = request.GetOptionValue<int>(ImagesGalleryModuleConstants.OptionKeys.ImagesPerSection)
                       };
        }

        /// <summary>
        /// Gets the album URL pattern.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Album URL pattern
        /// </returns>
        private string GetAlbumUrlPattern(RenderWidgetViewModel request)
        {
            var context = contextAccessor.GetCurrent();
            if (context != null && context.Request.Url != null)
            {
                var url = context.Request.Url.ToString();

                // Try to take url from option values
                var optionUrl = request.GetOptionValue<string>(ImagesGalleryModuleConstants.OptionKeys.AlbumUrl);
                if (!string.IsNullOrWhiteSpace(optionUrl))
                {
                    if (optionUrl.Contains("{0}"))
                    {
                        return optionUrl;
                    }

                    url = optionUrl;
                }

                var pattern = string.Format("{0}=[\\d\\w\\-]{{36}}", ImagesGalleryModuleConstants.GalleryFolderIdQueryParameterName);
                var regex = new Regex(pattern);
                var matches = regex.Matches(url);
                if (matches.Count > 0 && matches[0].Groups.Count > 0)
                {
                    url = url.Replace(matches[0].Groups[0].Value, string.Format("{0}={1}", ImagesGalleryModuleConstants.GalleryFolderIdQueryParameterName, "{0}"));
                }
                else
                {
                    url = string.Concat(url, !url.Contains("?") ? "?" : "&");
                    url = string.Format("{0}{1}={2}", url, ImagesGalleryModuleConstants.GalleryFolderIdQueryParameterName, "{0}");
                }

                return url;
            }

            return null;
        }
    }
}