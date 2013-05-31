using System.Linq;

using BetterCms.Api;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ApiTests
{
    [TestFixture]
    public class AuthorApiTests : ApiTestBase
    {
        [Test]
        public void Should_Return_Authors_List_Successfully()
        {
            Author author1 = TestDataProvider.CreateNewAuthor();
            Author author2 = TestDataProvider.CreateNewAuthor();

            var repositoryMock = MockRepository(new[] { author1, author2 });
            var tagService = new Mock<ITagService>();

            using (var service = new BlogsApiContext(Container.BeginLifetimeScope(), tagService.Object, repositoryMock.Object))
            {
                var authors = service.GetAuthors(new GetAuthorsRequest());

                Assert.IsNotNull(authors);
                Assert.AreEqual(authors.Items.Count, 2);

                var author = authors.Items.FirstOrDefault(l => author1.Id == l.Id);
                Assert.IsNotNull(author);
                Assert.AreEqual(author.Name, author.Name);
            }
        }

        [Test]
        public void Should_Return_Empty_Authors_List_Successfully()
        {
            var repositoryMock = MockRepository(new Author[] { });
            var tagService = new Mock<ITagService>();

            using (var service = new BlogsApiContext(Container.BeginLifetimeScope(), tagService.Object, repositoryMock.Object))
            {
                var authors = service.GetAuthors(new GetAuthorsRequest());

                Assert.IsNotNull(authors);
                Assert.IsNotNull(authors.Items);
                Assert.IsEmpty(authors.Items);
            }
        }
    }
}
