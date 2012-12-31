using System;
using System.IO;
using System.Web;

using BetterCms.Configuration;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaFileService : IMediaFileService
    {
        private readonly IStorageService storageService;

        private readonly ICmsConfiguration configuration;

        public DefaultMediaFileService(IStorageService storageService, ICmsConfiguration configuration)
        {
            this.storageService = storageService;
            this.configuration = configuration;
        }

        public void RemoveFile(Guid fileId, int version)
        {
            throw new NotImplementedException();
        }

        /*public MediaFile UploadTemporaryFile(string fileName, Stream inputStream)
        {
            string root = configuration.Storage.ContentRoot;
            if (configuration.Storage.ServiceType == StorageServiceType.FileSystem && VirtualPathUtility.IsAppRelative(root))
            {
                root = 
            }

            string path = Path.Combine(root, "temp", fileName);

            storageService.UploadObject(new UploadRequest
                                            {
                                                Uri = new Uri(path),
                                                InputStream = inputStream                                                
                                            });
        }*/
    }
}