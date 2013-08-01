namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaService
    {
        /// <summary>
        /// Notifies, when medias are deleted.
        /// </summary>
        /// <param name="medias">The array of medias.</param>
        void NotifiyMediaDeleted(System.Collections.Generic.IEnumerable<Models.Media> medias);

        /// <summary>
        /// Deletes sub items of media
        /// </summary>
        /// <param name="media">The media entity.</param>
        System.Collections.Generic.IList<Models.Media> DeleteSubMedias(Models.Media media);
    }
}