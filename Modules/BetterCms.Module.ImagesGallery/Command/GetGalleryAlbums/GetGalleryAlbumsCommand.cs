using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Web;

using BetterCms.Module.ImagesGallery.Controllers;
using BetterCms.Module.ImagesGallery.Providers;
using BetterCms.Module.ImagesGallery.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

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
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public GalleryViewModel Execute(RenderWidgetViewModel request)
        {
            IList<AlbumEditViewModel> albums;
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
                albums = repository
                    .AsQueryable<Models.Album>()
                    .Where(a => ids.Contains(a.Id))
                    .Select(a => new AlbumEditViewModel
                                     {
                                         Id = a.Id,
                                         Version = a.Version,
                                         Title = a.Title
                                     })
                    .ToList();
            }
            else
            {
                albums = new List<AlbumEditViewModel>();
            }

            return new GalleryViewModel
                       {
                           Albums = albums,
                           AlbumUrl = contextAccessor.ResolveActionUrl<GalleryController>(gc => gc.Album("{0}"))
                       };
        }
    }
}