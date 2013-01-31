using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Images;

namespace BetterCms.Module.MediaManager.Command.Images.SaveImage
{
    public class SaveImageDataCommand : CommandBase, ICommand<ImageViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        public void Execute(ImageViewModel request)
        {
            var mediaImage = Repository
                .AsQueryable<MediaImage>()
                .FirstOrDefault(f => f.Id == request.Id.ToGuidOrDefault());

            if (mediaImage == null)
            {
                throw new CmsException(string.Format("Image was not found by id={0}.", request.Id));
            }

            mediaImage.Caption = request.Caption;
            mediaImage.Title = request.Title;
            mediaImage.ImageAlign = request.ImageAlign;
            mediaImage.Width = request.ImageWidth;
            mediaImage.Height = request.ImageHeight;
            mediaImage.Version = request.Version.ToIntOrDefault();

            // Set crop options
            if (request.CropCoordX1 == 0
                && request.CropCoordY1 == 0
                && request.CropCoordX2 == mediaImage.OriginalWidth
                && request.CropCoordY2 == mediaImage.OriginalHeight)
            {
                mediaImage.CropCoordX1 = null;
                mediaImage.CropCoordY1 = null;
                mediaImage.CropCoordX2 = null;
                mediaImage.CropCoordY2 = null;
            }
            else
            {
                mediaImage.CropCoordX1 = request.CropCoordX1;
                mediaImage.CropCoordY1 = request.CropCoordY1;
                mediaImage.CropCoordX2 = request.CropCoordX2;
                mediaImage.CropCoordY2 = request.CropCoordY2;
            }

            Repository.Save(mediaImage);
            UnitOfWork.Commit();
        }
    }
}
