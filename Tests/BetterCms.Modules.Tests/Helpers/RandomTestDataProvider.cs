using System;
using System.Collections.Generic;
using System.Text;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Users;
using BetterCms.Module.Users.Models;

using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.Models;

using NHibernate;

using BlogOption = BetterCms.Module.Blog.Models.Option;

namespace BetterCms.Tests.Helpers
{
    public class RandomTestDataProvider
    {
        private readonly Random random = new Random();

        public string ProvideRandomString(int length)
        {
            var sb = new StringBuilder();

            while (sb.Length < length)
            {
                sb.Append(Guid.NewGuid().ToString().Replace("-", string.Empty));
            }

            return sb.ToString().Substring(0, length);
        }

        public string ProvideRandomStringMaxLength()
        {
            return ProvideRandomString(8000);
        }

        public bool ProvideRandomBooleanValue()
        {
            return (random.Next(2) > 0);
        }

        public int ProvideRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        public long ProvideRandomNumber(long min, long max)
        {
            return (long)(random.NextDouble() * (max - min)) + min;
        }

        public decimal ProvideRandomNumber(decimal min, decimal max)
        {
            return (decimal)((double)min + (random.NextDouble() * ((double)max - (double)min)));
        }

        public double ProvideRandomNumber(double min, double max)
        {
            return min + (random.NextDouble() * (max - min));
        }

        public DateTime ProvideRandomDateTime()
        {
            return new DateTime(
                ProvideRandomNumber(1990, 2019),
                ProvideRandomNumber(1, 12),
                ProvideRandomNumber(1, 29),
                ProvideRandomNumber(0, 23),
                ProvideRandomNumber(0, 59),
                ProvideRandomNumber(0, 59));
        }

        public TEnum ProvideRandomEnumValue<TEnum>()
        {
            var values = Enum.GetValues(typeof(TEnum));
            int index = ProvideRandomNumber(0, values.Length - 1);
            return (TEnum)values.GetValue(index);
        }

        public Guid ProvideRandomGuid()
        {
            return Guid.NewGuid();
        }

        public void PopulateBaseFields<TEntity>(EquatableEntity<TEntity> entity, User user = null)
            where TEntity : EquatableEntity<TEntity>
        {
            entity.Id = Guid.NewGuid();
            /*  entity.IsDeleted = false;
            entity.CreatedOn = ProvideRandomDateTime();
            entity.CreatedByUser = ProvideRandomString(256);
            entity.ModifiedOn = ProvideRandomDateTime();
            entity.ModifiedByUser = ProvideRandomString(256);
            entity.DeletedOn = ProvideRandomDateTime();
            entity.DeletedByUser = ProvideRandomString(256);*/
        }

        public Page CreateNewPage(Layout layout = null)
        {
            var entity = new Page();

            PopulateBaseFields(entity);

            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.PageUrl = ProvideRandomString(MaxLength.Url);
            entity.PageUrlHash = ProvideRandomString(MaxLength.Url).UrlHash();
            entity.Status = PageStatus.Published;
            entity.PublishedOn = ProvideRandomDateTime();
            entity.Layout = layout ?? CreateNewLayout();
            entity.Language = CreateNewLanguage();
            entity.LanguageGroupIdentifier = ProvideRandomGuid();

            return entity;
        }

        public PageProperties CreateNewPageProperties(Layout layout = null)
        {
            var entity = new PageProperties();

            PopulatePageProperties(entity, layout);

            return entity;
        }

        public BlogPost CreateNewBlogPost()
        {
            var entity = new BlogPost();

            PopulatePageProperties(entity);

            entity.Author = CreateNewAuthor();
            entity.ActivationDate = ProvideRandomDateTime();
            entity.ExpirationDate = ProvideRandomDateTime();

            return entity;
        }

        public BlogPostContent CreateNewBlogPostContent()
        {
            var entity = new BlogPostContent();

            PopulateHtmlContentProperties(entity);

            return entity;
        }

        public BlogOption CreateNewBlogOption()
        {
            var entity = new BlogOption();

            PopulateBaseFields(entity);

            entity.DefaultLayout = CreateNewLayout();
            entity.DefaultContentTextMode = ContentTextMode.Html;

            return entity;
        }

