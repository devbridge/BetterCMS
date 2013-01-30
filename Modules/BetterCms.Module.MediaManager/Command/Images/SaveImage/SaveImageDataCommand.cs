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
            mediaImage.Version = request.Version.ToIntOrDefault();
            
            Repository.Save(mediaImage);
            UnitOfWork.Commit();
        }
    }
}
