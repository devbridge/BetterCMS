using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Xml;

using BetterCms.Configuration;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BlogML.Xml;

using Common.Logging;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Web;

using FluentNHibernate.Utils;

using NHibernate.Linq;
using NHibernate.Util;

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
                        LiveFromDate = blogML.DateCreated.ToLocalTime().Date,
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
                failedResult = CreateFailedResult(blogML);
                failedResult.ErrorMessage = BlogGlobalization.ImportBlogPosts_ImportingBlogPostIdIsNotSet_Message;

                return false;
            }

            var validationContext = new ValidationContext(blogPostModel, null, null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(blogPostModel, validationContext, validationResults, true)
                && validationResults.Count > 0)
            {
                failedResult = CreateFailedResult(blogML);
                failedResult.ErrorMessage = validationResults[0].ErrorMessage;

                return false;
            }

            try
            {
                pageService.ValidatePageUrl(blogPostModel.BlogUrl);
            }
            catch (Exception exc)
            {
                failedResult = CreateFailedResult(blogML);
                failedResult.ErrorMessage = exc.Message;

                return false;
            }

            return true;
        }

        private BlogPostImportResult CreateFailedResult(BlogMLPost blogML)
        {
            return new BlogPostImportResult
                   {
                       Title = blogML.PostName ?? blogML.Title,
                       PageUrl = blogML.PostUrl,
                       Success = false,
                       Id = blogML.ID
                   };
        }

        public List<BlogPostImportResult> ValidateImport(BlogMLBlog blogPosts)
        {
            List<BlogPostImportResult> result = new List<BlogPostImportResult>();
            var unsavedUrls = new List<string>();

            if (blogPosts != null && blogPosts.Posts != null)
            {
                var existingCategoriesIds = new List<string>();
                // old type imports had only id to ref categories new also has categories to recreate
                var newCategoryImport = blogPosts.Categories.Count > 0;
                var importWithCategories = blogPosts.Posts.Any(t => t.Categories.Any());
                var importCategoriesIds = new List<Guid>();
                foreach (var blogML in blogPosts.Posts)
                {
                    for (var i = 0; i < blogML.Categories.Count; i++)
                    {
                        Guid id;
                        if (Guid.TryParse(blogML.Categories[i].Ref, out id))
                        {
                            importCategoriesIds.Add(id);
                        }
                    }
                }
                importCategoriesIds = importCategoriesIds.Distinct().ToList();
                if (!newCategoryImport && importWithCategories)
                {
                    existingCategoriesIds = repository.AsQueryable<Category>().Where(t => importCategoriesIds.Contains(t.Id)).Select(t => t.Id.ToString()).ToList();
                }
                foreach (var category in blogPosts.Categories)
                {
                    existingCategoriesIds.Add(category.ID);
                }
                foreach (var blogML in blogPosts.Posts)
                {
                    // Validate authors
                    if (blogML.Authors != null &&
                        blogML.Authors.Count > 0 &&
                        (blogPosts.Authors == null || blogPosts.Authors.All(a => a.ID != blogML.Authors[0].Ref)))
                    {
                        var failedResult = CreateFailedResult(blogML);
                        failedResult.ErrorMessage = string.Format(BlogGlobalization.ImportBlogPosts_AuthorByRefNotFound_Message, blogML.Authors[0].Ref);
                        result.Add(failedResult);

                        continue;
                    }
                    
                    // Validate categories
                    var blogMlCategoryReference = BlogMlCategoryCollectionToEnumerable(blogML.Categories).FirstOrDefault(t => !existingCategoriesIds.Contains(t.Ref));
                    if (blogMlCategoryReference != null)
                    {
                        var failedResult = CreateFailedResult(blogML);
                        failedResult.ErrorMessage = string.Format(BlogGlobalization.ImportBlogPosts_CategoryByRefNotFound_Message, blogMlCategoryReference.Ref);
                        result.Add(failedResult);

                        continue;
                    }

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

        private IEnumerable<BlogMLCategoryReference> BlogMlCategoryCollectionToEnumerable(BlogMLPost.CategoryReferenceCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                yield return collection[i];
            }
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

                var newTreeList = new List<CategoryTree>();
                var newCategoriesList = new List<Category>();

                var createdAuthors = new List<Author>();
                var authors = ImportAuthors(blogPosts.Authors, createdAuthors, blogs);
                var categories = ImportCategories(blogPosts.Categories, blogs, ref newTreeList, ref newCategoriesList);

                unitOfWork.Commit();

                // Notify authors, categories created
                createdAuthors.ForEach(a => Events.BlogEvents.Instance.OnAuthorCreated(a));
                newTreeList.ForEach(Events.RootEvents.Instance.OnCategoryTreeCreated);
                newCategoriesList.ForEach(Events.RootEvents.Instance.OnCategoryCreated);

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
                    BlogPostViewModel blogPostModel = null;

                    try
                    {
                        blogPostModel = MapViewModel(blogML, modifications.First(m => m.Id == blogML.ID));

                        BlogPostImportResult blogPostResult = null;
                        if (!ValidateModel(blogPostModel, blogML, out blogPostResult))
                        {
                            createdBlogPosts.Add(blogPostResult);
                            continue;
                        }

                        if (blogML.Authors != null && blogML.Authors.Count > 0)
                        {
                            blogPostModel.AuthorId = authors[blogML.Authors[0].Ref];
                        }
                        if (blogML.Categories != null && blogML.Categories.Count > 0 && categories != null && categories.Count > 0)
                        {
                            for (var i = 0; i < blogML.Categories.Count; i++)
                            {
                                var category = blogML.Categories[i];

                                if (blogPostModel.Categories == null)
                                {
                                    blogPostModel.Categories = new List<LookupKeyValue>();
                                }

                                blogPostModel.Categories.Add(new LookupKeyValue() {Key = categories[category.Ref].ToLowerInvariantString()});                                
                            }                            
                        }

                        string[] error;
                        var blogPost = blogService.SaveBlogPost(blogPostModel, null, principal, out error);
                        if (blogPost == null)
                        {
                            blogPostResult = new BlogPostImportResult
                            {
                                Title = blogML.PostName ?? blogML.Title,
                                Success = false,
                                ErrorMessage = error.Length > 0 ? error[0] : string.Empty
                            };
                            createdBlogPosts.Add(blogPostResult);

                            continue;
                        }
                        
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
                                                 PageUrl = blogPostModel != null && blogPostModel.BlogUrl != null ? blogPostModel.BlogUrl : blogML.PostUrl, 
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

            url = urlService.FixUrl(url);
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

        private IDictionary<string, Guid> ImportCategories(BlogMLBlog.CategoryCollection categories, 
                                                           IEnumerable<BlogMLPost> blogs,
                                                           ref List<CategoryTree> newCategoriesTreeList,
                                                           ref List<Category> newCategoriesList)
        {
            var dictionary = new Dictionary<string, Guid>();
            if (categories.Count == 0)
            {
                return dictionary;
            }


            // sort categories that parents would be first
            for (int i = 0; i < categories.Count; i++)
                {
                for (int j = 0; j < categories.Count; j++)
                    {
                    if (categories[i].ID == categories[j].ParentRef && i>j)
                    {
                        var tmp = categories[j];
                        categories[j] = categories[i];
                        categories[i] = tmp;
                    }
                }
            }
            var newIdsForCategories = new Dictionary<string, Guid>();
            // regenerate new ids for category tree
            foreach (var category in categories)
            {
                if (!newIdsForCategories.ContainsKey(category.ID))
                {
                    newIdsForCategories.Add(category.ID, Guid.NewGuid());
                }
                if (!string.IsNullOrEmpty(category.ParentRef) && !newIdsForCategories.ContainsKey(category.ParentRef))
                {
                    newIdsForCategories.Add(category.ParentRef, Guid.NewGuid());
                }
            }
            foreach (var category in categories)
            {
                category.ID = newIdsForCategories[category.ID].ToString();
                if (!string.IsNullOrEmpty(category.ParentRef))
                {
                    category.ParentRef = newIdsForCategories[category.ParentRef].ToString();
                }
            }
            var refrencedCategoriesIds = new List<Guid>();
            foreach (var blog in blogs)
            {
                for (int i = 0; i < blog.Categories.Count; i++)
                {
                    var category = blog.Categories[i];
                    var newCategoryId = newIdsForCategories[category.Ref];
                    category.Ref = newCategoryId.ToString();
                    refrencedCategoriesIds.Add(newCategoryId);
                }
            }

            Guid testParseGuid;
            var oldIdsCategories = newIdsForCategories.Keys.Where(t => Guid.TryParse(t, out testParseGuid)).Select(t => new Guid(t)).ToList();
            var existingCategoriesIdsFuture = repository.AsQueryable<Category>().Select(t => t.Id).Where(t => oldIdsCategories.Contains(t)).ToFuture();
            var availableFor = repository.AsQueryable<CategorizableItem>().ToFuture().ToList();
            var existingCategoriesIds = existingCategoriesIdsFuture.ToList();

            // creates categories trees
            foreach (var categoryTree in categories.Where(c => string.IsNullOrEmpty(c.ParentRef)))
                    {
                var newTree = new CategoryTree
                {
                    Title = categoryTree.Title,
                    CreatedOn = categoryTree.DateCreated,
                    ModifiedOn = categoryTree.DateModified,
                    Id = new Guid(categoryTree.ID)
                };
                newTree.AvailableFor = new List<CategoryTreeCategorizableItem>();
                foreach (var categorizableItem in availableFor)
                {
                    newTree.AvailableFor.Add(new CategoryTreeCategorizableItem{CategorizableItem = categorizableItem, CategoryTree = newTree});
                }
                newCategoriesTreeList.Add(newTree);
            }

            // create categories
            foreach (var category in categories.Where(t => !string.IsNullOrEmpty(t.ParentRef)))
            {
                var categoryParentId = new Guid(category.ParentRef);
                var newParentCategoryTree = newCategoriesTreeList.FirstOrDefault(t=>t.Id == categoryParentId);
                var newCategory = new Category
                {
                    Name = category.Title,
                    CreatedOn = category.DateCreated,
                    ModifiedOn = category.DateModified,
                    Id = new Guid(category.ID),
                    CategoryTree = newParentCategoryTree
                };
                newCategoriesList.Add(newCategory);
                    }

            // set references
            foreach (var category in newCategoriesList.Where(t=>t.CategoryTree == null))
            {
                category.CategoryTree = newCategoriesList.First(t => t.Id.ToString() == categories.First(c => c.ID == category.Id.ToString()).ParentRef).CategoryTree;
                }
            foreach (var tree in newCategoriesTreeList)
            {
                tree.Categories = newCategoriesList.Where(t => t.CategoryTree != null && t.CategoryTree.Id == tree.Id).ToList();
            }
            foreach (var child in newCategoriesList)
            {
                var parentId = new Guid(categories.First(t => t.ID == child.Id.ToString()).ParentRef);
                var parentCategory = newCategoriesList.FirstOrDefault(t => t.Id == parentId);
                if (parentCategory != null)
                {
                    child.ParentCategory = parentCategory;
                    parentCategory.ChildCategories = parentCategory.ChildCategories ?? new List<Category>();
                    parentCategory.ChildCategories.Add(child);
                }
            }

            // generate category for empty category tree
            foreach (var categoryTree in newCategoriesTreeList.Where(t => t.Categories != null && t.Categories.Count == 0))
            {
                var category = new Category
                {
                    Id = categoryTree.Id,
                    CreatedOn = categoryTree.CreatedOn,
                    ModifiedOn = categoryTree.ModifiedOn,
                    Name = categoryTree.Title,
                    CategoryTree = categoryTree
                };
                categoryTree.Id = Guid.NewGuid();
                categoryTree.Categories.Add(category);
                newCategoriesList.Add(category);
            }

            // removing referenced categories that they would not be saved and adding existing category id from db
            foreach (var existingCategoriesId in existingCategoriesIds)
            {
                refrencedCategoriesIds.Remove(newIdsForCategories[existingCategoriesId.ToLowerInvariantString()]);
                dictionary.Add(existingCategoriesId.ToString(), existingCategoriesId);
                foreach (var blog in blogs)
                {
                    for (int i = 0; i < blog.Categories.Count; i++)
                    {
                        if (blog.Categories[i].Ref == newIdsForCategories[existingCategoriesId.ToLowerInvariantString()].ToLowerInvariantString())
                        {
                            blog.Categories[i].Ref = existingCategoriesId.ToLowerInvariantString();
                        }
                    }
                }
            }
            
            // filter, save only categories who are related to imported blogs
            newCategoriesTreeList = newCategoriesTreeList.Where(t => t.Categories.Any(z => refrencedCategoriesIds.Contains(z.Id))).ToList();
            newCategoriesList = newCategoriesList.Where(t => t.CategoryTree != null && t.CategoryTree.Categories.Any(z => refrencedCategoriesIds.Contains(z.Id))).ToList();
            // save new records
            foreach (var category in newCategoriesList)
            {
                dictionary.Add(category.Id.ToString(), category.Id);
                unitOfWork.Session.Save(category);
            }
            foreach (var tree in newCategoriesTreeList)
            {
                unitOfWork.Session.Save(tree);
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