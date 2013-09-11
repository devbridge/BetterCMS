namespace BetterCms.Module.MediaManager.Models.Extensions
{
    public static class MediaExtensions
    {
        public static Media CreateHistoryItem(this Media media)
        {
            var historyitem = media.Clone();
            historyitem.Original = media;
            return historyitem;
        }
    }
}