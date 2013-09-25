using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbumImages
{
    public class GetAlbumImagesCommand : CommandBase, ICommand<Guid, AlbumEditViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public AlbumEditViewModel Execute(Guid request)
        {
            return new AlbumEditViewModel();
        }
    }
}