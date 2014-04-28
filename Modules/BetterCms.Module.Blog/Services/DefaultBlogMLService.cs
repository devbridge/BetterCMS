using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Schema;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Web;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BlogML;
using BlogML.Xml;

using Common.Logging;

using TypeHelperExtensionMethods = NHibernate.Linq.TypeHelperExtensionMethods;
using ValidationException = BetterCms.Core.Exceptions.Mvc.ValidationException;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogMLService : IBlogMLService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IRepository repository;

        private readonly IUrlService urlService;
        
        private readonly IBlogService blogService;
        
        private readonly IPageService pageService;
        
        private readonly IUnitOfWork unitOfWork;
        
        private readonly IRedirectService redirectService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IHttpContextAccessor httpContextAccessor;

        public DefaultBlogMLService(IRepository repository, IUrlService urlService, IBlogService blogService,
            IUnitOfWork unitOfWork, IRedirectService redirectService, IPageService pageService, ICmsConfiguration cmsConfiguration,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.urlService = urlService;
            this.blogService = blogService;
            this.pageService = pageService;
            this.unitOfWork = unitOfWork;
            this.redirectService = redirectService;
            this.cmsConfiguration = cmsConfiguration;
            this.httpContextAccessor = httpContextAccessor;
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
            try
            {
                stream.Position = 0;

                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.ConformanceLevel = ConformanceLevel.Document;
                readerSettings.CheckCharacters = true;
                readerSettings.ValidationType = ValidationType.None;

                var xmlReader = XmlReader.Create(stream, readerSettings);
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlReader);
            }
            catch (Exception exc)
            {
                var message = BlogGlobalization.ImportBlogPosts_FileIsNotValidXML_Message;
                var logMessage = "Provided file is not a valid XML file.";
                throw new ValidationException(() => message, logMessage, exc);
            }

            BlogMLBlog blogPosts;
            try
            {
                stream.Position = 0;
                blogPosts = BlogMLSerializer.Deserialize(stream);
            }
            catch (Exception exc)
            {
                var message = BlogGlobalization.ImportBlogPosts_FailedToDeserializeXML_Message;
                var logMessage = "Failed to deserialize provided XML file.";
                throw new ValidationException(() => message, logMessage, exc);
            }

            stream.Close();
            stream.Dispose();

            return blogPosts;
        }

        private BlogPostViewModel MapViewModel(BlogMLPost blogML, BlogPostImportResult modification = null, List<string> unsavedUrls = null)
        {
            var model = new BlogPostViewModel
                    {
                        Title = blogML.PostName ?? blogML.Title,
                        MetaTitle = blogML.Title,
                        IntroText = blogML.Excerpt != null ? blogML.Excerpt.UncodedText : null,
                        LiveFromDate = blogML.DateCreated.Date,
                        LiveToDate = null,
                        DesirableStatus = ContentStatus.Published,
                        Content = blogML.Content != null ? blogML.Content.UncodedText : null
                    };

            if (modification != null)
            {
                model.BlogUrl = modification.PageUrl;
                model.Title = modification.Title;
            }
            else
            {
                model.BlogUrl = blogService.CreateBlogPermalink(blogML.PostName ?? blogML.Title, unsavedUrls);
            }

            return model;
        }

        private bool ValidateModel(BlogPostViewModel blogPostModel, BlogMLPost blogML, out BlogPostImportResult failedResult)
        {
            failedResult = null;

            if (string.IsNullOrWhiteSpace(blogML.ID))
            {
                failedResult = new BlogPostImportResult
                {
                    Title = blogML.PostName ?? blogML.Title,
                    PageUrl = blogML.PostUrl,
                    Success = false,
                    ErrorMessage = BlogGlobalization.ImportBlogPosts_ImportingBlogPostIdIsNotSet_Message,
                    Id = blogML.ID
                };
                return false;
            }

            var validationContext = new ValidationContext(blogPostModel, null, null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(blogPostModel, validationContext, validationResults, true)
                && validationResults.Count > 0)
            {
                failedResult = new BlogPostImportResult
                    {
                        Title = blogML.PostName ?? blogML.Title,
                        PageUrl = blogML.PostUrl,
                        Success = false,
                        ErrorMessage = validationResults[0].ErrorMessage,
                        Id = blogML.ID
                    };
                return false;
            }

            try
            {
                pageService.ValidatePageUrl(blogPostModel.BlogUrl);
            }
            catch (Exception exc)
            {
                failedResult = new BlogPostImportResult
                {
                    Title = blogML.PostName ?? blogML.Title,
                    PageUrl = blogML.PostUrl,
                    Success = false,
                    ErrorMessage = exc.Message,
                    Id = blogML.ID
                };
                return false;
            }

            return true;
        }

        public List<BlogPostImportResult> ValidateImport(BlogMLBlog blogPosts)
        {
            List<BlogPostImportResult> result = new List<BlogPostImportResult>();
            var unsavedUrls = new List<string>();

            if (blogPosts != null && blogPosts.Posts != null)
            {
                foreach (var blogML in blogPosts.Posts)
                {
                    var blogPostModel = MapViewModel(blogML, null, unsavedUrls);
                    unsavedUrls.Add(blogPostModel.BlogUrl);

                    BlogPostImportResult blogPost;
                    if (!ValidateModel(blogPostModel, blogML, out blogPost))
                    {
                        result.Add(blogPost);
                        continue;
                    }

                    blogPost = new BlogPostImportResult
                        {
                            Title = blogML.PostName ?? blogML.Title,
                            PageUrl = blogPostModel.BlogUrl,
                            Success = true,
                            Id = blogML.ID
                        };
                    result.Add(blogPost);
                }

                // Validate for duplicate IDS
                result
                    .Where(bp => bp.Success)
                    .GroupBy(bp => bp.Id)
                    .Where(group => group.Count() > 1)
                    .ToList()
                    .ForEach(group => group
                        .ToList()
                        .ForEach(
                            bp =>
                            {
                                bp.Success = false;
                                bp.ErrorMessage = string.Format(BlogGlobalization.ImportBlogPosts_DuplicateId_Message, group.Key);
                            }));
            }

            return result;
        }

        public List<BlogPostImportResult> ImportBlogs(BlogMLBlog blogPosts, List<BlogPostImportResult> modifications, 
            IPrincipal principal, bool createRedirects = false)
        {
            List<BlogPostImportResult> createdBlogPosts = null;
            if (blogPosts != null)
            {
                var blogs = new List<BlogMLPost>();
                foreach (var blogML in blogPosts.Posts)
                {
                    var requestBlogPost = modifications.FirstOrDefault(rb => rb.Id == blogML.ID);
                    if (requestBlogPost != null)
                    {
                        blogs.Add(blogML);
                    }
                }

                // Import authors and categories
                unitOfWork.BeginTransaction();

                var createdCategories = new List<Category>();
                var createdAuthors = new List<Author>();
                var authors = ImportAuthors(blogPosts.Authors, createdAuthors, blogs);
                var categories = ImportCategories(blogPosts.Categories, createdCategories, blogs);

                unitOfWork.Commit();

                // Notify authors / categories created
                createdCategories.ForEach(c => Events.RootEvents.Instance.OnCategoryCreated(c));
                createdAuthors.ForEach(a => Events.BlogEvents.Instance.OnAuthorCreated(a));

                // Import blog posts
                createdBlogPosts = ImportBlogPosts(principal, authors, categories, blogs, modifications, createRedirects);
            }

            return createdBlogPosts ?? new List<BlogPostImportResult>();
        }

        private List<BlogPostImportResult> ImportBlogPosts(IPrincipal principal, IDictionary<string, Guid> authors, IDictionary<string, Guid> categories,
            List<BlogMLPost> blogs, List<BlogPostImportResult> modifications, bool createRedirects = false)
        {
            var createdBlogPosts = new List<BlogPostImportResult>();
            if (blogs != null)
            {
                foreach (var blogML in blogs)
                {
                    try
                    {
                        var blogPostModel = MapViewModel(blogML, modifications.First(m => m.Id == blogML.ID));

                        BlogPostImportResult blogPostResult;
                        if (!ValidateModel(blogPostModel, blogML, out blogPostResult))
                        {
                            createdBlogPosts.Add(blogPostResult);
                            continue;
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
                        
                        blogPostResult = new BlogPostImportResult
                                {
                                    Title = blogML.PostName ?? blogML.Title, 
                                    PageUrl = blogPost.PageUrl, 
                                    Id = blogPost.Id.ToString(), 
                                    Success = true
                                };
                        createdBlogPosts.Add(blogPostResult);

                        if (createRedirects)
                        {
                            var oldUrl = TryValidateOldUrl(blogML.PostUrl);
                            if (oldUrl != null && oldUrl != blogPostModel.BlogUrl)
                            {
                                var redirect = redirectService.GetPageRedirect(oldUrl);
                                if (redirect == null)
                                {
                                    redirect = redirectService.CreateRedirectEntity(oldUrl, blogPostModel.BlogUrl);
                                    repository.Save(redirect);
                                    unitOfWork.Commit();
                                    Events.PageEvents.Instance.OnRedirectCreated(redirect);
                                }
                                else
                                {
                                    blogPostResult.WarnMessage = string.Format(BlogGlobalization.ImportBlogPosts_RedirectWasAlreadyCreatedFor_Message, blogML.PostUrl);
                                }
                            }
                            else if (oldUrl == null)
                            {
                                blogPostResult.WarnMessage = string.Format(BlogGlobalization.ImportBlogPosts_FailedToCreateRedirectFor_Message, blogML.PostUrl);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        var failedBlogPost = new BlogPostImportResult
                                             {
                                                 Title = blogML.PostName ?? blogML.Title,
                                                 PageUrl = blogML.PostUrl, 
                                                 Success = false,
                                                 ErrorMessage = exc.Message,
                                                 Id = blogML.ID
                                             };
                        createdBlogPosts.Add(failedBlogPost);

                        Log.Error("Failed to import blog post.", exc);
                    }
                }
            }

            return createdBlogPosts;
        }

        private string TryValidateOldUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            if (!urlService.ValidateInternalUrl(url))
            {
                var serverPath = httpContextAccessor.MapPublicPath("/").TrimEnd('/');
                if (url.StartsWith(serverPath) && serverPath != url)
                {
                    url = url.Substring(serverPath.Length, url.Length - serverPath.Length);
                    if (!urlService.ValidateInternalUrl(url))
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            
            return url;
        }

        private IDictionary<string, Guid> ImportAuthors(BlogMLBlog.AuthorCollection authors, IList<Author> createdAuthors, List<BlogMLPost> blogs)
        {
            var dictionary = new Dictionary<string, Guid>();
            if (authors != null)
            {
                foreach (var authorML in authors)
                {
                    if (!blogs.Any(b => b.Authors != null && b.Authors.Cast<BlogMLAuthorReference>().Any(c => c.Ref == authorML.ID)))
                    {
                        continue;
                    }

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

        private IDictionary<string, Guid> ImportCategories(BlogMLBlog.CategoryCollection categories, IList<Category> createdCategories, List<BlogMLPost> blogs)
        {
            var dictionary = new Dictionary<string, Guid>();
            if (categories != null)
            {
                foreach (var categoryML in categories)
                {
                    if (!blogs.Any(b => b.Categories != null && b.Categories.Cast<BlogMLCategoryReference>().Any(c => c.Ref == categoryML.ID)))
                    {
                        continue;
                    }

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

        public Uri ConstructFilePath(Guid guid)
        {
            var contentRoot = GetContentRoot(cmsConfiguration.Storage.ContentRoot);
            var folderId = guid.ToString().Replace("-", string.Empty).ToLower();

            return new Uri(Path.Combine(contentRoot, "import", folderId, "blog-posts.xml"));
        }

        private string GetContentRoot(string rootPath)
        {
            if (cmsConfiguration.Storage.ServiceType == StorageServiceType.FileSystem && VirtualPathUtility.IsAppRelative(rootPath))
            {
                return httpContextAccessor.MapPath(rootPath);
            }

            return rootPath;
        }
    }
}