        private void PopulatePageProperties(PageProperties entity, Layout layout = null)
        {
            PopulateBaseFields(entity);

            entity.Status = ProvideRandomEnumValue<PageStatus>();
            entity.PublishedOn = ProvideRandomDateTime();
            entity.PageUrl = ProvideRandomString(MaxLength.Url);
            entity.PageUrlHash = ProvideRandomString(MaxLength.Url).UrlHash();
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.Description = ProvideRandomString(2000);
            entity.CustomCss = ProvideRandomString(2000);
            entity.CustomJS = ProvideRandomString(2000);
            entity.MetaTitle = ProvideRandomString(MaxLength.Name);
            entity.MetaKeywords = ProvideRandomString(MaxLength.Text);
            entity.MetaDescription = ProvideRandomString(MaxLength.Text);

            entity.Layout = layout ?? CreateNewLayout();

            var categoryTree = CreateNewCategoryTree();

            entity.Categories = new List<PageCategory>() {new PageCategory(){ Category = CreateNewCategory(categoryTree), Page = entity}};             
            entity.Image = CreateNewMediaImage();
            entity.FeaturedImage = CreateNewMediaImage();
            entity.SecondaryImage = CreateNewMediaImage();
            entity.Language = CreateNewLanguage();
        }

        public Content CreateNewContent()
        {
            var content = new Content();

            PopulateBaseFields(content);

            content.Name = ProvideRandomString(MaxLength.Name);
            content.PreviewUrl = ProvideRandomString(MaxLength.Url);            
            content.Status = ContentStatus.Published;
            content.Original = null;
            content.PublishedByUser = ProvideRandomString(MaxLength.Name);
            content.PublishedOn = ProvideRandomDateTime();

            return content;
        }

        public Layout CreateNewLayout()
        {
            var entity = new Layout();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.LayoutPath = ProvideRandomString(MaxLength.Url);
            entity.PreviewUrl = ProvideRandomString(MaxLength.Url);

            return entity;
        }

        public Region CreateNewRegion()
        {
            var entity = new Region();

            PopulateBaseFields(entity);
            
            entity.RegionIdentifier = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public Author CreateNewAuthor()
        {
            var entity = new Author();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Image = CreateNewMediaImage();

            return entity;
        }

        public CategoryTree CreateNewCategoryTree()
        {
            var entity = new CategoryTree();

            PopulateBaseFields(entity);

            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.Categories = new List<Category>();

            return entity;
        }

        public Category CreateNewCategory(CategoryTree tree)
        {
            var entity = new Category();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);

            entity.CategoryTree = tree;

            if (tree.Categories == null)
            {
                tree.Categories = new List<Category>();
            }
            tree.Categories.Add(entity);

            return entity;
        }

