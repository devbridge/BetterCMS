using System;
using System.Globalization;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Images.GetImage
{
    /// <summary>
    /// Command to get media image data.
    /// </summary>
    public class GetImageCommand : CommandBase, ICommand<Guid, ImageViewModel>
    {
        public IMediaFileService MediaFileService { get; set; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <returns>The view model.</returns>
        public ImageViewModel Execute(Guid imageId)
        {
            return Repository.AsQueryable<MediaImage>()
                .Where(f => f.Id == imageId)
                .ToList()
                .Select(f => new ImageViewModel
                                 {                                     
                                     Id = f.Id.ToString(),
                                     Caption = f.Caption,
                                     Title = f.Title,
                                     Url = f.PublicUrl,
                                     Version = f.Version.ToString(CultureInfo.InvariantCulture),
                                     FileName = f.OriginalFileName,
                                     FileExtension = f.OriginalFileExtension,
                                     FileSize = MediaFileService.GetFileSizeText(f.Size),
                                     ImageWidth = f.Width,
                                     ImageHeight = f.Height,
                                     ImageAlign = f.ImageAlign.HasValue ? f.ImageAlign.Value : MediaImageAlign.Left,
                                     CropCoordX1 = f.CropCoordX1.HasValue ? f.CropCoordX1.Value.ToString(CultureInfo.InvariantCulture) : "0",
                                     CropCoordY1 = f.CropCoordY1.HasValue ? f.CropCoordY1.Value.ToString(CultureInfo.InvariantCulture) : "0",
                                     CropCoordX2 = f.CropCoordX2.HasValue ? f.CropCoordX2.Value.ToString(CultureInfo.InvariantCulture) : f.Width.ToString(CultureInfo.InvariantCulture),
                                     CropCoordY2 = f.CropCoordY2.HasValue ? f.CropCoordY2.Value.ToString(CultureInfo.InvariantCulture) : f.Height.ToString(CultureInfo.InvariantCulture),
                                     OriginalImageUrl = f.PublicOriginallUrl
                                 })
                .FirstOrDefault();
        }

        
    }
}