using System;

using BetterCms.Module.MediaManager.Controllers;

using BetterModules.Core.Web.Web;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaFileUrlResolver : IMediaFileUrlResolver
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaFileUrlResolver" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public DefaultMediaFileUrlResolver(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the media file full URL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="publicUrl">The public URL.</param>
        /// <returns></returns>
        public string GetMediaFileFullUrl(Guid id, string publicUrl)
        {
            return contextAccessor.ResolveActionUrl<FilesController>(f => f.Download(id.ToString()), true);
        }

        public string EnsureFullPathUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("/"))
            {
                return contextAccessor.MapPublicPath(url);
            }

            return url;
        }
    }
}