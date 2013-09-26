using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbum
{
    public class GetAlbumCommand : CommandBase, ICommand<Guid, AlbumEditViewModel>
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
                            CoverImage = a.CoverImage == null ? null :
                                  new ImageSelectorViewModel
                                  {
                                      ImageId = a.CoverImage.Id,
                                      ImageVersion = a.CoverImage.Version,
                                      ImageUrl = a.CoverImage.PublicUrl,
                                      ThumbnailUrl = a.CoverImage.PublicThumbnailUrl,
                                      ImageTooltip = a.CoverImage.Caption
                                  },
                        })
                    .FirstOne();
            }

            return album;
        }
    }
}