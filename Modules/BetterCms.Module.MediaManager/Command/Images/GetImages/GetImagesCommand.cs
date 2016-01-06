using System;
using System.Linq.Expressions;
using System.Web;

using BetterModules.Core.DataAccess;

using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Command.Images.GetImages
{
    public class GetImagesCommand : GetMediaItemsCommandBase<MediaImage>
    {
        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected override MediaType MediaType
        {
            get { return MediaType.Image; }
        }

        /// <summary>
        /// Creates the image view model.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <returns>Created image view model</returns>
        protected override MediaViewModel ToViewModel(Media media)
        {
            var image = media as MediaImage;
            if (image != null)
            {
                var model = new MediaImageViewModel();
                FillMediaFileViewModel(model, image);

                var dimensionsCalculator = new ImageDimensionsCalculator(image);

                model.Tooltip = image.Caption;
                model.ThumbnailUrl = FileUrlResolver.EnsureFullPathUrl(image.PublicThumbnailUrl + string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat)));
                model.IsProcessing = image.IsUploaded == null || image.IsThumbnailUploaded == null || image.IsOriginalUploaded == null;
                model.IsFailed = image.IsUploaded == false || image.IsThumbnailUploaded == false || image.IsOriginalUploaded == false;
                model.Height = dimensionsCalculator.ResizedCroppedHeight;
                model.Width = dimensionsCalculator.ResizedCroppedWidth;

                return model;
            }

            throw new InvalidOperationException("Cannot convert image media to image view model. Wrong entity passed.");
        }

        /// <summary>
        /// Appends the search filter.
        /// </summary>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>
        /// Appended search filter
        /// </returns>
        protected override Expression<Func<Media, bool>> AppendSearchFilter(Expression<Func<Media, bool>> searchFilter, string searchQuery)
        {
            var searcQueryDecoded = HttpUtility.UrlDecode(searchQuery);

            return searchFilter.Or(m => (m is MediaImage
                && ((MediaImage)m).PublicUrl.Contains(searchQuery)
                    || ((MediaImage)m).PublicUrl.Contains(searcQueryDecoded)
                    || ((MediaImage)m).Caption.Contains(searchQuery)));
        }
    }
}