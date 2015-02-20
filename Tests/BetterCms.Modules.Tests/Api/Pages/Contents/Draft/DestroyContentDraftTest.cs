using System;
using System.Security.Principal;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft;
using BetterCms.Module.Pages.Exceptions;
using BetterCms.Module.Root;

using BetterModules.Core.Exceptions.DataTier;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Contents.Draft
{
    public class DestroyContentDraftTest : ApiIntegrationTestBase
    {
        private IPrincipal principal;

        [SetUp]
        public void SetUp()
        {
            principal = System.Threading.Thread.CurrentPrincipal;
            SetCurrentPrincipal(RootModuleConstants.UserRoles.AllRoles);
        }

        [TearDown]
        public void TearDown()
        {
            System.Threading.Thread.CurrentPrincipal = principal;
        }

        [Test]
        public void Should_Destroy_Draft_ByDraftId_Successfully()
        {
            RunApiActionInTransaction(
                (api, session) =>
                {
                    BetterCms.Module.Pages.Models.HtmlContent parent;
                    BetterCms.Module.Pages.Models.HtmlContent draft;
                    CreateContents(session, true, out parent, out draft);

                    var request = new DestroyContentDraftRequest { Id = draft.Id };
                    var response = api.Pages.Content.Draft.Delete(request);

                    Assert.IsNotNull(request);
                    Assert.IsTrue(response.Data);
                });
        }
        
        [Test]
        public void Should_Destroy_Draft_ByPublishedId_Successfully()
        {
            RunApiActionInTransaction(
                (api, session) =>
                {
                    BetterCms.Module.Pages.Models.HtmlContent parent;
                    BetterCms.Module.Pages.Models.HtmlContent draft;
                    CreateContents(session, true, out parent, out draft);

                    var request = new DestroyContentDraftRequest { Id = parent.Id };
                    var response = api.Pages.Content.Draft.Delete(request);

                    Assert.IsNotNull(request);
                    Assert.IsTrue(response.Data);
                });
        }
        
        [Test]
        [ExpectedException(typeof(ConcurrentDataException))]
        public void Should_NotDestroy_Draft_ConcurrentException()
        {
            RunApiActionInTransaction(
                (api, session) =>
                {
                    BetterCms.Module.Pages.Models.HtmlContent parent;
                    BetterCms.Module.Pages.Models.HtmlContent draft;
                    CreateContents(session, true, out parent, out draft);

                    var request = new DestroyContentDraftRequest { Id = parent.Id };
                    request.Data.Version = 15;
                    var response = api.Pages.Content.Draft.Delete(request);

                    Assert.IsNotNull(request);
                    Assert.IsTrue(response.Data);
                });
        }
        
        [Test]
        [ExpectedException(typeof(DraftNotFoundException))]
        public void Should_NotDestroy_Draft_DraftNotFoundException()
        {
            RunApiActionInTransaction(
                (api, session) =>
                {
                    BetterCms.Module.Pages.Models.HtmlContent parent;
                    BetterCms.Module.Pages.Models.HtmlContent draft;
                    CreateContents(session, false, out parent, out draft);

                    var request = new DestroyContentDraftRequest { Id = parent.Id };
                    var response = api.Pages.Content.Draft.Delete(request);

                    Assert.IsNotNull(request);
                    Assert.IsTrue(response.Data);
                });
        }
        
        [Test]
        [ExpectedException(typeof(DraftNotFoundException))]
        public void Should_NotDestroy_Draft_SecondTime_DraftNotFoundException()
        {
            RunApiActionInTransaction(
                (api, session) =>
                {
                    BetterCms.Module.Pages.Models.HtmlContent parent;
                    BetterCms.Module.Pages.Models.HtmlContent draft;
                    CreateContents(session, true, out parent, out draft);

                    var request = new DestroyContentDraftRequest { Id = parent.Id };
                    var response = api.Pages.Content.Draft.Delete(request);

                    Assert.IsNotNull(request);
                    Assert.IsTrue(response.Data);

                    request = new DestroyContentDraftRequest { Id = parent.Id };
                    response = api.Pages.Content.Draft.Delete(request);
                    Assert.IsTrue(false, "DraftNotFoundException exception should be thrown");
                });
        }

        private void CreateContents(ISession session, bool saveDraft, out BetterCms.Module.Pages.Models.HtmlContent parent, out BetterCms.Module.Pages.Models.HtmlContent draft)
        {
            parent = TestDataProvider.CreateNewHtmlContent();
            parent.Status = ContentStatus.Published;
            parent.PublishedOn = DateTime.Now;
            parent.PublishedByUser = "TEST";
            session.SaveOrUpdate(parent);

            if (saveDraft)
            {
                draft = TestDataProvider.CreateNewHtmlContent();
                draft.Status = ContentStatus.Draft;
                draft.PublishedOn = null;
                draft.PublishedByUser = null;
                draft.Original = parent;
                session.SaveOrUpdate(draft);
            }
            else
            {
                draft = null;
            }

            session.Flush();
            session.Clear();
        }
    }
}
