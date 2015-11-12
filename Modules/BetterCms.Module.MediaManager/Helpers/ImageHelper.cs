using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.MediaManager.Models.Enum;

namespace BetterCms.Module.MediaManager.Helpers
{
    public class ImageHelper
    {
        public static Image Resize(Image image, Size size)
        {
            var resizedImage = new Bitmap(size.Width, size.Height);
            using (image)
            {
                using (var graphic = Graphics.FromImage(resizedImage))
                {
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = CompositingQuality.HighQuality;

                    graphic.DrawImage(image, 0, 0, size.Width, size.Height);
                }
            }

            return resizedImage;
        }

        public static Image Crop(Image image, Rectangle area)
        {
            using (image)
            {
                using (var bitmap = new Bitmap(image))
                {
                    return bitmap.Clone(area, bitmap.PixelFormat);
                }
            }
        }

        public static ImageCodecInfo GetImageCodec(Image image)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            var format = image.RawFormat;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public static ImageType GetImageType(string extension)
        {
            if (extension != null)
            {
                extension = extension.Trim('.');
            }

            switch (extension)
            {
                case "gif":
                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                    return ImageType.Raster;
                case "svg":
                    return ImageType.Vector;
                default: 
                    const string message = "Image type not supported.";
                    throw new ValidationException(() => message, message);
            }
        }
    }
}