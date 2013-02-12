using System;
using System.Text;

using BetterCms.Api.Interfaces.Models.Enums;

using BetterCms.Core.Models;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Navigation.Models;
using BetterCms.Module.Users.Models;

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
            entity.IsPublished = true;
            entity.PublishedOn = ProvideRandomDateTime();
            entity.Layout = layout ?? CreateNewLayout();

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

            return entity;
        }

        private void PopulatePageProperties(PageProperties entity, Layout layout = null)
        {
            PopulateBaseFields(entity);

            entity.IsPublished = ProvideRandomBooleanValue();
            entity.PageUrl = ProvideRandomString(MaxLength.Url);
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.Description = ProvideRandomString(2000);
            entity.CanonicalUrl = ProvideRandomString(MaxLength.Url);
            entity.CustomCss = ProvideRandomString(2000);
            entity.CustomJS = ProvideRandomString(2000);
            entity.MetaTitle = ProvideRandomString(MaxLength.Name);
            entity.MetaKeywords = ProvideRandomString(MaxLength.Text);
            entity.MetaDescription = ProvideRandomString(MaxLength.Text);

            entity.Layout = layout ?? CreateNewLayout();
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

        public User CreateNewUser()
        {
            var entity = new User();

            PopulateBaseFields(entity);

            entity.UserName = ProvideRandomString(MaxLength.Name);
            entity.Email = ProvideRandomString(MaxLength.Email);
            entity.DisplayName = ProvideRandomString(MaxLength.Name);

            return entity;
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

        public Category CreateNewCategory()
        {
            var entity = new Category();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        //public Approval CreateNewApproval(Content content = null, PageProperties page = null, User publisher = null)
        //{
        //    var entity = new Approval();

        //    PopulateBaseFields(entity);

        //    entity.ApprovalStatus = ProvideRandomEnumValue<ApprovalStatus>();
        //    entity.ApprovedOn = ProvideRandomDateTime();
        //    entity.EditorComments = ProvideRandomString(2000);
        //    entity.PublisherComments = ProvideRandomString(2000);
        //    entity.Content = content ?? CreateNewContent();
        //    entity.Page = page ?? this.CreateNewPageProperties();
        //    entity.Publisher = publisher ?? this.CreateNewUser();
        //    entity.Editor = null;
        //    entity.Draft = null;

        //    return entity;
        //}

        //public ServerControlWidget CreateNewControl()
        //{
        //    var entity = new ServerControlWidget();

        //    PopulateBaseFields(entity);

        //    entity.Name = ProvideRandomString(50);
        //    entity.Url = ProvideRandomString(300);

        //    return entity;
        //}

        //public HtmlContentWidget CreateNewHtmlControl()
        //{
        //    var entity = new HtmlContentWidget();

        //    PopulateBaseFields(entity);

        //    entity.Name = ProvideRandomString(200);
        //    entity.Text = ProvideRandomStringMaxLength();
        //    entity.CustomCss = ProvideRandomString(2000);
        //    entity.UseCustomCss = ProvideRandomBooleanValue();

        //    return entity;
        //}

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

        public PageContentOption CreateNewPageContentOption(PageContent pageContent = null, ContentOption contentOption = null)
        {
            var entity = new PageContentOption();

            PopulateBaseFields(entity);

            entity.PageContent = pageContent ?? CreateNewPageContent();
            entity.Key = ProvideRandomString(MaxLength.Name);
            entity.Value = ProvideRandomString(100);
            entity.Type = ProvideRandomEnumValue<OptionType>();
            
            return entity;
        }
      
        public Widget CreateNewWidget(Category category = null)
        {
            var entity = new Widget();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Category = category ?? CreateNewCategory();
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

            PopulateBaseFields(entity);

            entity.Category = CreateNewCategory();
            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Url = ProvideRandomString(MaxLength.Url);
            entity.Status = ContentStatus.Published;
            entity.Original = null;
            entity.PublishedByUser = ProvideRandomString(MaxLength.Name);
            entity.PublishedOn = ProvideRandomDateTime();

            return entity;
        }

        public HtmlContentWidget CreateNewHtmlContentWidget()
        {
            var entity = new HtmlContentWidget();

            PopulateBaseFields(entity);

            entity.Category = CreateNewCategory();

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
       
        public HtmlContent CreateNewHtmlContent()
        {
            var entity = new HtmlContent();

            PopulateHtmlContentProperties(entity);

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
        }
       
        public Media CreateNewMedia(MediaType type = MediaType.Image)
        {
            var entity = new Media();

            PopulateBaseFields(entity);

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public MediaFolder CreateNewMediaFolder(bool createParentFolder = true, MediaType type = MediaType.Image)
        {
            var entity = new MediaFolder();

            PopulateBaseFields(entity);

            if (createParentFolder)
            {
                entity.ParentFolder = CreateNewMediaFolder(false);
            }
            else
            {
                entity.ParentFolder = null;
            }

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public MediaFile CreateNewMediaFile(MediaFolder folder = null, MediaType type = MediaType.Image)
        {
            var entity = new MediaFile();

            PopulateBaseFields(entity);

            entity.Type = type;
            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.OriginalFileName = ProvideRandomString(MaxLength.Name);
            entity.OriginalFileExtension = ProvideRandomString(10);
            entity.FileUri = new Uri(@"C:\web\test\content\100200\file.png");
            entity.PublicUrl = "http://bettercms.com/files/file?id=100200";
            entity.Size = ProvideRandomNumber(10, 2000);
            entity.Folder = folder ?? CreateNewMediaFolder(true, type);
            entity.IsUploaded = true;
            entity.IsTemporary = true;
            entity.IsCanceled = true;

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

        public SitemapNode CreateNewSitemapNode()
        {
            var entity = new SitemapNode();

            PopulateBaseFields(entity);

            entity.Title = ProvideRandomString(MaxLength.Name);
            entity.Url = ProvideRandomString(MaxLength.Url);
            entity.DisplayOrder = ProvideRandomNumber(0, int.MaxValue);

            return entity;
        }

        public Role CreateNewRole()
        {
            var entity = new Role();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public Permission CreateNewPermission()
        {
            var entity = new Permission();

            PopulateBaseFields(entity);

            entity.Name = ProvideRandomString(MaxLength.Name);
            entity.Description = ProvideRandomString(MaxLength.Name);

            return entity;
        }

        public RolePermissions CreateNewRolePermission(Role role = null, Permission permission = null)
        {
            var entity = new RolePermissions();

            PopulateBaseFields(entity);

            entity.Role = role ?? CreateNewRole();
            entity.Permission = permission ?? CreateNewPermission();

            return entity;
        }
    }
}