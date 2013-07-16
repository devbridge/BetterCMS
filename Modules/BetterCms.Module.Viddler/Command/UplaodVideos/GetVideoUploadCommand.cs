using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Services;
using BetterCms.Module.Viddler.ViewModels.UploadVideos;

namespace BetterCms.Module.Viddler.Command.UplaodVideos
{
    internal class GetVideoUploadCommand : CommandBase, ICommand<GetVideoUploadRequest, VideoUploadViewModel>
    {
        private readonly IViddlerService viddlerService;

        public GetVideoUploadCommand(IViddlerService viddlerService)
        {
            this.viddlerService = viddlerService;
        }

        public VideoUploadViewModel Execute(GetVideoUploadRequest request)
        {
            var sessionId = viddlerService.GetSessionId();
            var uploadData = viddlerService.GetUploadData(sessionId);

            var model = new VideoUploadViewModel
                {
                    RootFolderId = request.FolderId, 
                    ReuploadMediaId = request.ReuploadMediaId, 
                    SelectedFolderId = Guid.Empty,

                    SessionId = uploadData.SessionId,
                    Token = uploadData.Token,
                    Endpoint = uploadData.Endpoint,
                };

            var foldersQuery = Repository.AsQueryable<MediaFolder>().Where(f => f.Type == MediaType.Video);
            foldersQuery = request.FolderId == Guid.Empty
                ? foldersQuery.Where(f => f.Folder == null)
                : foldersQuery.Where(f => f.Folder.Id == request.FolderId);

            model.Folders = foldersQuery
                .OrderBy(f => f.Title)
                .Select(f => new Tuple<Guid, string>(f.Id, f.Title))
                .ToList();

            model.Folders.Insert(0, new Tuple<Guid, string>(Guid.Empty, ".."));

            return model;
        }
    }
}