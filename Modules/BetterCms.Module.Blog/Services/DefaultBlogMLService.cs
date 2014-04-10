using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BlogML.Xml;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogMLService : IBlogMLService
    {
        private readonly IRepository repository;

        private readonly IUrlService urlService;
        
        private readonly IBlogService blogService;
        
        private readonly IUnitOfWork unitOfWork;
        
        private readonly IRedirectService redirectService;

        public DefaultBlogMLService(IRepository repository, IUrlService urlService, IBlogService blogService,
            IUnitOfWork unitOfWork, IRedirectService redirectService)
        {
            this.repository = repository;
            this.urlService = urlService;
            this.blogService = blogService;
            this.unitOfWork = unitOfWork;
            this.redirectService = redirectService;
        }

        public BlogMLBlog DeserializeXMLFile(string filePath)
        {
            Stream fileStream;
            try
            {
                fileStream = File.OpenRead(filePath);
            }
            catch (Exception exc)
            {
                var message = "Failed to open file for reading";
                var logMessage = string.Format("Failed to open XML file {0} for reading.", filePath);
                throw new ValidationException(() => message, logMessage, exc);
            }

            BlogMLBlog blogPosts;
            try
            {
                blogPosts = BlogMLSerializer.Deserialize(fileStream);
            }
            catch (Exception exc)
            {
                var message = "Failed to deserialize XML file";
                var logMessage = string.Format("Failed to deserialize provided XML file {0}.", filePath);
                throw new ValidationException(() => message, logMessage, exc);
            }

            fileStream.Close();
            fileStream.Dispose();

            return blogPosts;
        }

        public List<BlogPost> ImportBlogs(BlogMLBlog blogPosts, IPrincipal principal, bool useOriginalUrls = false, bool createRedirects = false)
        {
            List<BlogPost> createdBlogPosts = null;
            if (blogPosts != null)
            {
                // Import authors and categories
                unitOfWork.BeginTransaction();
                var authors = ImportAuthors(blogPosts.Authors);
                var categories = ImportCategories(blogPosts.Categories);
                unitOfWork.Commit();

                createdBlogPosts = ImportBlogPosts(principal, authors, categories, blogPosts.Posts, useOriginalUrls, createRedirects);
            }

            return createdBlogPosts ?? new List<BlogPost>();
        }

        private List<BlogPost> ImportBlogPosts(IPrincipal principal, IDictionary<string, Guid> authors, IDictionary<string, Guid> categories,
            BlogMLBlog.PostCollection blogsML, bool useOriginalUrls = false, bool createRedirects = false)
        {
            var createdBlogPosts = new List<BlogPost>();
            if (blogsML != null)
            {
                foreach (var blogML in blogsML)
                {
                    var blogPostModel = new BlogPostViewModel
                        {
                           Title = blogML.PostName ?? blogML.Title,
                           IntroText = blogML.PostName != blogML.Title ? blogML.Title : null,
                           LiveFromDate = blogML.DateCreated.Date,
                           LiveToDate = null,
                           DesirableStatus = ContentStatus.Published,
                           Content = blogML.Content != null ? blogML.Content.UncodedText : null,
                           BlogUrl = null
                        };

                    var oldUrl = FixUrl(blogML.PostUrl);
                    if (useOriginalUrls)
                    {
                        blogPostModel.BlogUrl = oldUrl;
                    }
                    else
                    {
                        blogPostModel.BlogUrl = blogService.CreateBlogPermalink(blogPostModel.Title);
                    }

                    if (blogML.Authors != null && blogML.Authors.Count > 0)
                    {
                        blogPostModel.AuthorId = authors[blogML.Authors[0].Ref];
                    }
                    if (blogML.Categories != null && blogML.Categories.Count > 0)
                    {
                        blogPostModel.CategoryId = categories[blogML.Categories[0].Ref];
                    }

                    var blogPost = blogService.SaveBlogPost(blogPostModel, principal);
                    createdBlogPosts.Add(blogPost);

                    if (!useOriginalUrls && createRedirects && oldUrl != blogPostModel.BlogUrl)
                    {
                        var redirect = redirectService.GetPageRedirect(oldUrl);
                        if (redirect == null)
                        {
                            redirect = redirectService.CreateRedirectEntity(oldUrl, blogPostModel.BlogUrl);
                            repository.Save(redirect);
                            unitOfWork.Commit();
                        }
                    }
                }
            }

            return createdBlogPosts;
        }

        private string FixUrl(string url)
        {
            foreach (var ending in new[] { ".asp", ".aspx", ".php", ".htm", ".html" })
            {
                if (url.EndsWith(ending))
                {
                    url = url.Substring(0, url.LastIndexOf(ending, StringComparison.InvariantCulture));
                }
            }
            url = urlService.FixUrl(url);

            return url;
        }

        private IDictionary<string, Guid> ImportAuthors(BlogMLBlog.AuthorCollection authors)
        {
            var dictionary = new Dictionary<string, Guid>();
            if (authors != null)
            {
                foreach (var authorML in authors)
                {
                    var id = repository
                        .AsQueryable<Author>()
                        .Where(a => a.Name == authorML.Title)
                        .Select(a => a.Id)
                        .FirstOrDefault();

                    if (id.HasDefaultValue())
                    {
                        var author = new Author { Name = authorML.Title };
                        repository.Save(author);

                        id = author.Id;
                    }
                    
                    dictionary.Add(authorML.ID, id);
                }
            }

            return dictionary;
        }

        private IDictionary<string, Guid> ImportCategories(BlogMLBlog.CategoryCollection categories)
        {
            var dictionary = new Dictionary<string, Guid>();
            if (categories != null)
            {
                foreach (var categoryML in categories)
                {
                    var id = repository
                        .AsQueryable<Category>()
                        .Where(a => a.Name == categoryML.Title)
                        .Select(a => a.Id)
                        .FirstOrDefault();

                    if (id.HasDefaultValue())
                    {
                        var category = new Category { Name = categoryML.Title };
                        repository.Save(category);

                        id = category.Id;
                    }

                    dictionary.Add(categoryML.ID, id);
                }
            }

            return dictionary;
        }
    }
}