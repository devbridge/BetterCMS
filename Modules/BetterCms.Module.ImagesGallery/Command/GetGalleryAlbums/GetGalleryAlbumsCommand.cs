using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Web;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.ImagesGallery.Command.GetGalleryAlbums
{
    public class GetGalleryAlbumsCommand : CommandBase, ICommand<RenderWidgetViewModel, GalleryViewModel>
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGalleryAlbumsCommand" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetGalleryAlbumsCommand(IHttpContextAccessor contextAccessor, IMediaFileUrlResolver fileUrlResolver)
        {
            this.contextAccessor = contextAccessor;
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public GalleryViewModel Execute(RenderWidgetViewModel request)
        {
            MediaFolder folderAlias = null;
            AlbumViewModel modelAlias = null;

            var dateSubQuery = QueryOver.Of<MediaImage>()
                .Where(x => x.Original == null)
                .And(x => !x.IsDeleted)
                .And(x => !x.IsTemporary)
                .And(x => x.IsUploaded == true)
                .And(x => x.Folder.Id == folderAlias.Id)
                .Select(Projections.Max(Projections.Property<MediaFolder>(c => c.ModifiedOn)));

            var countSubQuery = QueryOver.Of<MediaImage>()
                .Where(x => x.Original == null)
                .And(x => !x.IsDeleted)
                .And(x => !x.IsTemporary)
                .And(x => x.IsUploaded == true)
                .And(x => x.Folder.Id == folderAlias.Id)
                .ToRowCountQuery();

            var coverImageUrlSubQuery = QueryOver.Of<MediaImage>()
                .Where(x => x.Original == null)
                .And(x => !x.IsDeleted)
                .And(x => !x.IsTemporary)
                .And(x => x.IsUploaded == true)
                .And(x => x.Folder.Id == folderAlias.Id)
                .OrderBy(o => o.Title).Asc
                .Select(s => s.PublicUrl)
                .Take(1);

            var albumQuery = UnitOfWork.Session.QueryOver(() => folderAlias).Where(m => m.Type == MediaType.Image).And(m => !m.IsDeleted);

            var id = request.GetOptionValue<Guid?>(ImagesGalleryModuleConstants.OptionKeys.GalleryFolder);
            if (id.HasValue)
            {
                var folderProxy = Repository.AsProxy<MediaFolder>(id.Value);
                albumQuery = albumQuery.And(f => f.Folder == folderProxy);
            }
            else
            {
                albumQuery = albumQuery.And(f => f.Folder == null);
            }

            // Load list of albums
            List<AlbumViewModel> albums = albumQuery
                .SelectList(select => select
                    .Select(Projections.Cast(NHibernateUtil.String, Projections.Property<MediaFolder>(c => c.Id))).WithAlias(() => modelAlias.Url)
                    .Select(() => folderAlias.Title).WithAlias(() => modelAlias.Title)
                    .SelectSubQuery(countSubQuery).WithAlias(() => modelAlias.ImagesCount)
                    .SelectSubQuery(dateSubQuery).WithAlias(() => modelAlias.LastUpdateDate)
                    .SelectSubQuery(coverImageUrlSubQuery).WithAlias(() => modelAlias.CoverImageUrl)
                )
                .TransformUsing(Transformers.AliasToBean<AlbumViewModel>())
                .List<AlbumViewModel>().ToList();

            var urlPattern = GetAlbumUrlPattern(request);
            albums.ForEach(a =>
                        {
                            a.Url = string.Format(urlPattern, a.Url);
                            a.CoverImageUrl = fileUrlResolver.EnsureFullPathUrl(a.CoverImageUrl);
                        });

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