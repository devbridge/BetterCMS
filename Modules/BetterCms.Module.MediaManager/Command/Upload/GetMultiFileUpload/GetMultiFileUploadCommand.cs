using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Upload;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload
{
    public class GetMultiFileUploadCommand : CommandBase, ICommand<GetMultiFileUploadRequest, MultiFileUploadViewModel>
    {
        public ICmsConfiguration CmsConfiguration { get; set; }

        public MultiFileUploadViewModel Execute(GetMultiFileUploadRequest request)
        {
            var model = new MultiFileUploadViewModel();
            model.RootFolderId = request.FolderId;
            model.RootFolderType = request.Type;
            model.ReuploadMediaId = request.ReuploadMediaId;
            model.SelectedFolderId = Guid.Empty;
            model.UploadedFiles = null;

            var foldersQuery = Repository.AsQueryable<MediaFolder>().Where(f => f.Type == request.Type);
            if (request.FolderId == Guid.Empty)
            {
                foldersQuery = foldersQuery.Where(f => f.Folder == null);
            }
            else
            {
                foldersQuery = foldersQuery.Where(f => f.Folder.Id == request.FolderId);
            }

            model.Folders = foldersQuery
                .OrderBy(f => f.Title)
                .Select(f => new Tuple<Guid, string>(f.Id, f.Title))
                .ToList();

            model.Folders.Insert(0, new Tuple<Guid, string>(Guid.Empty, ".."));

            // TODO: Generate default access based on configuration
            var defaultAccessLevel = new UserAccessViewModel
                                         {
                                             RoleOrUser = "Anonymous",
                                             AccessLevel = AccessLevel.Deny
                                         };

            model.UserAccessList.Add(defaultAccessLevel);

            model.AccessControlEnabled = CmsConfiguration.AccessControlEnabled;

            return model;
        }
    }
}