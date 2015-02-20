using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;

using BetterModules.Core.Web.Web.EmbeddedResources;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Embedded resources accessor controller.
    /// </summary>
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class EmbeddedResourcesController : Controller
    {        
        /// <summary>
        /// Known embedded resource mime types.
        /// </summary>
        internal static readonly Dictionary<string, string> MimeTypes;

        /// <summary>
        /// Embedded resources provider.
        /// </summary>
        private readonly IEmbeddedResourcesProvider embeddedResourcesProvider;

        /// <summary>
        /// Initializes static members of the <see cref="EmbeddedResourcesController" /> class.
        /// </summary>
        static EmbeddedResourcesController()
        {
            MimeTypes = new Dictionary<string, string>
                {
                    { "js", "text/javascript" },
                    { "css", "text/css" },
                    { "gif", "image/gif" },
                    { "png", "image/png" },
                    { "ico", "image/ico" },
                    { "jpg", "image/jpeg" },
                    { "jpeg", "image/jpeg" },                    
                    { "txt", "text/plain" },
                    { "xml", "application/xml" },
                    { "zip", "application/zip" }
                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourcesController" /> class.
        /// </summary>
        /// <param name="embeddedResourcesProvider">The embedded resources provider.</param>
        public EmbeddedResourcesController(IEmbeddedResourcesProvider embeddedResourcesProvider)
        {
            this.embeddedResourcesProvider = embeddedResourcesProvider;
        }

        /// <summary>
        /// Indexes the specified resource name.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="folder1">The folder on level 1.</param>
        /// <param name="folder2">The folder on level 2.</param>
        /// <param name="folder3">The folder on level 3.</param>
        /// <param name="folder4">The folder on level 4.</param>
        /// <param name="folder5">The folder on level 5.</param>
        /// <param name="folder6">The folder on level 6.</param>
        /// <param name="file">The file name.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns>
        /// Embedded resource file.
        /// </returns>
        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.Any, Duration = 31536000, VaryByParam = "area;folder1;folder2;folder3;folder4;folder5;folder6;file;resourceType")]
        public ActionResult Index(string area = null, string folder1 = null, string folder2 = null, string folder3 = null, string folder4 = null, string folder5 = null, string folder6 = null, string file = null, string resourceType = null)
        {
            string contentType = GetContentType(resourceType);
            if (string.IsNullOrEmpty(contentType))
            {                
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            string[] folders = new[]
                                    {
                                        folder1, 
                                        folder2, 
                                        folder3, 
                                        folder4, 
                                        folder5,
                                        folder6
                                    };

            string virtualPath = "~/Areas/" + area + "/";
            for (int i = 0; i < folders.Length; i++)
            {
                if (!string.IsNullOrEmpty(folders[i]))
                {
                    virtualPath += folders[i] + "/";
                }
            }

            virtualPath += file + "." + resourceType;

            if (!embeddedResourcesProvider.IsEmbeddedResourceVirtualPath(virtualPath))
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            var virtualFile = embeddedResourcesProvider.GetEmbeddedResourceVirtualFile(virtualPath);
           
            return File(virtualFile.Open(), contentType);
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="resourceType">Name of the resource.</param>
        /// <returns>Mime type of resource.</returns>
        private static string GetContentType(string resourceType)
        {
            if (MimeTypes.ContainsKey(resourceType.ToLowerInvariant()))
            {
                return MimeTypes[resourceType];
            }
                        
            return null;
        }
    }
}