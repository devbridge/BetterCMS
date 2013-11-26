using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.Command.Content.DeletePageContent;
using BetterCms.Module.Root.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class DeletePageContentCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Delete_Page_Content()
        {
            RunActionInTransaction(session =>
            {
                // Create content
                var content = TestDataProvider.CreateNewHtmlContent();
                var pageContent = TestDataProvider.CreateNewPageContent(content);
                
                session.SaveOrUpdate(pageContent);
                session.Flush();
                session.Clear();

                // Delete page content
                var request = new DeletePageContentCommandRequest
                                  {
                                      PageContentId = pageContent.Id,
                                      PageContentVersion = pageContent.Version,
                                      ContentVersion = content.Version
                                  };
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var contentService = Container.Resolve<IContentService>();
                var command = new DeletePageContentCommand(contentService);
                command.UnitOfWork = unitOfWork;
                command.Repository = repository;

                var result = command.Execute(request);
                Assert.IsTrue(result);
            });
        }
    }
}
