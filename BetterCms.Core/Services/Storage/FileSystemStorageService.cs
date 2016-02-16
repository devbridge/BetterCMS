// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemStorageService.cs" company="Devbridge Group LLC">
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

            var downloadResponse = new DownloadResponse();
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
