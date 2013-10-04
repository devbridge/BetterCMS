using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbumForEdit
{
    public class GetAlbumForEditCommand : CommandBase, ICommand<Guid, AlbumEditViewModel>
    {
        public AlbumEditViewModel Execute(Guid request)
        {
            AlbumEditViewModel album;

            if (request.HasDefaultValue())
            {
                album = new AlbumEditViewModel();
            }
            else
            {
                album = Repository
                    .AsQueryable<Models.Album>()
                    .Where(a => a.Id == request)
                    .Select(a =>
                        new AlbumEditViewModel
                        {
                            Id = a.Id,
                            Version = a.Version,
                            Title = a.Title,
                            CoverImage = a.CoverImage == null || a.CoverImage.IsDeleted ? null :
                                  new ImageSelectorViewModel
                                  {
                                      ImageId = a.CoverImage.Id,
                                      ImageVersion = a.CoverImage.Version,
                                      ImageUrl = a.CoverImage.PublicUrl,
                                      ThumbnailUrl = a.CoverImage.PublicThumbnailUrl,
                                      ImageTooltip = a.CoverImage.Caption,
                                      FolderId = a.CoverImage.Folder != null ? a.CoverImage.Folder.Id : (Guid?)null
                                  },
                            Folder = a.Folder == null || a.Folder.IsDeleted ? null :
                                  new FolderSelectorViewModel
                                  {
                                      FolderId = a.Folder.Id,
                                      FolderTitle = a.Folder.Title,
                                      ParentFolderId = a.Folder.Folder != null ? a.Folder.Folder.Id : (Guid?)null
                                  },
                            FolderTitle = a.Folder == null || a.Folder.IsDeleted ? null : a.Folder.Title
                        })
                    .FirstOne();
            }

            return album;
        }
    }
}