using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Commands.GetTemplates;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.LayoutTests
{
    [TestFixture]
    public class GetTemplatesCommandTest : TestBase
    {
        [Test]
        public void Should_Return_Templates_List_Successfully()
        {
            BetterCms.Module.Root.Models.Layout layout1 = this.TestDataProvider.CreateNewLayout();
            BetterCms.Module.Root.Models.Layout layout2 = this.TestDataProvider.CreateNewLayout();

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Layout>())
                .Returns(new[] { layout1, layout2 }.AsQueryable());

            var command = new GetTemplatesCommand();
            command.Repository = repositoryMock.Object;

            var response = command.Execute(new GetTemplatesRequest());

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Templates);
            Assert.AreEqual(response.Templates.Count, 2);

            var layout = response.Templates.FirstOrDefault(l => layout1.Id == l.TemplateId);
            Assert.IsNotNull(layout);

            Assert.AreEqual(layout1.Name, layout.Title);
        }

        [Test]
        public void Should_Return_Empty_List()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Layout>())
                .Returns(new BetterCms.Module.Root.Models.Layout[] { }.AsQueryable());

            var command = new GetTemplatesCommand();
            command.Repository = repositoryMock.Object;

            var list = command.Execute(new GetTemplatesRequest());

            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Templates);
            Assert.IsEmpty(list.Templates);
        }
    }
}
