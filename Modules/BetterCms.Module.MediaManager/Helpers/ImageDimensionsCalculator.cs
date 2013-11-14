using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Helpers
{
    /// <summary>
    /// Helper class for calculating image dimensions, when image is being resized / cropped
    /// </summary>
    public class ImageDimensionsCalculator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageDimensionsCalculator" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public ImageDimensionsCalculator(MediaImage image)
            : this(image.Width, image.Height, image.OriginalWidth, image.OriginalHeight, image.CropCoordX1, image.CropCoordX2, image.CropCoordY1, image.CropCoordY2)
        {
        }

        public ImageDimensionsCalculator(int width, int height, int originalWidht, int originalHeight,
            int? cropX1 = null, int? cropX2 = null, int? cropY1 = null, int? cropY2 = null)
        {
            Width = width;
            Height = height;
            OriginalWidth = originalWidht;
            OriginalHeight = originalHeight;
            CropCoordX1 = cropX1;
            CropCoordX2 = cropX2;
            CropCoordY1 = cropY1;
            CropCoordY2 = cropY2;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the original.
        /// </summary>
        /// <value>
        /// The width of the original.
        /// </value>
        public int OriginalWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the original.
        /// </summary>
        /// <value>
        /// The height of the original.
        /// </value>
        public int OriginalHeight { get; set; }

        /// <summary>
        /// Gets or sets the crop coord x1.
        /// </summary>
        /// <value>
        /// The crop coord x1.
        /// </value>
        public int? CropCoordX1 { get; set; }

        /// <summary>
        /// Gets or sets the crop coord x2.
        /// </summary>
        /// <value>
        /// The crop coord x2.
        /// </value>
        public int? CropCoordX2 { get; set; }

        /// <summary>
        /// Gets or sets the crop coord y1.
        /// </summary>
        /// <value>
        /// The crop coord y1.
        /// </value>
        public int? CropCoordY1 { get; set; }

        /// <summary>
        /// Gets or sets the crop coord y2.
        /// </summary>
        /// <value>
        /// The crop coord y2.
        /// </value>
        public int? CropCoordY2 { get; set; } 

        /// <summary>
        /// Gets the width of the resized and cropped image.
        /// </summary>
        /// <value>
        /// The width of the resized and cropped image.
        /// </value>
        public int ResizedCroppedWidth
        {
            get
            {
                return CalculateLength(CropCoordX1, CropCoordX2, Width, OriginalWidth);
            }
        }

        /// <summary>
        /// Gets the height of the resized and cropped image.
        /// </summary>
        /// <value>
        /// The height of the resized and cropped image.
        /// </value>
        public int ResizedCroppedHeight
        {
            get
            {
                return CalculateLength(CropCoordY1, CropCoordY2, Height, OriginalHeight);
            }
        }

        /// <summary>
        /// Gets the resized crop coord x1.
        /// </summary>
        /// <value>
        /// The resized crop coord x1.
        /// </value>
        public int? ResizedCropCoordX1
        {
            get
            {
                if (CropCoordX1.HasValue)
                {
                    var ratio = GetRatio(Width, OriginalWidth);

                    return (int)Math.Floor(CropCoordX1.Value * ratio);
                }

                return null;
            }
        }
        
        /// <summary>
        /// Gets the resized crop coord x2.
        /// </summary>
        /// <value>
        /// The resized crop coord x2.
        /// </value>
        public int? ResizedCropCoordX2
        {
            get
            {
                if (CropCoordX2.HasValue)
                {
                    var ratio = GetRatio(Width, OriginalWidth);

                    return (int)Math.Floor(CropCoordX2.Value * ratio);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the resized crop coord y1.
        /// </summary>
        /// <value>
        /// The resized crop coord y1.
        /// </value>
        public int? ResizedCropCoordY1
        {
            get
            {
                if (CropCoordY1.HasValue)
                {
                    var ratio = GetRatio(Height, OriginalHeight);

                    return (int)Math.Floor(CropCoordY1.Value * ratio);
                }

                return null;
            }
        }
        
        /// <summary>
        /// Gets the resized crop coord y2.
        /// </summary>
        /// <value>
        /// The resized crop coord y2.
        /// </value>
        public int? ResizedCropCoordY2
        {
            get
            {
                if (CropCoordY2.HasValue)
                {
                    var ratio = GetRatio(Height, OriginalHeight);

                    return (int)Math.Floor(CropCoordY2.Value * ratio);
                }

                return null;
            }
        }

        /// <summary>
        /// Calculates the length.
        /// </summary>
        /// <param name="crop1">The crop1.</param>
        /// <param name="crop2">The crop2.</param>
        /// <param name="length">The length.</param>
        /// <param name="originalLength">Length of the original.</param>
        /// <returns>Calculated length, depending on cropped values and resized values</returns>
        private static int CalculateLength(int? crop1, int? crop2, int length, int originalLength)
        {
            if (crop1.HasValue && crop2.HasValue)
            {
                if (length != originalLength)
                {
                    var ratio = GetRatio(length, originalLength);

                    crop1 = (int)Math.Floor(crop1.Value * ratio);
                    crop2 = (int)Math.Floor(crop2.Value * ratio);

                    return crop2.Value - crop1.Value;
                }

                return crop2.Value - crop1.Value;
            }

            return length;
        }

        /// <summary>
        /// Gets the calculated ratio for width / height.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="originalLength">Length of the original.</param>
        /// <returns>Calculated ratio for width / height</returns>
        private static decimal GetRatio(int length, int originalLength)
        {
            if (length != originalLength)
            {
                return (decimal)length / (originalLength == 0 ? length : originalLength);
            }

            return 1;
        }
    }
}