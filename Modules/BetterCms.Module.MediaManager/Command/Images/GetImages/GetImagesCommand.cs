using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;

namespace BetterCms.Module.MediaManager.Command.Images.GetImages
{
    public class GetImagesCommand : GetMediaItemsCommandBase<MediaImageViewModel, MediaImage>
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
        /// Selects the items.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        protected override QueryOverProjectionBuilder<MediaImage> SelectItems(QueryOverProjectionBuilder<MediaImage> builder)
        {
            return builder
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.Caption).WithAlias(() => modelAlias.Tooltip)
                    .Select(() => alias.OriginalFileExtension).WithAlias(() => modelAlias.FileExtension)
                    .Select(() => alias.PublicThumbnailUrl).WithAlias(() => modelAlias.ThumbnailUrl)
                    .Select(() => alias.PublicUrl).WithAlias(() => modelAlias.PublicUrl)
                    .Select(IsProcessing()).WithAlias(() => modelAlias.IsProcessing)
                    .Select(() => alias.Size).WithAlias(() => modelAlias.Size);
        }

        /// <summary>
        /// Gets the is processing conditions.
        /// </summary>
        /// <returns></returns>
        protected override ICriterion GetIsProcessingConditions()
        {
            return Restrictions.Where(() => !alias.IsUploaded || !alias.IsThumbnailUploaded || !alias.IsOriginalUploaded);
        }
    }
}