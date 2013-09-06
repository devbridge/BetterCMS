using System;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.ViewModels.History;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.History.GetMediaVersion
{
    /// <summary>
    /// Command to get media preview.
    /// </summary>
    public class GetMediaVersionCommand : CommandBase, ICommand<Guid, MediaPreviewViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Media preview.</returns>
        public MediaPreviewViewModel Execute(Guid request)
        {
            var response = new MediaPreviewViewModel();

            var media = Repository
                .First<Media>(m => m.Id == request);


            var image = media as MediaImage;
            if (image != null)
            {
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Caption, image.Caption);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Title, image.Title);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Description, image.Description);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_FileSize, image.SizeAsText());
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_ImageDimensions, string.Format("{0} x {1}", image.Width, image.Height));
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_PublicUrl, image.PublicUrl, true);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_PublicThumbnailUrl, image.PublicThumbnailUrl, true);

                response.AddProperty(image.Caption, image.PublicUrl, isImageUrl: true);
            }

            var file = media as MediaFile;
            if (file != null)
            {
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Title, file.Title);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Description, file.Description);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_FileSize, file.SizeAsText());
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_PublicUrl, file.PublicUrl, true);

                if (media.Image != null)
                {
                    response.AddProperty(media.Image.Caption, media.Image.PublicUrl, isImageUrl: true);
                }
            }

            return response;
        }
    }
}