using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.ImagesGallery.Models;
using BetterCms.Module.ImagesGallery.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbumList
{
    public class GetAlbumListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<AlbumEditViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<AlbumEditViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<AlbumEditViewModel> model;

            request.SetDefaultSortingOptions("Title");

            var query = Repository
                .AsQueryable<Album>();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(a => a.Title.Contains(request.SearchQuery));
            }

            var albums = query
                .Select(album =>
                    new AlbumEditViewModel
                        {
                            Id = album.Id,
                            Version = album.Version,
                            Title = album.Title
                        });

            var count = query.ToRowCountFutureValue();
            albums = albums.AddSortingAndPaging(request);

            model = new SearchableGridViewModel<AlbumEditViewModel>(albums.ToList(), request, count.Value);

            return model;
        }
    }
}