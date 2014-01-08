using System;
using System.Collections.Generic;
using System.Linq;

using BetterCMS.Module.LuceneSearch.Models;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Blog.Models;

using NHibernate.Linq;

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
        
        [Test]
        public void AAAA()
        {
            RunActionInTransaction(
                session =>
                    {
                        var repository = new DefaultRepository(new DefaultUnitOfWork(session));

                        var links1 = GetExpiredLinks(100, repository);
                        var links2 = GetFailedLinks(100, repository);
                        var links3 = GetUnprocessedLinks(100, repository);

                        var aaaa = "5";
                    });
        }

        private const int CrawlFrequency = 1;
        private const int EndTimeout = 5;

        private IList<IndexSource> GetUnprocessedLinks(int limit, IRepository repository)
        {
            IndexSource indexSourceAlias = null;

            var unprocessedUrls =
                repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.StartTime == null)
                          .OrderByAlias(() => indexSourceAlias.Id)
                          .Asc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            return unprocessedUrls;
        }

        private IList<IndexSource> GetExpiredLinks(int limit, IRepository repository)
        {
            IndexSource indexSourceAlias = null;
            var endDate = DateTime.Now.AddMinutes(CrawlFrequency * -1);

            var expiredUrls =
                repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.EndTime != null)
                          .Where(() => indexSourceAlias.EndTime <= endDate)
                          .OrderByAlias(() => indexSourceAlias.EndTime)
                          .Desc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            //expiredUrls = expiredUrls.Where(url => (DateTime.Now - url.EndTime).Value.TotalMinutes > CrawlFrequency).ToList();

            return expiredUrls;
        }

        private IList<IndexSource> GetFailedLinks(int limit, IRepository repository)
        {
            IndexSource indexSourceAlias = null;
            var startDate = DateTime.Now.AddMinutes(EndTimeout * -1);

            var urls =
                repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.StartTime != null && indexSourceAlias.EndTime == null)
                          .Where(() => indexSourceAlias.StartTime <= startDate)
                          .OrderByAlias(() => indexSourceAlias.EndTime)
                          .Desc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            //urls = urls.Where(url => (DateTime.Now - url.StartTime).Value.TotalMinutes > EndTimeout).ToList();

            return urls;
        }
    }
}
