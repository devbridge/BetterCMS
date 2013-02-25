using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.DataServices;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultCategoryApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_Categorys_List_Successfully()
        {
            BetterCms.Module.Root.Models.Category category1 = TestDataProvider.CreateNewCategory();
            BetterCms.Module.Root.Models.Category category2 = TestDataProvider.CreateNewCategory();

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Category>())
                .Returns(new[] { category1, category2 }.AsQueryable());

            var service = new DefaultCategoryApiService(repositoryMock.Object);
            var categories = service.GetCategories();

            Assert.IsNotNull(categories);
            Assert.AreEqual(categories.Count, 2);

            var category = categories.FirstOrDefault(l => category1.Id == l.Id);
            Assert.IsNotNull(category);
            Assert.AreEqual(category1.Name, category.Name);
        }
        
        [Test]
        public void Should_Return_Empty_Categorys_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Category>())
                .Returns(new BetterCms.Module.Root.Models.Category[] { }.AsQueryable());

            var service = new DefaultCategoryApiService(repositoryMock.Object);
            var categories = service.GetCategories();

            Assert.IsNotNull(categories);
            Assert.IsEmpty(categories);
        }
    }
}
