using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;

using BetterModules.Core.DataAccess;

using NHibernate.Linq;

namespace BetterCms.Module.MediaManager.Helpers
{
    public class MediaHelper
    {
        public static void SetCategories(IRepository repository, Media sourceMedia, Media destinationMedia)
        {
            var destination = destinationMedia as ICategorized;
            var source = sourceMedia as ICategorized;

            if (destination == null || source == null)
            {
                return;
            }

            if (destination.Categories != null)
            {
                var categoriesToRemove = destination.Categories.ToList();
                categoriesToRemove.ForEach(repository.Delete);
            }

            if (source.Categories == null)
            {
                return;
            }

            source.Categories.ForEach(destination.AddCategory);
            if (destination.Categories != null)
            {
                destination.Categories.ForEach(e => e.SetEntity(destinationMedia));
            }
        }

        public static void SetTags(IRepository repository, Media sourceMedia, Media destinationMedia)
        {
            var destination = destinationMedia;
            var source = sourceMedia;

            if (destination == null || source == null)
            {
                return;
            }

            if (destination.MediaTags != null)
            {
                var tagsToRemove = destination.MediaTags.ToList();
                tagsToRemove.ForEach(repository.Delete);
            }

            if (source.MediaTags == null)
            {
                return;
            }

            source.MediaTags.ForEach(destination.AddTag);
            if (destination.MediaTags != null)
            {
                foreach (var mediaTag in destination.MediaTags)
                {
                    mediaTag.Media = destinationMedia;
                }
            }
        }

        public static void SetCollections(IRepository repository, Media sourceMedia, Media destinationMedia)
        {
            SetCategories(repository, sourceMedia, destinationMedia);
            SetTags(repository, sourceMedia, destinationMedia);
        }
    }
}