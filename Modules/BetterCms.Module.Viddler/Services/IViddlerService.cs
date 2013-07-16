using BetterCms.Module.Viddler.Services.Models;

namespace BetterCms.Module.Viddler.Services
{
    /// <summary>
    /// Viddler service interface.
    /// </summary>
    internal interface IViddlerService
    {
        /// <summary>
        /// Gets the player URL.
        /// </summary>
        /// <param name="videoId">The video id.</param>
        /// <returns>URL for using in iFrame.</returns>
        string GetPlayerUrl(string videoId);

        /// <summary>
        /// Gets the video URL.
        /// </summary>
        /// <param name="videoId">The video id.</param>
        /// <returns>URL to access video on the website.</returns>
        string GetVideoUrl(string videoId);

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <returns>Session id.</returns>
        string GetSessionId();

        /// <summary>
        /// Gets the upload data.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>Data for video upload.</returns>
        Upload GetUploadData(string sessionId);

        /// <summary>
        /// Gets the video details.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="videoId">The video id.</param>
        /// <returns>Video details.</returns>
        Video GetVideoDetails(string sessionId, string videoId);

        /// <summary>
        /// Makes the video public.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="videoId">The video id.</param>
        /// <param name="makePublic">if set to <c>true</c> to make public or <c>false</c> to make private.</param>
        /// <returns><c>true</c> if video is public, <c>false</c> otherwise.</returns>
        bool MakeVideoPublic(string sessionId, string videoId, bool makePublic);

        /// <summary>
        /// Removes the video.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="videoId">The video id.</param>
        /// <returns><c>true</c> if successfully removed, <c>false</c> otherwise.</returns>
        bool RemoveVideo(string sessionId, string videoId);

        /// <summary>
        /// Gets the uploading status.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="token">The token.</param>
        /// <returns>Percentage video upload status.</returns>
        int GetUploadingStatus(string sessionId, string token);
    }
}