using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Root.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ApiTests
{
    [TestFixture]
    public class TagApiTests : ApiTestBase
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
                Assert.AreEqual(tags.Items.Count, fakeTags.Count);
                Assert.AreEqual(tags.TotalCount, fakeTags.Count);

                var fakeTag = fakeTags[0];
                var tag = tags.Items.FirstOrDefault(l => fakeTag.Id == l.Id);
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
                var tags = service.GetTags(new GetTagsRequest(t => t.Name.Contains("Tag"), t => t.Name, true));

                Assert.IsNotNull(tags);
                Assert.AreEqual(tags.Items.Count, 3);
                Assert.AreEqual(tags.TotalCount, 3);

                var fakeTag = fakeTags.First(t => t.Name == "Tag3");
                var tag = tags.Items[0];
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
                var request = new GetTagsRequest(t => t.Name.Contains("Tag"), null, true);
                request.AddPaging(1, 2);
                var tags = service.GetTags(request);

                Assert.IsNotNull(tags);
                Assert.AreEqual(tags.Items.Count, 1);
                Assert.AreEqual(tags.TotalCount, 3);

                var fakeTag = fakeTags.First(t => t.Name == "Tag2");
                var tag = tags.Items[0];
                Assert.IsNotNull(tag);
                Assert.AreEqual(fakeTag.Id, tag.Id);
            }
        }
        
        [Test]
        public void Should_Return_Empty_Tags_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Tag>())
                .Returns(new Tag[] { }.AsQueryable());

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var tags = service.GetTags();

                Assert.IsNotNull(tags);
                Assert.IsEmpty(tags.Items);
                Assert.AreEqual(tags.TotalCount, 0);
            }
        }

        private IEnumerable<Tag> CreateTags()
        {
            Tag tag1 = TestDataProvider.CreateNewTag();
            Tag tag2 = TestDataProvider.CreateNewTag();
            Tag tag3 = TestDataProvider.CreateNewTag();
            Tag tag4 = TestDataProvider.CreateNewTag();
            Tag tag5 = TestDataProvider.CreateNewTag();

            tag1.Name = "Tag1";
            tag2.Name = "Tag2";
            tag3.Name = "Tag3";
            tag4.Name = "aaa1";
            tag5.Name = "aaa2";

            return new[] { tag1, tag2, tag3, tag4, tag5 };
        }
    }
}
