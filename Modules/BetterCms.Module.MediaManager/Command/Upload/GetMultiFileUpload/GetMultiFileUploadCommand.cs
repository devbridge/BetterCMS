using System;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Upload;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.DataAccess.DataContext;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload
{
    public class GetMultiFileUploadCommand : CommandBase, ICommand<GetMultiFileUploadRequest, MultiFileUploadViewModel>
    {
        private readonly ICmsConfiguration cmsConfiguration;

        public GetMultiFileUploadCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
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

            if (request.Type != MediaType.Image && cmsConfiguration.Security.AccessControlEnabled)
            {
                if (!request.ReuploadMediaId.HasDefaultValue())
                {
                    var media = Repository.AsQueryable<Media>(m => m.Id == request.ReuploadMediaId).FirstOne();
                    var file = media as MediaFile;
                    if (file != null)
                    {
                        AccessControlService.DemandAccess(file, Context.Principal, AccessLevel.ReadWrite);
                    }
                }
                else
                {
                    var principal = SecurityService.GetCurrentPrincipal();
                    model.UserAccessList = AccessControlService.GetDefaultAccessList(principal).Cast<UserAccessViewModel>().ToList();
                    model.AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled;                    
                }

            }
            return model;
        }
    }
}