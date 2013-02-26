using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Blog.DataServices;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ServiceTests
{
    [TestFixture]
    public class DefaultAuthorApiServiceTest : TestBase
    {
        // TODO
        [Test]
        public void Should_Return_Authors_List_Successfully()
        {
//            BetterCms.Module.Blog.Models.Author author1 = TestDataProvider.CreateNewAuthor();
//            BetterCms.Module.Blog.Models.Author author2 = TestDataProvider.CreateNewAuthor();
//
//            Mock<IRepository> repositoryMock = new Mock<IRepository>();
//            repositoryMock.Setup(f => f.AsQueryable<BetterCms.Module.Blog.Models.Author>()).Returns(new[] { author1, author2 }.AsQueryable());
//
//            var service = new DefaultAuthorApiService(repositoryMock.Object);
//            var authors = service.GetAuthors();
//
//            Assert.IsNotNull(authors);
//            Assert.AreEqual(authors.Count, 2);
//
//            var author = authors.FirstOrDefault(l => author1.Id == l.Id);
//            Assert.IsNotNull(author);
//            Assert.AreEqual(author1.Name, author.Name);
//            Assert.IsNotNull(author.Image);
//            Assert.AreEqual(author1.Image.Caption, author.Image.Caption);
        }

        [Test]
        public void Should_Return_Empty_Authors_List_Successfully()
        {
//            Mock<IRepository> repositoryMock = new Mock<IRepository>();
//            repositoryMock.Setup(f => f.AsQueryable<BetterCms.Module.Blog.Models.Author>()).Returns(new BetterCms.Module.Blog.Models.Author[] { }.AsQueryable());
//
//            var service = new DefaultAuthorApiService(repositoryMock.Object);
//            var authors = service.GetAuthors();
//
//            Assert.IsNotNull(authors);
//            Assert.IsEmpty(authors);
        }
    }
}
