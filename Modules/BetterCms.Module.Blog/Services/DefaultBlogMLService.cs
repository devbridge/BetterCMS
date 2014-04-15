using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BlogML.Xml;

using Common.Logging;

using ValidationException = BetterCms.Core.Exceptions.Mvc.ValidationException;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogMLService : IBlogMLService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

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

            return DeserializeXMLStream(fileStream);
        }

        public BlogMLBlog DeserializeXMLStream(Stream stream)
        {
            BlogMLBlog blogPosts;
            try
            {
                blogPosts = BlogMLSerializer.Deserialize(stream);
            }
            catch (Exception exc)
            {
                var message = "Failed to deserialize XML file";
                var logMessage = "Failed to deserialize provided XML file.";
                throw new ValidationException(() => message, logMessage, exc);
            }

            stream.Close();
            stream.Dispose();

            return blogPosts;
        }

        public List<BlogPostImportResult> ImportBlogs(BlogMLBlog blogPosts, IPrincipal principal, bool useOriginalUrls = false, bool createRedirects = false)
        {
            List<BlogPostImportResult> createdBlogPosts = null;
            if (blogPosts != null)
            {
                // Import authors and categories
                unitOfWork.BeginTransaction();

                var createdCategories = new List<Category>();
                var createdAuthors = new List<Author>();
                var authors = ImportAuthors(blogPosts.Authors, createdAuthors);
                var categories = ImportCategories(blogPosts.Categories, createdCategories);

                unitOfWork.Commit();

                // Notify authors / categories created
                createdCategories.ForEach(c => Events.RootEvents.Instance.OnCategoryCreated(c));
                createdAuthors.ForEach(a => Events.BlogEvents.Instance.OnAuthorCreated(a));

                // Import blog posts
                createdBlogPosts = ImportBlogPosts(principal, authors, categories, blogPosts.Posts, useOriginalUrls, createRedirects);
            }

            return createdBlogPosts ?? new List<BlogPostImportResult>();
        }

        private List<BlogPostImportResult> ImportBlogPosts(IPrincipal principal, IDictionary<string, Guid> authors, IDictionary<string, Guid> categories,
            BlogMLBlog.PostCollection blogsML, bool useOriginalUrls = false, bool createRedirects = false)
        {
            var createdBlogPosts = new List<BlogPostImportResult>();
            if (blogsML != null)
            {
                foreach (var blogML in blogsML)
                {
                    try
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

                        var validationContext = new ValidationContext(blogPostModel, null, null); 
                        var validationResults = new List<ValidationResult>();
                        if (!Validator.TryValidateObject(blogPostModel, validationContext, validationResults, true)
                            && validationResults.Count > 0)
                        {
                            var failedBlogPost = new BlogPostImportResult
                            {
                                Title = blogML.Title,
                                PageUrl = blogML.PostUrl,
                                Success = false,
                                ErrorMessage = validationResults[0].ErrorMessage
                            };
                            createdBlogPosts.Add(failedBlogPost);
                            continue;
                        }

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
                        createdBlogPosts.Add(new BlogPostImportResult
                                             {
                                                 Title = blogPost.Title, 
                                                 PageUrl = blogPost.PageUrl, 
                                                 Id = blogPost.Id, 
                                                 Success = true
                                             });

                        if (!useOriginalUrls && createRedirects && oldUrl != blogPostModel.BlogUrl)
                        {
                            var redirect = redirectService.GetPageRedirect(oldUrl);
                            if (redirect == null)
                            {
                                redirect = redirectService.CreateRedirectEntity(oldUrl, blogPostModel.BlogUrl);
                                repository.Save(redirect);
                                unitOfWork.Commit();
                                Events.PageEvents.Instance.OnRedirectCreated(redirect);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        var failedBlogPost = new BlogPostImportResult
                                             {
                                                 Title = blogML.Title,
                                                 PageUrl = blogML.PostUrl, 
                                                 Success = false,
                                                 ErrorMessage = exc.Message
                                             };
                        createdBlogPosts.Add(failedBlogPost);

                        Log.Error("Failed to import blog post.", exc);
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

        private IDictionary<string, Guid> ImportAuthors(BlogMLBlog.AuthorCollection authors, IList<Author> createdAuthors)
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

                        createdAuthors.Add(author);

                        id = author.Id;
                    }
                    
                    dictionary.Add(authorML.ID, id);
                }
            }

            return dictionary;
        }

        private IDictionary<string, Guid> ImportCategories(BlogMLBlog.CategoryCollection categories, IList<Category> createdCategories)
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
                        
                        createdCategories.Add(category);

                        id = category.Id;
                    }

                    dictionary.Add(categoryML.ID, id);
                }
            }

            return dictionary;
        }
    }
}