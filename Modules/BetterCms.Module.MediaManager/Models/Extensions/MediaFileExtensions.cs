using NHibernate;
using NHibernate.Criterion;

namespace BetterCms.Module.MediaManager.Models.Extensions
{
    public static class MediaFileExtensions
    {
        /// <summary>
        /// Gets the isProcessing field value depending on media file type.
        /// </summary>
        /// <param name="file">The media file entity.</param>
        /// <returns><c>true</c>, if file is still processing</returns>
        public static bool GetIsProcessing(this MediaFile file)
        {
            var image = file as MediaImage;
            if (image != null)
            {
                return image.IsUploaded == null || image.IsThumbnailUploaded == null || image.IsOriginalUploaded == null;
            }
            return file.IsUploaded == null;
        }
        
        /// <summary>
        /// Gets the sFailed field value depending on media file type.
        /// </summary>
        /// <param name="file">The media file entity.</param>
        /// <returns><c>true</c>, if file failed to process</returns>
        public static bool GetIsFailed(this MediaFile file)
        {
            var image = file as MediaImage;
            if (image != null)
            {
                return image.IsUploaded == false || image.IsThumbnailUploaded == false || image.IsOriginalUploaded == false;
            }
            return file.IsUploaded == false;
        }

        /// <summary>
        /// Gets the isProcessing field conditions.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>Conditional nHybernate projection</returns>
        public static IProjection GetIsProcessingConditions(this MediaFile alias)
        {
            return Projections.Conditional(GetIsProcessingConditionCriterion(alias),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));
        }
        
        /// <summary>
        /// Gets the isProcessing field conditions.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>Conditional nHybernate projection</returns>
        public static IProjection GetIsProcessingConditions(this MediaImage alias)
        {
            return Projections.Conditional(GetIsProcessingConditionCriterion(alias),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));
        }

        /// <summary>
        /// Gets the isFailed field conditions.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>Conditional nHybernate projection</returns>
        public static IProjection GetIsFailedConditions(this MediaFile alias)
        {
            return Projections.Conditional(GetIsFailedConditionCriterion(alias),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));
        }
        
        /// <summary>
        /// Gets the isFailed field conditions.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>Conditional nHybernate projection</returns>
        public static IProjection GetIsFailedConditions(this MediaImage alias)
        {
            return Projections.Conditional(GetIsFailedConditionCriterion(alias),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));
        }

        /// <summary>
        /// Gets the isProcessing field condition where restriction.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>nHybernate criterion</returns>
        private static ICriterion GetIsProcessingConditionCriterion(MediaFile alias)
        {
            return Restrictions.Where(() => alias.IsUploaded == null);
        }
        
        /// <summary>
        /// Gets the isProcessing field condition where restriction.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>nHybernate criterion</returns>
        private static ICriterion GetIsProcessingConditionCriterion(MediaImage alias)
        {
            return Restrictions.Where(() => alias.IsUploaded == null || alias.IsThumbnailUploaded == null || alias.IsOriginalUploaded == null);
        }

        /// <summary>
        /// Gets the isFailed field condition where restriction.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>nHybernate criterion</returns>
        private static ICriterion GetIsFailedConditionCriterion(MediaFile alias)
        {
            return Restrictions.Where(() => alias.IsUploaded == false);
        }
        
        /// <summary>
        /// Gets the isFailed field condition where restriction.
        /// </summary>
        /// <param name="alias">The table alias.</param>
        /// <returns>nHybernate criterion</returns>
        private static ICriterion GetIsFailedConditionCriterion(MediaImage alias)
        {
            return Restrictions.Where(() => alias.IsUploaded == false || alias.IsThumbnailUploaded == false || alias.IsOriginalUploaded == false);
        }

        public static string SizeAsText(this MediaFile image)
        {
            string[] sizes = { "bytes", "KB", "MB", "GB" };
            double fileSize = image.Size;
            var order = 0;
            while (fileSize >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                fileSize = fileSize / 1024;
            }

            return string.Format("{0:0.##} {1}", fileSize, sizes[order]);
        }
    }
}