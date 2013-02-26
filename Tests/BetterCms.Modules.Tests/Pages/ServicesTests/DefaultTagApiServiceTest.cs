using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.DataServices;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultTagApiServiceTest : TestBase
    {
        // TODO
        [Test]
        public void Should_Return_Tags_List_Successfully()
        {
//            BetterCms.Module.Root.Models.Tag tag1 = TestDataProvider.CreateNewTag();
//            BetterCms.Module.Root.Models.Tag tag2 = TestDataProvider.CreateNewTag();
//
//            Mock<IRepository> repositoryMock = new Mock<IRepository>();
//            repositoryMock
//                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Tag>())
//                .Returns(new[] { tag1, tag2 }.AsQueryable());
//
//            var service = new DefaultTagApiService(repositoryMock.Object);
//            var tags = service.GetTags();
//
//            Assert.IsNotNull(tags);
//            Assert.AreEqual(tags.Count, 2);
//
//            var tag = tags.FirstOrDefault(l => tag1.Id == l.Id);
//            Assert.IsNotNull(tag);
//            Assert.AreEqual(tag1.Name, tag.Name);
        }
        
        [Test]
        public void Should_Return_Empty_Tags_List_Successfully()
        {
//            Mock<IRepository> repositoryMock = new Mock<IRepository>();
//            repositoryMock
//                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Tag>())
//                .Returns(new BetterCms.Module.Root.Models.Tag[] { }.AsQueryable());
//
//            var service = new DefaultTagApiService(repositoryMock.Object);
//            var tags = service.GetTags();
//
//            Assert.IsNotNull(tags);
//            Assert.IsEmpty(tags);
        }
    }
}
