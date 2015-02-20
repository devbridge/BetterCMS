using System;
using System.Linq;

using Autofac;

using BetterCMS.Module.LuceneSearch.Models;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ModelTests.MapTests
{
    [TestFixture]
    public class AuthorMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Author_Successfully()
        {
            var content = TestDataProvider.CreateNewAuthor();
            RunEntityMapTestsInTransaction(content);  
        }
        
        [Test]
        public void Should_Retrieve_AllField_If_Image_Is_Null()
        {
            RunActionInTransaction(
                session =>
                    {
                        var testAuthor = TestDataProvider.CreateNewAuthor();
                        testAuthor.Image = TestDataProvider.CreateNewMediaImage();
                        testAuthor.Image.IsDeleted = true;                        
                        session.SaveOrUpdate(testAuthor);
                        session.Flush();
                        session.Clear();

                        var repository = new DefaultRepository(new DefaultUnitOfWork(session));

                        var model = repository.AsQueryable<Author>()
                            .Where(f => !f.IsDeleted)
                            .Where(f => f.Id == testAuthor.Id)
                            .Select(author => new AuthorModel
                                          {
                                              Id = author.Id,
                                              Version = author.Version,
                                              CreatedBy = author.CreatedByUser,
                                              CreatedOn = author.CreatedOn,
                                              LastModifiedBy = author.ModifiedByUser,
                                              LastModifiedOn = author.ModifiedOn,

                                              Name = author.Name,

                                              ImageId = author.Image != null && !author.Image.IsDeleted ? author.Image.Id : (Guid?)null,
                                              ImageUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicUrl : (string)null,
                                              ImageThumbnailUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicThumbnailUrl : (string)null,
                                              ImageCaption = author.Image != null && !author.Image.IsDeleted ? author.Image.Caption : (string)null

                                          }).FirstOne();

                        Assert.IsNotNull(model);
                        Assert.AreEqual(testAuthor.Id, model.Id);
                        Assert.IsNull(model.ImageId);
                    });
        }
    }
}
