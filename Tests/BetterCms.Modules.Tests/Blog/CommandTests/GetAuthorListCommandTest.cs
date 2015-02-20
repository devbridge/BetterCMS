//using System.Linq;
//
//using BetterModules.Core.DataAccess;
//using BetterCms.Module.Blog.Commands.GetAuthorList;
//using BetterCms.Module.Root.Mvc.Grids.GridOptions;
//
//using Moq;
//
//using NUnit.Framework;
//
//namespace BetterCms.Test.Module.Blog.CommandTests
//{
//    [TestFixture]
//    public class GetAuthorListCommandTest : TestBase
//    {
//        [Test]
//        public void Should_Return_Author_List_Successfully()
//        {
//            var author1 = TestDataProvider.CreateNewAuthor();
//            var author2 = TestDataProvider.CreateNewAuthor();
//
//            Mock<IRepository> repositoryMock = new Mock<IRepository>();
//            repositoryMock
//                .Setup(f => f.AsQueryable<BetterCms.Module.Blog.Models.Author>())
//                .Returns(new[] { author1, author2 }.AsQueryable());
//
//            var command = new GetAuthorListCommand { Repository = repositoryMock.Object };
//
//            var response = command.Execute(new SearchableGridOptions());
//
//            Assert.IsNotNull(response);
//            Assert.IsNotNull(response.Items);
//            Assert.AreEqual(response.Items.Count(), 2);
//
//            var author = response.Items.FirstOrDefault(l => author1.Id == l.Id);
//            Assert.IsNotNull(author);
//
//            Assert.AreEqual(author1.Name, author.Name);
//        }
//
//        [Test]
//        public void Should_Return_Empty_List()
//        {
//            Mock<IRepository> repositoryMock = new Mock<IRepository>();
//            repositoryMock
//                .Setup(f => f.AsQueryable<BetterCms.Module.Blog.Models.Author>())
//                .Returns(new BetterCms.Module.Blog.Models.Author[] { }.AsQueryable());
//
//            var command = new GetAuthorListCommand { Repository = repositoryMock.Object };
//
//            var list = command.Execute(new SearchableGridOptions());
//
//            Assert.IsNotNull(list);
//            Assert.IsNotNull(list.Items);
//            Assert.IsEmpty(list.Items);
//        }
//    }
//}
