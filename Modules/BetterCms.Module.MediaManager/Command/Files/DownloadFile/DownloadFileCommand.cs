using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Command.MediaManager.DownloadMedia;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Files.DownloadFile
{
    /// <summary>
    /// Gets file info for downloading.
    /// </summary>
    public class DownloadFileCommand : CommandBase, ICommand<Guid, DownloadFileCommandResponse>
    {
        /// <summary>
        /// Gets or sets the storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>
        private readonly IStorageService storageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileCommand"/> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        public DownloadFileCommand(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        /// <summary>
        /// Executes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Response type of <see cref="DownloadFileCommandResponse"/></returns>
        public DownloadFileCommandResponse Execute(Guid id)
        {
            var file = Repository.FirstOrDefault<MediaFile>(f => f.Id == id && !f.IsDeleted);
            if (file != null)
            {
                var response = storageService.DownloadObject(file.FileUri);
                if (response != null)
                {
                    return new DownloadFileCommandResponse
                        {
                            FileStream = response.ResponseStream,
                            ContentMimeType = System.Net.Mime.MediaTypeNames.Application.Octet, // Specify the generic octet-stream MIME type.
                            FileDownloadName = string.Format("{0}{1}", System.IO.Path.GetFileNameWithoutExtension(file.Title), file.OriginalFileExtension)
                        };
                }
            }

            return null;
        }
    }
}