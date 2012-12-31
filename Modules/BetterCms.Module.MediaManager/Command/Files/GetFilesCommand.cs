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
using NHibernate.Transform;

namespace BetterCms.Module.MediaManager.Command.Files
{
    public class GetFilesCommand : CommandBase, ICommand<MediaManagerViewModel, MediaManagerItemsViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">A filter to search for specific files/folders.</param>
        /// <returns>A list of tags.</returns>
        public MediaManagerItemsViewModel Execute(MediaManagerViewModel request)
        {
            request.SetDefaultSortingOptions("Title");

            IEnumerable<MediaViewModel> results;
            var folders = GetFolders(request);
            var files = GetFiles(request);
            if (request.Direction == SortDirection.Descending)
            {
                results = files.Cast<MediaViewModel>().Concat(folders);
            }
            else
            {
                results = folders.Cast<MediaViewModel>().Concat(files);
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
            MediaFolder alias = null;
            MediaFolderViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Type == MediaType.File);

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
        /// <param name="model">The model.</param>
        /// <returns>
        /// The list with file view models
        /// </returns>
        private IList<MediaFileViewModel> GetFiles(MediaManagerViewModel model)
        {
            MediaFile alias = null;
            MediaFileViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Type == MediaType.File);

            if (!string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", model.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery));
            }
            if (!model.CurrentFolderId.HasDefaultValue())
            {
                query = query.Where(() => alias.Folder.Id == model.CurrentFolderId);
            }
            else
            {
                query = query.Where(() => alias.Folder == null);
            }

            query = query.SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.Size).WithAlias(() => modelAlias.Size))
                .TransformUsing(Transformers.AliasToBean<MediaFileViewModel>());

            query.AddSortingAndPaging(model);

            var files = query.List<MediaFileViewModel>();
            return files;
        }

        /// <summary>
        /// Loads the media folder.
        /// </summary>
        /// <returns>Media folder view model</returns>
        private MediaPathViewModel LoadMediaFolder(MediaManagerViewModel request)
        {
            var emptyFolderViewModel = new MediaFolderViewModel { Id = Guid.Empty, Type = MediaType.File };
            var model = new MediaPathViewModel
            {
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