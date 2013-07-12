namespace BetterCms.Module.Vimeo.Services.Models
{
    public class ThumbnailList : ListBase
    {
        public Thumbnail[] Thumbnail { get; set; }
    }

    internal static class ThumbnailListExtensions
    {
        public static string GetThumbnailUrl(this ThumbnailList list, int maxWidth, int maxHight)
        {
            if (list == null)
            {
                return string.Empty;
            }
            
            Thumbnail result = null;
            foreach (var thumbnail in list.Thumbnail)
            {
                if (result == null)
                {
                    result = thumbnail;
                }
                else
                {
                    if (result.Width <= thumbnail.Width && result.Height <= thumbnail.Height &&
                        thumbnail.Width <= maxWidth && thumbnail.Height <= maxHight)
                    {
                        result = thumbnail;
                    }
                }
            }

            return result != null ? result._Content : string.Empty;
        }
    }
}