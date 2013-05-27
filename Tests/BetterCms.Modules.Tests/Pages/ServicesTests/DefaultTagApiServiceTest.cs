using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Root.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultTagApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_Tags_List_Successfully()
        {
            var fakeTags = CreateTags().ToList();
            var repositoryMock = MockRepository(fakeTags);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var tags = service.GetTags();

                Assert.IsNotNull(tags);
                Assert.AreEqual(tags.Count, fakeTags.Count);

                var fakeTag = fakeTags[0];
                var tag = tags.FirstOrDefault(l => fakeTag.Id == l.Id);
                Assert.IsNotNull(tag);
                Assert.AreEqual(fakeTag.Name, tag.Name);
            }
        }

        [Test]
        public void Should_Return_Filtered_Ordered_Tags_List_Successfully()
        {
            var fakeTags = CreateTags().ToList();
            var repositoryMock = MockRepository(fakeTags);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var tags = service.GetTags(new GetDataRequest<Tag>(t => t.Name.Contains("Tag"), null, true));

                Assert.IsNotNull(tags);
                Assert.AreEqual(tags.Count, 3);

                var fakeTag = fakeTags.First(t => t.Name == "Tag3");
                var tag = tags[0];
                Assert.IsNotNull(tag);
                Assert.AreEqual(fakeTag.Id, tag.Id);
            }
        }

        [Test]
        public void Should_Return_Filtered_Ordered_Paged_Tags_List_Successfully()
        {
            var fakeTags = CreateTags().ToList();
            var repositoryMock = MockRepository(fakeTags);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var tags = service.GetTags(new GetDataRequest<Tag>(2, 1, t => t.Name.Contains("Tag"), null, true));

                Assert.IsNotNull(tags);
                Assert.AreEqual(tags.Count, 1);

                var fakeTag = fakeTags.First(t => t.Name == "Tag2");
                var tag = tags[0];
                Assert.IsNotNull(tag);
                Assert.AreEqual(fakeTag.Id, tag.Id);
            }
        }
        
        [Test]
        public void Should_Return_Empty_Tags_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Tag>())
                .Returns(new BetterCms.Module.Root.Models.Tag[] { }.AsQueryable());

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var tags = service.GetTags();

                Assert.IsNotNull(tags);
                Assert.IsEmpty(tags);
            }
        }

        private Mock<IRepository> MockRepository(IEnumerable<BetterCms.Module.Root.Models.Tag> tags)
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Tag>())
                .Returns(tags.AsQueryable());

            return repositoryMock;
        }

        private IEnumerable<BetterCms.Module.Root.Models.Tag> CreateTags()
        {
            BetterCms.Module.Root.Models.Tag tag1 = TestDataProvider.CreateNewTag();
            BetterCms.Module.Root.Models.Tag tag2 = TestDataProvider.CreateNewTag();
            BetterCms.Module.Root.Models.Tag tag3 = TestDataProvider.CreateNewTag();
            BetterCms.Module.Root.Models.Tag tag4 = TestDataProvider.CreateNewTag();
            BetterCms.Module.Root.Models.Tag tag5 = TestDataProvider.CreateNewTag();

            tag1.Name = "Tag1";
            tag2.Name = "Tag2";
            tag3.Name = "Tag3";
            tag4.Name = "aaa1";
            tag5.Name = "aaa2";

            return new[] { tag1, tag2, tag3, tag4, tag5 };
        }
    }
}
