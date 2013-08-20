using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Upload;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload
{
    public class GetMultiFileUploadCommand : CommandBase, ICommand<GetMultiFileUploadRequest, MultiFileUploadViewModel>
    {
        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        private readonly ISecurityService securityService;

        public GetMultiFileUploadCommand(ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService, ISecurityService securityService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
        }

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

            if (request.Type != MediaType.Image && cmsConfiguration.AccessControlEnabled && request.ReuploadMediaId.HasDefaultValue())
            {
                var principal = securityService.GetCurrentPrincipal();
                model.UserAccessList = accessControlService.GetDefaultAccessList(principal).Cast<UserAccessViewModel>().ToList();

                model.AccessControlEnabled = cmsConfiguration.AccessControlEnabled;
            }

            return model;
        }
    }
}