using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
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

            using (var api = CreateBlogsApiContext(new[] { blogPost1, blogPost2 }))
            {
                var blogPosts = api.GetBlogPosts(new GetBlogPostsRequest(includeUnpublished: true, includeNotActive: true));

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
            using (var api = CreateBlogsApiContext(new BlogPost[0]))
            {
                var blogPosts = api.GetBlogPosts(new GetBlogPostsRequest());

                Assert.IsNotNull(blogPosts);
                Assert.IsNotNull(blogPosts.Items);
                Assert.IsEmpty(blogPosts.Items);
            }
        }

        private BlogsApiContext CreateBlogsApiContext(BlogPost[] blogs)
        {
            var repositoryMock = MockRepository(blogs);
            var tagService = new Mock<ITagService>();
            var authorServiceMock = new Mock<IAuthorService>();
            var blogServiceMock = new Mock<IBlogService>();

            blogServiceMock
                .Setup(f => f.GetBlogPostsAsQueryable(It.IsAny<GetBlogPostsRequest>()))
                .Returns(blogs.Select(b => new BlogPostModel
                                               {
                                                   Id = b.Id,
                                                   Version = b.Version,
                                                   Title = b.Title
                                               }).AsQueryable());

            return new BlogsApiContext(Container.BeginLifetimeScope(), tagService.Object, blogServiceMock.Object, authorServiceMock.Object, repositoryMock.Object);
        }
    }
}
