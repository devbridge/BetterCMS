using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.DataServices
{
    public interface IMediaApiService
    {
        /// <summary>
        /// Gets the list of folder media entities.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="folderId">The folder id.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of folder media entities
        /// </returns>
        IList<Media> GetFolderMedias(MediaType mediaType,
            Guid? folderId = null,
            Expression<Func<Media, bool>> filter = null,
            Expression<Func<Media, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);

        /// <summary>
        /// Gets the list of media image entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        IList<MediaImage> GetImages(Expression<Func<MediaImage, bool>> filter = null,
            Expression<Func<MediaImage, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);

        /// <summary>
        /// Gets the list of media file entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of media file entities
        /// </returns>
        IList<MediaFile> GetFiles(Expression<Func<MediaFile, bool>> filter = null,
            Expression<Func<MediaFile, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);

        /// <summary>
        /// Gets the list of media folder entities.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of folder entities
        /// </returns>
        IList<MediaFolder> GetFolders(MediaType mediaType,
            Expression<Func<MediaFolder, bool>> filter = null,
            Expression<Func<MediaFolder, dynamic>> order = null,
            bool orderDescending = false,
            int? pageNumber = null,
            int? itemsPerPage = null);
    }
}
