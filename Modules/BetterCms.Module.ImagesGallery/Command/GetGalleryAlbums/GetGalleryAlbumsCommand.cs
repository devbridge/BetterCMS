using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Web;

using BetterCms.Module.ImagesGallery.Providers;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate.Linq;

namespace BetterCms.Module.ImagesGallery.Command.GetGalleryAlbums
{
    public class GetGalleryAlbumsCommand : CommandBase, ICommand<RenderWidgetViewModel, GalleryViewModel>
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGalleryAlbumsCommand" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        public GetGalleryAlbumsCommand(IRepository repository, IHttpContextAccessor contextAccessor)
        {
            this.repository = repository;
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public GalleryViewModel Execute(RenderWidgetViewModel request)
        {
            List<AlbumViewModel> albums;
            var ids = request.Options
                .Where(o => o.Type == OptionType.Custom
                    && o.CustomOption != null
                    && o.CustomOption.Identifier == ImageGalleryAlbumOptionProvider.Identifier
                    && o.Value is Guid)
                .Select(o => o.Value)
                .Distinct()
                .ToArray();

            if (ids.Length > 0)
            {
                // Load list of albums
                albums = repository
                    .AsQueryable<Models.Album>()
                    .Where(a => ids.Contains(a.Id))
                    .Select(a => new AlbumViewModel
                                     {
                                         Url = a.Id.ToString(),
                                         Title = a.Title,
                                         ImagesCount = a.Folder.Medias.Count(m => m is MediaImage),
                                         LastUpdateDate = a.Folder.Medias.Max(m => m.ModifiedOn),
                                         CoverImageUrl = a.CoverImage != null ? a.CoverImage.PublicUrl : null
                                     })
                    .ToFuture()
                    .ToList();

                var urlPattern = GetAlbumUrlPattern(); string.Format("/?{0}={1}",
                    ImageGallerModuleConstants.GalleryAlbumIdQueryParameterName,
                    "{0}");
                albums.ForEach(a => a.Url = string.Format(urlPattern, a.Url));
            }
            else
            {
                albums = new List<AlbumViewModel>();
            }

            return new GalleryViewModel
                       {
                           Albums = albums,
                           LoadCmsStyles = request.GetOptionValue<bool>(ImageGallerModuleConstants.LoadCmsStylesWidgetOptionKey)
                       };
        }

        /// <summary>
        /// Gets the album URL pattern.
        /// </summary>
        /// <returns>Album URL pattern</returns>
        private string GetAlbumUrlPattern()
        {
            var context = contextAccessor.GetCurrent();
            if (context != null && context.Request.Url != null)
            {
                var url = context.Request.Url.ToString();
                if (!url.Contains("?"))
                {
                    url = string.Concat(url, "?");
                }
                else
                {
                    url = string.Concat(url, "&");
                }
                url = string.Format("{0}{1}={2}", url, ImageGallerModuleConstants.GalleryAlbumIdQueryParameterName, "{0}");

                return url;
            }

            return null;
        }
    }
}