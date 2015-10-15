using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Service;

namespace BetterCms.Core.Services.Storage
{
    public class FileSystemStorageService : IStorageService
    {
        public bool ObjectExists(Uri uri)
        {
            CheckUri(uri);
                        
            return File.Exists(uri.LocalPath) || Directory.Exists(uri.LocalPath);
        }

        public void UploadObject(UploadRequest request)
        {
            CheckUri(request.Uri);

            string pathRoot = Path.GetDirectoryName(request.Uri.LocalPath);
            if (pathRoot != null && !Directory.Exists(pathRoot))
            {
                Directory.CreateDirectory(pathRoot);
            }

            using (FileStream writeStream = new FileStream(request.Uri.LocalPath, FileMode.Create, FileAccess.Write))
            {
                request.InputStream.Seek(0, SeekOrigin.Begin);
                request.InputStream.CopyTo(writeStream);
                writeStream.Flush(true);
                writeStream.Close();
            }
        }

        public DownloadResponse DownloadObject(Uri uri)
        {
            CheckUri(uri);

            DownloadResponse downloadResponse = new DownloadResponse();
            downloadResponse.Uri = uri;

            using (FileStream writeStream = new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read))
            {
                downloadResponse.ResponseStream = new MemoryStream();
                writeStream.CopyTo(downloadResponse.ResponseStream);
                downloadResponse.ResponseStream.Seek(0, SeekOrigin.Begin);

                writeStream.Flush();
                writeStream.Close();
            }

            return downloadResponse;
        }

        public void CopyObject(Uri sourceUri, Uri destinationUri)
        {
            CheckUri(sourceUri);
            CheckUri(destinationUri);

            var cleanedDestinationPath = destinationUri.LocalPath.Remove(destinationUri.LocalPath.TrimEnd('\\', '/').LastIndexOfAny(new[] { '\\', '/' }));
            Directory.CreateDirectory(cleanedDestinationPath);
            using (FileStream readStream = new FileStream(sourceUri.LocalPath, FileMode.Open, FileAccess.Read))
            {
                using (FileStream writeStream = new FileStream(destinationUri.LocalPath, FileMode.Create, FileAccess.Write))
                {
                    readStream.CopyTo(writeStream);
                    readStream.Flush();
                    writeStream.Flush(true);
                    readStream.Close();
                    writeStream.Close();                    
                }                
            }
        }

        public void RemoveObject(Uri uri)
        {
            CheckUri(uri);
            if (File.Exists(uri.LocalPath))
            {
                File.Delete(uri.LocalPath);
            }
        }

        public void RemoveFolder(Uri uri)
        {
            CheckUri(uri);
            
            string pathRoot = Path.GetDirectoryName(uri.LocalPath);
            if (pathRoot != null && Directory.Exists(pathRoot))
            {
                Directory.Delete(pathRoot);
            }
        }

        private void CheckUri(Uri uri)
        {            
            if (!Uri.CheckSchemeName(uri.Scheme) || !uri.Scheme.Equals(Uri.UriSchemeFile))
            {
                throw new StorageException(string.Format("An Uri scheme {0} is invalid. Uri {1} can't be processed with a {2} storage service.", uri.Scheme, uri, GetType().Name));
            }
        }

        public bool SecuredUrlsEnabled
        {
            get { return false; }
        }

        public string SecuredContainerIssueWarning
        {
            get { return null; }
        }

        public string GetSecuredUrl(Uri uri)
        {
            throw new CmsException("File system storage doesn't support token based security.");
        }
    }
}
