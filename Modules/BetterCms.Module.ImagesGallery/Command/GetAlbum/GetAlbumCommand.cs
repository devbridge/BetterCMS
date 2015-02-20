using System.Linq;

using BetterCms.Module.ImagesGallery.ViewModels;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbum
{
    public class GetAlbumCommand : CommandBase, ICommand<GetAlbumCommandRequest, AlbumViewModel>
    {
        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAlbumCommand" /> class.
        /// </summary>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetAlbumCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public AlbumViewModel Execute(GetAlbumCommandRequest request)
        {
            var imagesQuery = Repository.AsQueryable<MediaImage>();
            var isRoot = !request.FolderId.HasValue || request.FolderId.Value.HasDefaultValue();

            if (!isRoot)
            {
                var folderProxy = Repository.AsProxy<MediaFolder>(request.FolderId.Value);
                imagesQuery = imagesQuery.Where(f => f.Folder == folderProxy);
            }
            else
            {
                imagesQuery = imagesQuery.Where(f => f.Folder == null);
            }

            var result = imagesQuery
                .Where(i => i.Original == null && !i.IsDeleted && i.IsUploaded == true && !i.IsTemporary)
                .OrderBy(i => i.Title)
                .Select(i => new
                                 {
                                     Image = new ImageViewModel
                                                 {
                                                     Url = i.PublicUrl,
                                                     Caption = i.Caption ?? i.Title,
                                                     Title = i.Title
                                                 },
                                     ModifiedOn = i.ModifiedOn,
                                     FolderTitle = i.Folder != null ? i.Folder.Title : null
                                 })
                .ToList();

            AlbumViewModel album;
            if (result.Count > 0)
            {
                album = new AlbumViewModel
                            {
                                Title = result[0].FolderTitle ?? MediaGlobalization.RootFolder_Title,
                                LastUpdateDate = result.Max(i => i.ModifiedOn),
                                Images =  result.Select(i => i.Image).ToList(),
                                ImagesCount = result.Count
                            };
            }
            else
            {
                if (isRoot)
                {
                    album = new AlbumViewModel { Title = MediaGlobalization.RootFolder_Title };
                }
                else
                {
                    album = new AlbumViewModel
                                {
                                    Title = Repository
                                        .AsQueryable<MediaFolder>(f => f.Id == request.FolderId.Value)
                                        .Select(f => f.Title)
                                        .FirstOrDefault()
                                };
                }
            }

            album.LoadCmsStyles = request.WidgetViewModel.GetOptionValue<bool>(ImagesGalleryModuleConstants.OptionKeys.LoadCmsStyles);
            album.ImagesPerSection = request.WidgetViewModel.GetOptionValue<int>(ImagesGalleryModuleConstants.OptionKeys.ImagesPerSection);
            album.RenderHeader = request.RenderBackUrl || request.WidgetViewModel.GetOptionValue<bool>(ImagesGalleryModuleConstants.OptionKeys.RenderAlbumHeader);
            album.RenderBackUrl = request.RenderBackUrl;

            if (album.Images != null)
            {
                album.Images.ForEach(i => i.Url = fileUrlResolver.EnsureFullPathUrl(i.Url));
            }
            
            return album;
        }
    }
}