        public Language CreateNewLanguage()
        {
            var entity = new Language();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Code = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public Tag CreateNewTag()
        {
            var entity = new Tag();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public Module.Root.Models.Module CreateNewModule()
        {
            var enity = new Module.Root.Models.Module();

            PopulateBaseFields(enity);

            enity.Name = ProvideRandomString(MaxLength.Name);
            enity.Description = ProvideRandomString(MaxLength.Text);
            enity.ModuleVersion = "10.21.6.10";
            enity.Enabled = true;

            return enity;
        }

        public LayoutRegion CreateNewLayoutRegion(Layout layout = null, Region region = null)
        {
            var entity = new LayoutRegion();

            PopulateBaseFields(entity);

            entity.Description = ProvideRandomString(MaxLength.Name);
            entity.Layout = layout ?? CreateNewLayout();
            entity.Region = region ?? CreateNewRegion();

            return entity;
        }

        public PageContent CreateNewPageContent(Content content = null, Page page = null, Region region = null)
        {
            var entity = new PageContent();

            PopulateBaseFields(entity);

            entity.Order = 1;            
            entity.Content = content ?? CreateNewContent();
            entity.Page = page ?? CreateNewPage();
            entity.Region = region ?? CreateNewRegion();
            entity.Options = null;            
            
            return entity;
        }
       
        public ContentOption CreateNewContentOption(Content content = null)
        {
            var entity = new ContentOption();

            PopulateBaseFields(entity);

            entity.Key = ProvideRandomString(MaxLength.Name);
            entity.Content = content ?? CreateNewContent();
            entity.Type = ProvideRandomEnumValue<OptionType>();
            entity.DefaultValue = ProvideRandomString(100);

            return entity;
        }
        
        public LayoutOption CreateNewLayoutOption(Layout layout = null)
        {
            var entity = new LayoutOption();

            PopulateBaseFields(entity);

            entity.Key = ProvideRandomString(MaxLength.Name);
            entity.Layout = layout ?? CreateNewLayout();
            entity.Type = ProvideRandomEnumValue<OptionType>();
            entity.DefaultValue = ProvideRandomString(100);

            return entity;
        }  

        public PageContentOption CreateNewPageContentOption(PageContent pageContent = null)
        {
            var entity = new PageContentOption();

            PopulateBaseFields(entity);

            entity.PageContent = pageContent ?? CreateNewPageContent();
            entity.Key = ProvideRandomString(MaxLength.Name);
            entity.Value = ProvideRandomString(100);
            entity.Type = ProvideRandomEnumValue<OptionType>();
            
            return entity;
        }

        public PageOption CreateNewPageOption(Page page = null)
        {
            var entity = new PageOption();

            PopulateBaseFields(entity);

            entity.Page = page ?? CreateNewPage();
            entity.Key = ProvideRandomString(MaxLength.Name);
            entity.Value = ProvideRandomString(100);
            entity.Type = ProvideRandomEnumValue<OptionType>();
            
            return entity;
        }

        public WidgetCategory CreateWidgetCategory(Widget widget, Category category = null)
        {
            var entity = new WidgetCategory()
            {
                Category = category,
                Widget = widget
            };

            return entity;
        }

        public Widget CreateNewWidget(Category category = null)
        {
            var entity = new Widget();            
            var categoryTree = CreateNewCategoryTree();
            var widgetCategory = CreateWidgetCategory(entity, CreateNewCategory(categoryTree));

            PopulateBaseFields(entity);
            PopulateBaseFields(widgetCategory);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Categories = new List<WidgetCategory>() { widgetCategory };
            entity.Status = ContentStatus.Archived;
            entity.Original = null;
            entity.PublishedByUser = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();
            
            return entity;
        }
      
        public Redirect CreateNewRedirect()
        {
            var entity = new Redirect();

            PopulateBaseFields(entity);

            entity.PageUrl = ProvideRandomString(MaxLength.Url);
            entity.RedirectUrl = ProvideRandomString(MaxLength.Url);

            return entity;
        }

        public PageTag CreateNewPageTag(PageProperties page = null, Tag tag = null)
        {
            var entity = new PageTag();

            PopulateBaseFields(entity);

            entity.Page = page ?? CreateNewPageProperties();
            entity.Tag = tag ?? CreateNewTag();

            return entity;
        }

        public ServerControlWidget CreateNewServerControlWidget()
        {
            var entity = new ServerControlWidget();            
            var categoryTree = CreateNewCategoryTree();
            var widgetCategory = CreateWidgetCategory(entity, CreateNewCategory(categoryTree));

            PopulateBaseFields(entity);
            PopulateBaseFields(widgetCategory);

            entity.Categories = new List<WidgetCategory>() { widgetCategory };
            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Url = ProvideRandomString(MaxLength.Url);
            entity.PreviewUrl = ProvideRandomString(MaxLength.Url);
            entity.Status = ContentStatus.Published;
            entity.Original = null;
            entity.PublishedByUser = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();

            return entity;
        }

        public HtmlContentWidget CreateNewHtmlContentWidget()
        {
            var entity = new HtmlContentWidget();
            var categoryTree = CreateNewCategoryTree();
            var widgetCategory = CreateWidgetCategory(entity, CreateNewCategory(categoryTree));

            PopulateBaseFields(entity);
            PopulateBaseFields(widgetCategory);

            entity.Categories = new List<WidgetCategory>() { widgetCategory };
            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Html = ProvideRandomString(100);
            entity.UseCustomCss = true;
            entity.CustomCss = ProvideRandomString(100);
            entity.UseCustomJs = true;
            entity.CustomJs = ProvideRandomString(100);
            entity.Status = ContentStatus.Published;
            entity.Original = null;
            entity.PublishedByUser = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();

            return entity;
        }
       
        public HtmlContent CreateNewHtmlContent(int htmlContentLength = 2000)
        {
            var entity = new HtmlContent();

            PopulateHtmlContentProperties(entity);
            entity.Html = ProvideRandomString(htmlContentLength);

            return entity;
        }

        private void PopulateHtmlContentProperties(HtmlContent entity)
        {
            PopulateBaseFields(entity);

            entity.ActivationDate = ProvideRandomDateTime();
            entity.ExpirationDate = ProvideRandomDateTime();
            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Html = ProvideRandomString(100);
            entity.UseCustomCss = true;
            entity.CustomCss = ProvideRandomString(100);
            entity.UseCustomJs = true;
            entity.CustomJs = ProvideRandomString(100);
            entity.Status = ContentStatus.Published;
            entity.Original = null;
            entity.PublishedByUser = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();
            entity.ContentTextMode = ContentTextMode.Html;
        }
       
        public Media CreateNewMedia(MediaType type = MediaType.Image)
        {
            var entity = new Media();

            PopulateBaseFields(entity);

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();

            return entity;
        }

        public MediaFolder CreateNewMediaFolder(bool createParentFolder = true, MediaType type = MediaType.Image)
        {
            var entity = new MediaFolder();

            PopulateBaseFields(entity);

            if (createParentFolder)
            {
                entity.Folder = CreateNewMediaFolder(false);
            }
            else
            {
                entity.Folder = null;
            }

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();

            return entity;
        }

        public MediaFile CreateNewMediaFile(MediaFolder folder = null, MediaType type = MediaType.File)
        {
            var entity = new MediaFile();

            PopulateBaseFields(entity);

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.OriginalFileName = ProvideRandomString(MaxLength.Name);
            entity.OriginalFileExtension = ProvideRandomString(10);
            entity.PublishedOn = ProvideRandomDateTime();
            entity.FileUri = new Uri(@"C:\web\test\content\100200\file.png");
            entity.PublicUrl = "http://bettercms.com/files/file?id=100200";
            entity.Size = ProvideRandomNumber(10, 2000);
            entity.Folder = folder ?? CreateNewMediaFolder(true, type);
            entity.IsUploaded = true;
            entity.IsTemporary = true;
            entity.IsCanceled = true;
            entity.IsArchived = false;

            return entity;
        }

        public MediaImage CreateNewMediaImage(MediaFolder folder = null, MediaType type = MediaType.Image)
        {
            var entity = new MediaImage();

            PopulateBaseFields(entity);

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.OriginalFileName = ProvideRandomString(MaxLength.Name);
            entity.OriginalFileExtension = ProvideRandomString(10);
            entity.PublishedOn = ProvideRandomDateTime();
            entity.FileUri = new Uri(@"C:\Projects\BetterCMS\file100.png");
            entity.PublicUrl = "http://bettercms.com/files/image?id=100200&t=image;";
            entity.Size = ProvideRandomNumber(10, 2000);
            entity.Folder = folder ?? CreateNewMediaFolder(true, type);

            entity.Caption = ProvideRandomString(MaxLength.Text);
            entity.ImageAlign = ProvideRandomEnumValue<MediaImageAlign>();
            entity.Width = ProvideRandomNumber(1, 1024);
            entity.Height = ProvideRandomNumber(1, 1024);
            entity.CropCoordX1 = ProvideRandomNumber(1, 1024);
            entity.CropCoordY1 = ProvideRandomNumber(1, 1024);
            entity.CropCoordX2 = ProvideRandomNumber(1, 1024);
            entity.CropCoordY2 = ProvideRandomNumber(1, 1024);
            entity.OriginalWidth = ProvideRandomNumber(entity.Width, 1024);
            entity.OriginalHeight = ProvideRandomNumber(entity.Height, 1024);
            entity.OriginalSize = ProvideRandomNumber(entity.Size, 4000);
            entity.OriginalUri = new Uri(@"C:\Projects\BetterCMS\o_file100.jpg");
            entity.IsOriginalUploaded = true;
            entity.PublicOriginallUrl = "http://bettercms.com/files/image?id=100200&t=image&o";

            entity.ThumbnailWidth = ProvideRandomNumber(1, 96);
            entity.ThumbnailHeight = ProvideRandomNumber(1, 96);
            entity.ThumbnailSize = ProvideRandomNumber(1, 960);
            entity.ThumbnailUri = new Uri(@"C:\Projects\BetterCMS\t_file100.png");
            entity.IsThumbnailUploaded = true;
            entity.PublicThumbnailUrl = "http://bettercms.com/files/image?id=100200&t=image&p";

            return entity;
        }

        public Sitemap CreateNewSitemap()
        {
            var entity = new Sitemap();

            PopulateBaseFields(entity);
            entity.Title = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public SitemapNode CreateNewSitemapNode(Sitemap sitemap)
        {
            var entity = new SitemapNode();

            PopulateBaseFields(entity);

            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.Url = ProvideRandomString(MaxLength.Url);
            entity.DisplayOrder = ProvideRandomNumber(0, int.MaxValue);
            entity.Sitemap = sitemap;
            
            return entity;
        }

        public Role CreateNewRole()
        {
            var entity = new Role();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Description = ProvideRandomString(MaxLength.Name);
            entity.IsSystematic = ProvideRandomBooleanValue();

            return entity;
        }

        public User CreateNewUser()
        {
            var entity = new User();

            PopulateBaseFields(entity);

            entity.UserName = ProvideRandomString(UsersModuleConstants.UserNameMaxLength);
            entity.Email = ProvideRandomString(MaxLength.Email);
            entity.FirstName = ProvideRandomString(MaxLength.Name);
            entity.LastName = ProvideRandomString(MaxLength.Name);
            entity.Password = ProvideRandomString(MaxLength.Password);
            entity.Salt = ProvideRandomString(MaxLength.Password);

            entity.Image = CreateNewMediaImage();

            return entity;
        }

        public Module.Users.Models.UserRole CreateNewUserRoles(Role role = null, User user = null)
        {
            var entity = new Module.Users.Models.UserRole();

            PopulateBaseFields(entity);

            entity.Role = role ?? CreateNewRole();
            entity.User = user ?? CreateNewUser();

            return entity;
        }

        public AccessRule CreateNewAccessRule()
        {
            var entity = new AccessRule();

            entity.Identity = ProvideRandomString(MaxLength.Name);
            entity.AccessLevel = ProvideRandomEnumValue<AccessLevel>();

            return entity;
        }

        public PageProperties CreateNewPageWithTagsContentsOptionsAndAccessRules(ISession session, int tagsCount = 2, int contentsCount = 2, int optionsCount = 2, int accessRulesCount = 2)
        {
            var page = CreateNewPageProperties();

            if (accessRulesCount > 0)
            {
                for (int i = 0; i < accessRulesCount; i++)
                {
                    page.AddRule(CreateNewAccessRule());
                }
            }

            if (tagsCount > 0)
            {
                for (int i = 0; i < tagsCount; i++)
                {
                    session.SaveOrUpdate(CreateNewPageTag(page));
                }
            }

            if (contentsCount > 0)
            {
                for (int i = 0; i < contentsCount; i++)
                {
                    session.SaveOrUpdate(CreateNewPageContent(null, page));
                }
            }

            if (optionsCount > 0)
            {
                for (int i = 0; i < optionsCount; i++)
                {
                    session.SaveOrUpdate(CreateNewPageOption(page));
                }
            }

            return page;
        }

        public MediaFile CreateNewMediaFileWithAccessRules(int accessRulesCount = 2)
        {
            var file = CreateNewMediaFile();

            if (accessRulesCount > 0)
            {
                for (int i = 0; i < accessRulesCount; i++)
                {
                    file.AddRule(CreateNewAccessRule());
                }
            }

            return file;
        }

        public string CreateChildWidgetAssignment(Guid widgetId, Guid? assignmentId = null)
        {
            return string.Format("<widget data-id=\"{0}\" data-assign-id=\"{1}\">{2}</widget>",
                widgetId,
                assignmentId,
                ProvideRandomString(20));
        }
    }
}