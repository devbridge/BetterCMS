using System;

using BetterCms.Module.MediaManager.Command.Images.GetImage;
using BetterCms.Module.MediaManager.ViewModels.Images;

namespace BetterCms.Module.MediaManager.Command.Images.GetImageProperties
{
    public class GetImagePropertiesCommand : GetImageCommand
    {
        public override ImageViewModel Execute(Guid imageId)
        {
            var viewModel = base.Execute(imageId);
            viewModel.OriginalImageUrl = viewModel.Url;

            var xRatio = (decimal)viewModel.ImageWidth / (viewModel.OriginalImageWidth);
            var yRatio = (decimal)viewModel.ImageHeight / (viewModel.OriginalImageHeight);

            var cropX1 = (int)Math.Floor(viewModel.CropCoordX1 * xRatio);
            var cropY1 = (int)Math.Floor(viewModel.CropCoordY1 * yRatio);
            var cropX2 = (int)Math.Floor(viewModel.CropCoordX2 * xRatio);
            var cropY2 = (int)Math.Floor(viewModel.CropCoordY2 * yRatio);

            viewModel.OriginalImageWidth = viewModel.ImageWidth = cropX2 - cropX1;
            viewModel.OriginalImageHeight = viewModel.ImageHeight = cropY2 - cropY1;
            viewModel.CropCoordX1 = 0;
            viewModel.CropCoordY1 = 0;
            viewModel.CropCoordX2 = viewModel.ImageWidth;
            viewModel.CropCoordY2 = viewModel.ImageHeight;

            return viewModel;
        }
    }
}