// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageHelper.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            if (!string.IsNullOrEmpty(extension))
            {
                extension = extension.Trim('.').ToLowerInvariant();
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