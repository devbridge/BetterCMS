using Autofac;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Command.Content.DeletePageContent;
using BetterCms.Module.Root.Services;

using Moq;

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
                var securityService = new Mock<ISecurityService>().Object;
                var optionService = new Mock<IOptionService>().Object;
                var childContentService = new Mock<IChildContentService>().Object;

                var contentService = new DefaultContentService(securityService, repository, optionService, childContentService);
                var command = new DeletePageContentCommand(contentService);
                command.UnitOfWork = unitOfWork;
                command.Repository = repository;

                var result = command.Execute(request);
                Assert.IsTrue(result);
            });
        }
    }
}
