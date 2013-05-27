using System;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultHistoryApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_History_List_Successfully()
        {
            var serviceMock = MockHistoryService(CreateFakeContents());

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), null, null, serviceMock.Object))
            {
                var history = service.GetContentHistory(Guid.NewGuid());

                Assert.IsNotNull(history);
                Assert.AreEqual(history.Count, 4);
            }
        }

        [Test]
        public void Should_Return_Empty_List_Successfully()
        {
            var serviceMock = MockHistoryService();
            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), null, null, serviceMock.Object))
            {
                var history = service.GetContentHistory(Guid.NewGuid());

                Assert.IsNotNull(history);
                Assert.AreEqual(history.Count, 0);
            }
        }

        [Test]
        public void Should_Return_Sorted_List_Successfully()
        {
            var contents = CreateFakeContents();
            var serviceMock = MockHistoryService(contents);
            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), null, null, serviceMock.Object))
            {
                var history = service.GetContentHistory(Guid.NewGuid(), new GetFilteredDataRequest<Content>(order: c => c.Name, orderDescending: true));

                Assert.IsNotNull(history);
                Assert.AreEqual(history.Count, 4);
                Assert.AreEqual(history[1].Id, contents.First(c => c.Name == "Test_1").Id);
            }
        }
        
        [Test]
        public void Should_Return_Filtered_List_Successfully()
        {
            var filteredName = "Test_1"; 
            var contents = CreateFakeContents();
            var serviceMock = MockHistoryService(contents);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), null, null, serviceMock.Object))
            {
                var history = service.GetContentHistory(Guid.NewGuid(), new GetFilteredDataRequest<Content>(c => c.Name == filteredName));

                Assert.IsNotNull(history);
                Assert.AreEqual(history.Count, 1);
                Assert.AreEqual(history[0].Id, contents.First(c => c.Name == filteredName).Id);
            }
        }

        private Content[] CreateFakeContents()
        {
            var content1 = TestDataProvider.CreateNewContent();
            var content2 = TestDataProvider.CreateNewHtmlContent();
            var content3 = TestDataProvider.CreateNewHtmlContentWidget();
            var content4 = TestDataProvider.CreateNewServerControlWidget();

            content1.Name = "Content_1";
            content2.Name = "Content_2";
            content3.Name = "Test_1";
            content4.Name = "Test_2";

            return new[] { content1, content2, content3, content4 };
        }

        private Mock<IHistoryService> MockHistoryService(Content[] contents = null)
        {
            var serviceMock = new Mock<IHistoryService>();

            serviceMock
                .Setup(h => h.GetContentHistory(It.IsAny<Guid>(), It.IsAny<SearchableGridOptions>()))
                .Returns(contents ?? new Content[0]);

            return serviceMock;
        }
    }
}
