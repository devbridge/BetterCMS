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
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

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
            List<AlbumViewModelEx> albums;
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
                Models.Album albumAlias = null;
                MediaFolder folderAlias = null;
                MediaImage coverAlias = null;
                AlbumViewModelEx albumViewModel = null;

                albums = UnitOfWork.Session
                    .QueryOver(() => albumAlias)
                            .Left.JoinAlias(c => c.Folder, () => folderAlias)
                            .Left.JoinAlias(c => c.CoverImage, () => coverAlias)
                            .Where(() => !albumAlias.IsDeleted && !folderAlias.IsDeleted)
                            .Where(Restrictions.In(Projections.Property(() => albumAlias.Id), ids))
                            .SelectList(select => select
                                .Select(Projections.Cast(NHibernateUtil.String, Projections.Property<Models.Album>(c => c.Id))).WithAlias(() => albumViewModel.Url)
                                .Select(() => albumAlias.Title).WithAlias(() => albumViewModel.Title)
                                .Select(() => coverAlias.PublicUrl).WithAlias(() => albumViewModel.CoverImageUrl)
                                .Select(() => coverAlias.IsDeleted).WithAlias(() => albumViewModel.IsCoverImageDeleted)
                                .SelectSubQuery(
                                    QueryOver.Of<Media>()
                                        .Where(c => !c.IsDeleted)
                                        .And(c => c.Folder.Id == folderAlias.Id && c.Original == null)
                                        .ToRowCountQuery()
                                ).WithAlias(() => albumViewModel.ImagesCount)
                                .SelectSubQuery(
                                    QueryOver.Of<Media>()
                                        .Where(c => !c.IsDeleted)
                                        .And(c => c.Folder.Id == folderAlias.Id && c.Original == null)
                                        .Select(Projections.Max<Media>(c => c.ModifiedOn))
                                ).WithAlias(() => albumViewModel.LastUpdateDate)
                                .SelectSubQuery(
                                    QueryOver.Of<MediaImage>()
                                        .Where(c => !c.IsDeleted)
                                        .And(c => c.Folder.Id == folderAlias.Id && c.Original == null)
                                        .OrderBy(c => c.Title).Asc()
                                        .SelectList(media => media.Select(m => m.PublicUrl))
                                        .Take(1)
                                ).WithAlias(() => albumViewModel.FirstImageUrl)
                            )
                        .TransformUsing(Transformers.AliasToBean<AlbumViewModelEx>())
                        .List<AlbumViewModelEx>()
                        .ToList();

                albums.ForEach(
                    a =>
                        {
                            if (a.IsCoverImageDeleted)
                            {
                                a.CoverImageUrl = null;
                            }
                            if (string.IsNullOrEmpty(a.CoverImageUrl))
                            {
                                a.CoverImageUrl = a.FirstImageUrl;
                            }
                        });

                var urlPattern = GetAlbumUrlPattern();
                albums.ForEach(a => a.Url = string.Format(urlPattern, a.Url));
            }
            else
            {
                albums = new List<AlbumViewModelEx>();
            }

            return new GalleryViewModel
                       {
                           Albums = albums.Cast<AlbumViewModel>().ToList(),
                           LoadCmsStyles = request.GetOptionValue<bool>(ImagesGalleryModuleConstants.LoadCmsStylesWidgetOptionKey)
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
                url = string.Format("{0}{1}={2}", url, ImagesGalleryModuleConstants.GalleryAlbumIdQueryParameterName, "{0}");

                return url;
            }

            return null;
        }

        /// <summary>
        /// Album view model, extended with additional properties for filtering using QueryOver
        /// </summary>
        private class AlbumViewModelEx : AlbumViewModel
        {
            public bool IsCoverImageDeleted { get; set; }

            public string FirstImageUrl { get; set; }
        }
    }
}