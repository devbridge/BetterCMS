using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.MediaManager.DownloadMedia
{
    /// <summary>
    /// Gets file info for downloading.
    /// </summary>
    public class DownloadFileCommand : CommandBase, ICommand<Guid, DownloadFileCommandResponse>
    {
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
                return new DownloadFileCommandResponse
                    {
                        FileName = file.FileUri.AbsolutePath,
                        ContentMimeType = System.Net.Mime.MediaTypeNames.Application.Octet, // Specify the generic octet-stream MIME type.
                        FileDownloadName = string.Format("{0}{1}", System.IO.Path.GetFileNameWithoutExtension(file.Title), file.OriginalFileExtension)
                    };
            }

            return null;
        }
    }
}