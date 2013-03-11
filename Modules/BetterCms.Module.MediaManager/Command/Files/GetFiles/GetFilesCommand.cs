using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Command.Files.GetFiles
{
    public class GetFilesCommand : GetMediaItemsCommandBase<MediaFileViewModel, MediaFile>
    {
        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected override MediaType MediaType
        {
            get { return MediaType.File; }
        }

        /// <summary>
        /// Selects the items.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        protected override NHibernate.Criterion.Lambda.QueryOverProjectionBuilder<MediaFile> SelectItems(NHibernate.Criterion.Lambda.QueryOverProjectionBuilder<MediaFile> builder)
        {
            return builder.Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.OriginalFileExtension).WithAlias(() => modelAlias.FileExtension)
                    .Select(() => alias.PublicUrl).WithAlias(() => modelAlias.PublicUrl)
                    .Select(IsProcessing()).WithAlias(() => modelAlias.IsProcessing)
                    .Select(IsFailed()).WithAlias(() => modelAlias.IsFailed)
                    .Select(() => alias.Size).WithAlias(() => modelAlias.Size);
        }
    }
}