using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using MvcContrib.Pagination;
using MvcContrib.Sorting;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.MediaManager.Command.Images
{
    public class GetImagesCommand : CommandBase, ICommand<MediaManagerViewModel, ImagesTabViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">A filter to search for specific images/folders.</param>
        /// <returns>A list of tags.</returns>
        public ImagesTabViewModel Execute(MediaManagerViewModel request)
        {
            request.SetDefaultSortingOptions("Title");

            ImagesTabViewModel model = new ImagesTabViewModel { GridOptions = request };

            model.Path = LoadMediaFolder(request);

            IEnumerable<MediaViewModel> results;
            var folders = GetFolders(request);
            var images = GetImages(request);
            if (request.Direction == SortDirection.Descending)
            {
                results = images.Cast<MediaViewModel>().Concat(folders);
            }
            else
            {
                results = folders.Cast<MediaViewModel>().Concat(images);
            }

            model.Items = new CustomPagination<MediaViewModel>(results, 1, 10, images.Count);

            return model;
        }

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list with folder view models
        /// </returns>
        private IList<MediaFolderViewModel> GetFolders(MediaManagerViewModel request)
        {
            MediaFolder alias = null;
            MediaFolderViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery));
            }
            if (!request.CurrentFolderId.HasDefaultValue())
            {
                query = query.Where(() => alias.Folder.Id == request.CurrentFolderId);
            }
            else
            {
                query = query.Where(() => alias.Folder == null);
            }
            if (request.FolderType.HasValue)
            {
                query = query.Where(() => alias.Type == request.FolderType.Value);
            }

            query = query.SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.Type).WithAlias(() => modelAlias.Type))
                .TransformUsing(Transformers.AliasToBean<MediaFolderViewModel>());

            var options = new SearchableGridOptions() { Direction = request.Direction, Column = "Title" };
            query.AddSortingAndPaging(options);

            var folders = query.List<MediaFolderViewModel>();
            return folders;
        }

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list with image view models
        /// </returns>
        private IList<MediaImageViewModel> GetImages(MediaManagerViewModel request)
        {
            MediaImage alias = null;
            MediaImageViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery));
            }
            if (!request.CurrentFolderId.HasDefaultValue())
            {
                query = query.Where(() => alias.Folder.Id == request.CurrentFolderId);
            }
            else
            {
                query = query.Where(() => alias.Folder == null);
            }
            if (request.FolderType.HasValue)
            {
                query = query.Where(() => alias.Type == request.FolderType.Value);
            }

            query = query.SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)

                    // TODO: ??? Which field should go there?
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Tooltip)
                    .Select(() => alias.Size).WithAlias(() => modelAlias.Size))

                    // TODO: ???
                    //.Select(() => alias.OriginalUri).WithAlias(() => modelAlias.PreviewUrl))
                .TransformUsing(Transformers.AliasToBean<MediaImageViewModel>());

            query.AddSortingAndPaging(request);

            var images = query.List<MediaImageViewModel>();
            return images;
        }

        /// <summary>
        /// Loads the media folder.
        /// </summary>
        /// <returns>Media folder view model</returns>
        private MediaPathViewModel LoadMediaFolder(MediaManagerViewModel request)
        {
            var emptyFolderViewModel = new MediaFolderViewModel { Id = Guid.Empty, Type = MediaType.Image };
            var model = new MediaPathViewModel
                            {
                                MediaType = MediaType.Image,
                                CurrentFolder = emptyFolderViewModel
                            };
            var folders = new List<MediaFolderViewModel> { emptyFolderViewModel };

            if (!request.CurrentFolderId.HasDefaultValue())
            {
                MediaFolder alias = null;
                MediaFolderViewModel modelAlias = null;

                var folder = UnitOfWork.Session
                    .QueryOver(() => alias)
                    .Where(() => !alias.IsDeleted && alias.Id == request.CurrentFolderId)
                    .SelectList(select => select
                        .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                        .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                        .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                        .Select(() => alias.Type).WithAlias(() => modelAlias.Type))
                    .TransformUsing(Transformers.AliasToBean<MediaFolderViewModel>())
                    .SingleOrDefault<MediaFolderViewModel>();

                model.CurrentFolder = folder ?? new MediaFolderViewModel();

                if (folder != null)
                {
                    folders.Add(new MediaFolderViewModel { Id = folder.Id, Name = folder.Name, Type = folder.Type });
                }
            }

            model.Folders = folders;
            return model;
        }
    }
}