using BetterCms.Module.Root.Providers;

namespace BetterCms.Module.ImagesGallery.Providers
{
    public class ImageGalleryAlbumOptionProvider : ICustomOptionProvider
    {
        public const string Identifier = "images-gallery-album";

        /// <summary>
        /// Converts the value to correct type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Value, converted to correct type
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertValueToCorrectType(string value)
        {
            System.Guid guid;
            if (System.Guid.TryParse(value, out guid))
            {
                return guid;
            }

            return null;
        }

        /// <summary>
        /// Gets the default value for type.
        /// </summary>
        /// <returns>
        /// Default value for provider type
        /// </returns>
        public object GetDefaultValueForType()
        {
            return null;
        }
    }
}