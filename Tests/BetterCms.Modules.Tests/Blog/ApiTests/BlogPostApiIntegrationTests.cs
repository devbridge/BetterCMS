using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ApiTests
{
    [TestFixture]
    public class BlogPostApiIntegrationTests : DatabaseTestBase
    {
        private static Category category;

        private const string BlogPostTitle1 = "Algeria";
        private const string BlogPostTitle2 = "Bolivia";
        private const string BlogPostTitle3 = "Albania";
        private const string BlogPostTitle4 = "Laos";
        private const string BlogPostTitle5 = "Qatar";
        private const string BlogPostTitle6 = "Mali";
        private const string BlogPostTitle7 = "Latvia";
        private const string BlogPostTitle8 = "Liberia";
        private const string BlogPostTitle9 = "Luxembourg";
        private const string BlogPostTitle10 = "Lithuania";

        [Test]
        public void GetBlogPosts()
        {
            RunActionInTransaction(session =>
            {
                using (var api = CreateApi(session))
                {
                    var blogs = CreateFakeBlogs(session);

                    // get all items by category id
                    var request = new GetBlogPostsRequest(b => b.CategoryId == category.Id, includeNotActive: true, includeUnpublished: true);
                    var response = api.GetBlogPosts(request);

                    Assert.IsNotNull(response);
                    Assert.AreEqual(response.Items.Count, blogs.Count);    
                }
            });
        }

        [Test]
        public void GetBlogPostsFilteredByCategoryName()
        {
            RunActionInTransaction(
                session =>
                    {
                        using (var api = CreateApi(session))
                        {
                            var blogs = CreateFakeBlogs(session);

                            // get all items by category name
                            var request = new GetBlogPostsRequest(b => b.CategoryName == category.Name, includeNotActive: true, includeUnpublished: true);
                            var response = api.GetBlogPosts(request);

                            Assert.IsNotNull(response);
                            Assert.AreEqual(response.Items.Count, blogs.Count);
                        }
                    });
        }

        [Test]
        public void GetBlogPostsFilteredByName()
        {
            RunActionInTransaction(
               session =>
               {
                   using (var api = CreateApi(session))
                   {
                       var blogs = CreateFakeBlogs(session);

                       // get filtered by name
                       var request = new GetBlogPostsRequest(b => b.CategoryId == category.Id && b.Title.StartsWith("L"), includeNotActive: true, includeUnpublished: true);
                       var response = api.GetBlogPosts(request);

                       Assert.IsNotNull(response);
                       Assert.AreEqual(response.Items.Count, blogs.Count(b => b.Title.StartsWith("L")));
                   }
               });
        }
        
        [Test]
        public void GetBlogPostsOrderedAndPaged()
        {
            RunActionInTransaction(
               session =>
               {
                   using (var api = CreateApi(session))
                   {
                       var blogs = CreateFakeBlogs(session);

                       // get ordered and pages
                       var request = new GetBlogPostsRequest(b => b.CategoryId == category.Id, b => b.Title, true, includeNotActive: true, includeUnpublished: true);
                       var response = api.GetBlogPosts(request);

                       Assert.IsNotNull(response);
                       Assert.AreEqual(response.Items.Count, blogs.Count);
                       Assert.AreEqual(response.Items[0].Title, blogs.OrderByDescending(b => b.Title).First().Title);
                   }
               });
        }
        
        [Test]
        public void GetBlogPostsOrderedPagedAndSkipped()
        {
            RunActionInTransaction(
               session =>
               {
                   using (var api = CreateApi(session))
                   {
                       var blogs = CreateFakeBlogs(session);

                       // get ordered and skipped paged
                       var request = new GetBlogPostsRequest(b => b.CategoryId == category.Id, b => b.Title, startItemNumber: 3, itemsCount: 5, includeNotActive: true, includeUnpublished: true);
                       var response = api.GetBlogPosts(request);

                       Assert.IsNotNull(response);
                       Assert.AreEqual(response.Items.Count, 5);
                       Assert.AreEqual(response.TotalCount, blogs.Count);
                       Assert.AreEqual(response.Items[0].Title, blogs.OrderBy(b => b.Title).Skip(request.StartItemNumber - 1).Take(1).First().Title);
                   }
               });
        }
        
        [Test]
        public void GetBlogPostsOrderedByMultipleField()
        {
            RunActionInTransaction(
               session =>
               {
                   using (var api = CreateApi(session))
                   {
                       var blogs = CreateFakeBlogs(session);

                       // get ordered by 2 fields and adding paging
                       var request = new GetBlogPostsRequest(b => b.CategoryId == category.Id, includeNotActive: true, includeUnpublished: true);
                       request.SetDefaultOrder(b => b.IntroText);
                       request.AddOrder(b => b.Title, true);
                       request.AddPaging(3, 2);
                       var response = api.GetBlogPosts(request);

                       Assert.IsNotNull(response);
                       Assert.AreEqual(response.Items.Count, 3);
                       Assert.AreEqual(response.TotalCount, blogs.Count);
                       Assert.AreEqual(response.Items[0].Title, blogs.OrderBy(b => b.Description).ThenByDescending(b => b.Title).Skip(request.StartItemNumber - 1).Take(1).First().Title);
                   }
               });
        }

        private List<BlogPost> CreateFakeBlogs(ISession session)
        {
            category = TestDataProvider.CreateNewCategory();
            session.SaveOrUpdate(category);

            var blogs = new List<BlogPost>();
            foreach (var country in new[]
                                        {
                                            BlogPostTitle1, 
                                            BlogPostTitle2, 
                                            BlogPostTitle3, 
                                            BlogPostTitle4, 
                                            BlogPostTitle5, 
                                            BlogPostTitle6, 
                                            BlogPostTitle7,
                                            BlogPostTitle8,
                                            BlogPostTitle9,
                                            BlogPostTitle10
                                        })
            {
                var blog = TestDataProvider.CreateNewBlogPost();
                blog.Category = category;
                blog.Title = country;

                blogs.Add(blog);
                session.SaveOrUpdate(blog);
            }

            session.Flush();
            session.Clear();

            return blogs;
        }

        private Tuple<IUnitOfWork, IRepository> CreateRepository(ISession session)
        {
            var unitOfWork = new DefaultUnitOfWork(session);
            var repository = new DefaultRepository(unitOfWork);
            return new Tuple<IUnitOfWork, IRepository>(unitOfWork, repository);
        }

        private BlogsApiContext CreateApi(ISession session)
        {
            var repository = CreateRepository(session);
            var blogService = new DefaultBlogService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IUrlService>(), repository.Item2);
            var authorService = new DefaultAuthorService(repository.Item1, repository.Item2);

            return new BlogsApiContext(Container.BeginLifetimeScope(), null, blogService, authorService, repository.Item2);
        }
    }
}
