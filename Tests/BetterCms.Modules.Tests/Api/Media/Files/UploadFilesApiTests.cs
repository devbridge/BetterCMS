// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadFilesApiTests.cs" company="Devbridge Group LLC">
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
using System.IO;
using System.Reflection;

using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.MediaManager.Models;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Media.Files
{
    public class UploadFileApiTests : ApiCrdIntegrationTestBase<
        UploadFileModel, FileModel,
        UploadFileRequest, UploadFileResponse,
        GetFileRequest, GetFileResponse,
        DeleteFileRequest, DeleteFileResponse>
    {
        protected const string TestBigImageFileName = "logo.big.png";
        protected const string TestBigImagePath = "BetterCms.Test.Module.Contents.Images.logo.big.png";

        private MediaFolder folder;

        [Test]
        public void Should_Upload_File_From_Stream_Successfully()
        {
            Events.MediaManagerEvents.Instance.MediaFileUploaded += Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
            {
                folder = TestDataProvider.CreateNewMediaFolder(false, MediaType.File);
                session.SaveOrUpdate(folder);
                session.Flush();

                Run(session, api.Media.Files.Upload.Post, api.Media.File.Get, api.Media.File.Delete);
            });

            Events.MediaManagerEvents.Instance.MediaFileUploaded -= Instance_EntityCreated;
            Events.MediaManagerEvents.Instance.MediaFileDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        [ExpectedException(typeof(CmsApiValidationException))]
        public void Should_Throw_Exception_For_Invalid_Folder_Type()
        {
            RunApiActionInTransaction((api, session) =>
            {
                folder = TestDataProvider.CreateNewMediaFolder(false);
                session.SaveOrUpdate(folder);
                session.Flush();

                Run(session, api.Media.Files.Upload.Post, api.Media.File.Get, api.Media.File.Delete);
            });
        }

        protected override UploadFileModel GetCreateModel(ISession session)
        {
            return new UploadFileModel
            {
                FileName = TestBigImageFileName,
                FileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(TestBigImagePath),
                FolderId = folder.Id,
                Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                AccessRules =
                    new[]
                    {
                        new AccessRuleModel { AccessLevel = AccessLevel.ReadWrite, Identity = TestDataProvider.ProvideRandomString(20), IsForRole = false },
                        new AccessRuleModel { AccessLevel = AccessLevel.Deny, Identity = TestDataProvider.ProvideRandomString(20), IsForRole = true }
                    },
                WaitForUploadResult = true
            };
        }

        protected override GetFileRequest GetGetRequest(SaveResponseBase saveResponseBase)
        {
            return new GetFileRequest { FileId = saveResponseBase.Data.Value, Data = new GetFileModel() { IncludeAccessRules = true, IncludeTags = true } };
        }

        protected override void OnAfterGet(GetFileResponse getResponse, UploadFileModel model)
        {
            Assert.IsNotNull(getResponse.Data.Id);
            Assert.AreEqual(getResponse.Data.FileSize, model.FileStream.Length);
            Assert.AreEqual(getResponse.Data.FolderId, model.FolderId);
            Assert.AreEqual(getResponse.Data.FolderName, folder.Title);
            Assert.AreEqual(getResponse.Data.OriginalFileName, TestBigImageFileName);
            Assert.AreEqual(getResponse.Data.OriginalFileExtension, Path.GetExtension(TestBigImageFileName));
            Assert.AreEqual(getResponse.Data.FileExtension, Path.GetExtension(TestBigImageFileName));
            Assert.AreEqual(getResponse.Data.IsArchived, false);
            Assert.AreEqual(getResponse.Data.IsCanceled, false);
            Assert.AreEqual(getResponse.Data.IsTemporary, false);
            Assert.AreEqual(getResponse.Data.IsUploaded, true);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.AreEqual(getResponse.AccessRules.Count, model.AccessRules.Count);
            
            const string urlStart = "http://bettercms.sandbox.mvc4.local.net/uploads/file/";

            Assert.IsTrue(getResponse.Data.FileUrl.StartsWith(urlStart)
                && getResponse.Data.FileUrl.EndsWith(string.Format("/{0}", TestBigImageFileName), System.StringComparison.Ordinal));
            Assert.IsTrue(getResponse.Data.FileUri.EndsWith(string.Format("/{0}", TestBigImageFileName), System.StringComparison.Ordinal));

            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
            
            Assert.AreEqual(getResponse.Data.ThumbnailId, null);
            Assert.AreEqual(getResponse.Data.ThumbnailCaption, null);
            Assert.AreEqual(getResponse.Data.ThumbnailUrl, null);
        }
    }
}
