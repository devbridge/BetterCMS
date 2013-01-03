using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using MvcContrib.Sorting;

using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace BetterCms.Module.MediaManager.Command.MediaManager
{
    public abstract class GetMediaItemsCommandBase<TModel, TEntity> : CommandBase, ICommand<MediaManagerViewModel, MediaManagerItemsViewModel>
        where TModel: MediaViewModel
        where TEntity: MediaFile
    {
        /// <summary>
        /// The model alias
        /// </summary>
        protected TModel modelAlias;

        /// <summary>
        /// The database entity alias
        /// </summary>
        protected TEntity alias;

        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected abstract MediaType MediaType { get; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">A filter to search for specific media items/folders.</param>
        /// <returns>A list of tags.</returns>
        public MediaManagerItemsViewModel Execute(MediaManagerViewModel request)
        {
            request.SetDefaultSortingOptions("Title");

            IEnumerable<MediaViewModel> results;
            var folders = GetFolders(request);
            var items = GetItemsList(request);
            if (request.Direction == SortDirection.Descending)
            {
                results = items.Cast<MediaViewModel>().Concat(folders);
            }
            else
            {
                results = folders.Cast<MediaViewModel>().Concat(items);
            }

            var model = new MediaManagerItemsViewModel(results, request, results.Count());
            model.Path = LoadMediaFolder(request);

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
            MediaFolder folderAlias = null;
            MediaFolderViewModel folderModelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => folderAlias)
                .Where(() => !folderAlias.IsDeleted && folderAlias.Type == MediaType);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => folderAlias.Title), searchQuery));
            }
            if (!request.CurrentFolderId.HasDefaultValue())
            {
                query = query.Where(() => folderAlias.Folder.Id == request.CurrentFolderId);
            }
            else
            {
                query = query.Where(() => folderAlias.Folder == null);
            }

            query = query.SelectList(select => select
                    .Select(() => folderAlias.Id).WithAlias(() => folderModelAlias.Id)
                    .Select(() => folderAlias.Title).WithAlias(() => folderModelAlias.Name)
                    .Select(() => folderAlias.Version).WithAlias(() => folderModelAlias.Version)
                    .Select(() => folderAlias.Type).WithAlias(() => folderModelAlias.Type))
                .TransformUsing(Transformers.AliasToBean<MediaFolderViewModel>());

            var options = new SearchableGridOptions() { Direction = request.Direction, Column = "Title" };
            query.AddSortingAndPaging(options);

            var folders = query.List<MediaFolderViewModel>();
            return folders;
        }

        /// <summary>
        /// Loads the media folder.
        /// </summary>
        /// <returns>Media folder view model</returns>
        private MediaPathViewModel LoadMediaFolder(MediaManagerViewModel request)
        {
            var emptyFolderViewModel = new MediaFolderViewModel { Id = Guid.Empty, Type = MediaType };
            var model = new MediaPathViewModel
                            {
                                CurrentFolder = emptyFolderViewModel
                            };
            var folders = new List<MediaFolderViewModel> { emptyFolderViewModel };

            if (!request.CurrentFolderId.HasDefaultValue())
            {
                MediaFolder folderAlias = null;
                MediaFolderViewModel folderModelAlias = null;

                var folder = UnitOfWork.Session
                    .QueryOver(() => folderAlias)
                    .Where(() => !folderAlias.IsDeleted && folderAlias.Id == request.CurrentFolderId)
                    .SelectList(select => select
                        .Select(() => folderAlias.Id).WithAlias(() => folderModelAlias.Id)
                        .Select(() => folderAlias.Title).WithAlias(() => folderModelAlias.Name)
                        .Select(() => folderAlias.Version).WithAlias(() => folderModelAlias.Version)
                        .Select(() => folderAlias.Type).WithAlias(() => folderModelAlias.Type))
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

        /// <summary>
        /// Gets the media items list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Media items list</returns>
        private IList<TModel> GetItemsList(MediaManagerViewModel request)
        {
            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Type == MediaType && !alias.IsTemporary);

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

            query = query
                .SelectList(SelectItems)
                .TransformUsing(Transformers.AliasToBean<TModel>());

            query.AddSortingAndPaging(request);

            var items = query.List<TModel>();
            return items;
        }

        /// <summary>
        /// Media items selector.
        /// </summary>
        /// <param name="builder">Media items projection builder.</param>
        /// <returns>Media items projection builder</returns>
        protected virtual QueryOverProjectionBuilder<TEntity> SelectItems(QueryOverProjectionBuilder<TEntity> builder)
        {
            return builder
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.FileExtension).WithAlias(() => modelAlias.FileExtension)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version);
        }
    }
}