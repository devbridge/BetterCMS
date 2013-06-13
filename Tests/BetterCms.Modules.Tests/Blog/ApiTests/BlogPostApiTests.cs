using System.Linq;

using BetterCms.Api;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ApiTests
{
    [TestFixture]
    public class BlogPostApiTests : ApiTestBase
    {
        [Test]
        public void Should_Return_BlogPosts_List_Successfully()
        {
            BlogPost blogPost1 = TestDataProvider.CreateNewBlogPost();
            BlogPost blogPost2 = TestDataProvider.CreateNewBlogPost();

            var repositoryMock = MockRepository(new[] { blogPost1, blogPost2 });
            var tagServiceMock = new Mock<ITagService>();
            var authorServiceMock = new Mock<IAuthorService>();

            using (var service = new BlogsApiContext(Container.BeginLifetimeScope(), tagServiceMock.Object, authorServiceMock.Object, repositoryMock.Object))
            {
                var blogPosts = service.GetBlogPosts(new GetBlogPostsRequest(includeUnpublished:true, includeNotActive:true));

                Assert.IsNotNull(blogPosts);
                Assert.AreEqual(blogPosts.Items.Count, 2);

                var blogPost = blogPosts.Items.FirstOrDefault(l => blogPost1.Id == l.Id);
                Assert.IsNotNull(blogPost);
                Assert.AreEqual(blogPost1.Title, blogPost.Title);
            }
        }

        [Test]
        public void Should_Return_Empty_BlogPosts_List_Successfully()
        {
            var repositoryMock = MockRepository(new BlogPost[] { });
            var tagService = new Mock<ITagService>();
            var authorServiceMock = new Mock<IAuthorService>();

            using (var service = new BlogsApiContext(Container.BeginLifetimeScope(), tagService.Object, authorServiceMock.Object, repositoryMock.Object))
            {
                var blogPosts = service.GetBlogPosts(new GetBlogPostsRequest());

                Assert.IsNotNull(blogPosts);
                Assert.IsNotNull(blogPosts.Items);
                Assert.IsEmpty(blogPosts.Items);
            }
        }
    }
}
