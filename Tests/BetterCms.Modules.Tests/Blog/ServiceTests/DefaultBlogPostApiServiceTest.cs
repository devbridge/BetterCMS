using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Blog.DataServices;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ServiceTests
{
    [TestFixture]
    public class DefaultBlogPostApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_BlogPosts_List_Successfully()
        {
            BetterCms.Module.Blog.Models.BlogPost blogPost1 = TestDataProvider.CreateNewBlogPost();
            BetterCms.Module.Blog.Models.BlogPost blogPost2 = TestDataProvider.CreateNewBlogPost();

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(f => f.AsQueryable<BetterCms.Module.Blog.Models.BlogPost>()).Returns(new[] { blogPost1, blogPost2 }.AsQueryable());

            var service = new DefaultBlogApiService(repositoryMock.Object);
            var blogPosts = service.GetBlogPosts();

            Assert.IsNotNull(blogPosts);
            Assert.AreEqual(blogPosts.Count, 2);

            var blogPost = blogPosts.FirstOrDefault(l => blogPost1.Id == l.Id);
            Assert.IsNotNull(blogPost);
            Assert.AreEqual(blogPost1.Title, blogPost.Title);
        }

        [Test]
        public void Should_Return_Empty_BlogPosts_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(f => f.AsQueryable<BetterCms.Module.Blog.Models.BlogPost>()).Returns(new BetterCms.Module.Blog.Models.BlogPost[] { }.AsQueryable());

            var service = new DefaultBlogApiService(repositoryMock.Object);
            var blogPosts = service.GetBlogPosts();

            Assert.IsNotNull(blogPosts);
            Assert.IsEmpty(blogPosts);
        }
    }
}
