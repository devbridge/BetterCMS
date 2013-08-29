using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Command.Content.DeletePageContent;

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
                var command = new DeletePageContentCommand();
                command.UnitOfWork = unitOfWork;
                command.Repository = new DefaultRepository(unitOfWork);

                var result = command.Execute(request);
                Assert.IsTrue(result);
            });
        }
    }
}
