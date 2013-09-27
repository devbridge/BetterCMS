using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Web;

using BetterCms.Module.ImagesGallery.Models;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbum
{
    public class GetAlbumCommand : CommandBase, ICommand<GetAlbumCommandRequest, AlbumViewModel>
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAlbumCommand" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public GetAlbumCommand(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public AlbumViewModel Execute(GetAlbumCommandRequest request)
        {
            AlbumViewModel album = null;
            var result = Repository
                .AsQueryable<Album>()
                .Where(a => a.Id == request.AlbumId && !a.Folder.IsDeleted)
                .Select(a => new
                    {
                        FolderId = a.Folder.Id,
                        Album = new AlbumViewModel
                            {
                                Title = a.Title,
                                LastUpdateDate = a.Folder.Medias.Max(m => m.ModifiedOn),
                                CoverImageUrl = a.CoverImage != null ? a.CoverImage.PublicUrl : null
                            }
                    })
                .FirstOrDefault();

            if (result != null)
            {
                album = result.Album;
                album.Url = ConstructBackUrl(request.AlbumId);

                if (!result.FolderId.HasDefaultValue())
                {
                    album.Images = Repository
                        .AsQueryable<MediaImage>()
                        .Where(i => i.Folder.Id == result.FolderId)
                        .Select(i => new ImageViewModel
                            {
                                Url = i.PublicUrl,
                                Caption = i.Caption ?? i.Title
                            })
                        .OrderBy(i => i.Caption)
                        .ToList();
                    album.ImagesCount = album.Images.Count;
                }
            }

            album.LoadCmsStyles = request.WidgetViewModel.GetOptionValue<bool>(ImageGallerModuleConstants.LoadCmsStylesWidgetOptionKey);

            return album;
        }

        /// <summary>
        /// Constructs the back url.
        /// </summary>
        /// <returns>Back URL</returns>
        private string ConstructBackUrl(Guid id)
        {
            var context = contextAccessor.GetCurrent();
            if (context != null && context.Request.Url != null)
            {
                var url = context.Request.Url.ToString();
                var parameter = string.Format("{0}={1}", ImageGallerModuleConstants.GalleryAlbumIdQueryParameterName, id.ToString()).ToLower();
                var index = url.ToLower().IndexOf(parameter, StringComparison.InvariantCulture);
                if (index >= 0)
                {
                    url = url.Replace(url.Substring(index, parameter.Length), string.Empty).TrimEnd('?').TrimEnd('&');
                }

                return url;
            }

            return "javascript:history.back()";
        }
    }
}